import { useCallback, useEffect, useState } from "react";
import { authorizedHttpClient } from "../../../../../api";
import {
  GetAllocationSuggestionsResponse,
  OutgoingFuelTransactionResponse,
  SuggestionResponse,
} from "../../../../../api/api";
import { useSnackBar } from "../../../../../shared/snackbar/use-snackbar";
import { useOrder } from "../../../../../shared/sorting/use-order";
import { outgoingTabTranslations } from "../../../../../translations/pages/outgoing-tab-translations";
import { isAxiosErrorType } from "../../../../../util/errors/predicates";
import useHandleNetworkError from "../../../../../util/errors/use-handle-network-error";
import {
  AllocationSuggestionPageArgs,
  OutgoingFuelTransactionResponseWithWarnings,
  QueryState,
} from "../../../../types";

export type SelectedAmount = {
  id: string;
  volume: number;
};

interface Props {
  initialFuelTransaction: OutgoingFuelTransactionResponse;
  filterDates: { fromDate: string; toDate: string };
}

export const useManualAllocation = ({
  initialFuelTransaction,
  filterDates,
}: Props) => {
  const { showErrorDialog } = useHandleNetworkError();
  const [isLoading, setIsLoading] = useState(false);
  const [volume, setVolume] = useState<number | undefined>(undefined);
  const [isSubmitDisabled, setIsSubmitDisabled] = useState(true);
  let fuelTransactionWithWarnings: OutgoingFuelTransactionResponseWithWarnings =
    { ...initialFuelTransaction, hasWarnings: false, warnings: [] };
  const [shownFuelTransaction, setShownFuelTransaction] = useState(
    fuelTransactionWithWarnings,
  );
  const { showSnackBar } = useSnackBar();

  // page may not be below 1
  const defaultPageArgs = {
    page: 1,
    pageSize: 100,
    isOrderDescending: false,
    orderByProperty: "company",
    customerId: initialFuelTransaction.customerId,
    startDate: filterDates.fromDate,
    endDate: filterDates.toDate,
    product: initialFuelTransaction.productName,
    country: initialFuelTransaction.country,
    location: initialFuelTransaction.location,
  };

  const [pageArgs, setPageArgs] =
    useState<AllocationSuggestionPageArgs>(defaultPageArgs);
  const { direction, orderBy, getLabelProps } =
    useOrder<keyof SuggestionResponse>("company");

  const [
    queryStateAllocationSuggestionDeclarations,
    setQueryStateAllocationSuggestionDeclarations,
  ] = useState<QueryState<GetAllocationSuggestionsResponse>>({
    isLoading: true,
  });

  const getAllocationSuggestionDeclarations = useCallback(async () => {
    try {
      const response =
        await authorizedHttpClient.api.getAllocationSuggestions(pageArgs);
      if (pageArgs.page === 1) {
        setQueryStateAllocationSuggestionDeclarations({
          data: response.data,
          isLoading: false,
        });
      } else {
        setQueryStateAllocationSuggestionDeclarations((state) => ({
          ...state,
          data: {
            hasMoreSuggestions: response.data.hasMoreSuggestions,
            suggestions: [
              ...(state.data?.suggestions ?? []),
              ...response.data.suggestions,
            ],
            totalAmountOfSuggestions: response.data.totalAmountOfSuggestions,
          },
        }));
      }
    } catch (error) {
      const mappedError = isAxiosErrorType(error);
      setQueryStateAllocationSuggestionDeclarations((state) => ({
        ...state,
        data: undefined,
        error: mappedError,
        hasError: true,
        isLoading: false,
      }));
    }
  }, [pageArgs]);

  const loadMore = () => {
    if (!queryStateAllocationSuggestionDeclarations.data?.hasMoreSuggestions) {
      return;
    }
    setPageArgs((state) => ({
      ...state,
      page: state.page + 1,
    }));
  };

  useEffect(() => {
    getAllocationSuggestionDeclarations();
  }, [pageArgs]);

  const [selectedData, setSelectedData] = useState<SelectedAmount[]>([]);

  const selectAsManyAsPossibleFromAll = useCallback(() => {
    let suggestions =
      queryStateAllocationSuggestionDeclarations.data?.suggestions;
    if (suggestions === undefined) {
      return;
    }
    selectAsManyAsPossibleFromList(suggestions);
  }, [queryStateAllocationSuggestionDeclarations]);

  const selectAsManyAsPossibleFromSelectedRows = useCallback(
    (newSelectedRows: string[]) => {
      let suggestions: SuggestionResponse[] = [];

      newSelectedRows.forEach((id) => {
        let suggestion =
          queryStateAllocationSuggestionDeclarations.data?.suggestions.find(
            (e) => e.id === id,
          );

        if (suggestion !== undefined) {
          suggestions = [...suggestions, suggestion];
        }
      });

      selectAsManyAsPossibleFromList(suggestions);
    },
    [queryStateAllocationSuggestionDeclarations],
  );

  const selectAsManyAsPossibleFromList = useCallback(
    (suggestions: SuggestionResponse[]) => {
      let manuallyAllocatedVolume = 0;
      let missingAllocation = initialFuelTransaction.missingAllocationQuantity;
      let warnings: string[] = [];
      let selectedDataList: SelectedAmount[] = [];

      if (
        initialFuelTransaction.quantity <= 0 ||
        initialFuelTransaction.missingAllocationQuantity <= 0
      ) {
        // Calculations below requires a volume/missing volume greater then zero
        return;
      }

      for (let suggestion of suggestions) {
        if (missingAllocation <= 0) {
          break;
        }
        let volumeToAdd = 0;
        if (missingAllocation - suggestion.volumeAvailable >= 0) {
          volumeToAdd = suggestion.volumeAvailable;
        } else {
          volumeToAdd = missingAllocation;
        }
        manuallyAllocatedVolume = manuallyAllocatedVolume + volumeToAdd;
        missingAllocation = missingAllocation - volumeToAdd;
        warnings = [...warnings, ...suggestion.warnings];
        const dataToAdd = { id: suggestion.id, volume: volumeToAdd };
        selectedDataList = [...selectedDataList, dataToAdd];
      }

      setSelectedData(selectedDataList);

      const missingPercentage =
        100 - initialFuelTransaction.alreadyAllocatedPercentage;
      const percentagePerVolumeUnit =
        missingPercentage / initialFuelTransaction.missingAllocationQuantity;
      const manuallyAddedPercentage =
        percentagePerVolumeUnit * manuallyAllocatedVolume;
      const alreadyAllocatedPercentage =
        initialFuelTransaction.alreadyAllocatedPercentage +
        manuallyAddedPercentage;

      setShownFuelTransaction((transaction) => ({
        ...transaction,
        allocatedQuantity:
          initialFuelTransaction.allocatedQuantity + manuallyAllocatedVolume,
        missingAllocationQuantity: missingAllocation,
        alreadyAllocatedPercentage: alreadyAllocatedPercentage,
        hasWarnings: warnings.length > 0,
        warnings: warnings,
      }));

      setSelectedRows(selectedDataList.map((e) => e.id));
      setIsSubmitDisabled(selectedDataList.length === 0);
    },
    [
      queryStateAllocationSuggestionDeclarations,
      shownFuelTransaction,
      setShownFuelTransaction,
    ],
  );

  const [selectedRows, setSelectedRows] = useState<string[]>([]);

  const selectRow = useCallback(
    (id: string) => {
      let newSelectedRows = selectedRows;
      if (newSelectedRows.includes(id)) {
        newSelectedRows = newSelectedRows.filter((e) => id !== e);
      } else {
        if (shownFuelTransaction.missingAllocationQuantity <= 0) {
          // Cannot select more - Show snackbar
          showSnackBar(
            outgoingTabTranslations.manualAllocationDialog
              .allocationFullWarning,
            "info",
          );
          return;
        }
        newSelectedRows = [...newSelectedRows, id];
      }
      selectAsManyAsPossibleFromSelectedRows(newSelectedRows);
    },
    [setSelectedRows, selectedRows, queryStateAllocationSuggestionDeclarations],
  );

  const selectAllRows = useCallback(() => {
    if (selectedRows.length > 0) {
      selectAsManyAsPossibleFromList([]);
    } else {
      if (shownFuelTransaction.missingAllocationQuantity <= 0) {
        // Cannot select more - Show snackbar
        showSnackBar(
          outgoingTabTranslations.manualAllocationDialog.allocationFullWarning,
          "info",
        );
        return;
      }
      selectAsManyAsPossibleFromAll();
    }
  }, [
    queryStateAllocationSuggestionDeclarations,
    setSelectedRows,
    selectedRows,
  ]);

  useEffect(() => {
    if (
      pageArgs.isOrderDescending !== (direction === "desc") ||
      pageArgs.orderByProperty !== orderBy
    ) {
      setPageArgs({
        ...defaultPageArgs,
        orderByProperty: orderBy,
        isOrderDescending: direction === "desc",
      });
    }
  }, [orderBy, direction]);

  useEffect(() => {
    const volumeCalc =
      queryStateAllocationSuggestionDeclarations.data?.suggestions
        .map((tx) => tx.volumeAvailable)
        .reduce((accumulated, value) => accumulated + value, 0);
    setVolume(
      queryStateAllocationSuggestionDeclarations.data?.suggestions.length === 0
        ? undefined
        : volumeCalc,
    );
  }, [queryStateAllocationSuggestionDeclarations]);
  const mapToAllocation = (e: SelectedAmount) => {
    return { incomingDeclarationId: e.id, volume: e.volume };
  };

  const submitManualAllocation = useCallback(async () => {
    try {
      setIsLoading(true);
      await authorizedHttpClient.api.postManualAllocation({
        fuelTransactionsBatch: {
          customerId: initialFuelTransaction.customerId,
          startDate: defaultPageArgs.startDate,
          endDate: defaultPageArgs.endDate,
          productNumber: initialFuelTransaction.productNumber,
          country: initialFuelTransaction.country,
          stationName: initialFuelTransaction.stationName,
          productName: initialFuelTransaction.productName,
          locationId: initialFuelTransaction.locationId,
        },
        allocations: selectedData.map((e) => mapToAllocation(e)),
      });
      showSnackBar(
        outgoingTabTranslations.manualAllocationDialog.allocationComplete,
        "success",
      );
      setIsLoading(false);
      return true;
    } catch (error) {
      showErrorDialog(error);
      setIsLoading(false);
      return false;
    }
  }, [selectedData]);

  return {
    currentFuelTransaction: shownFuelTransaction,
    queryStateAllocationSuggestionDeclarations,
    isLoading,
    isSubmitDisabled,
    setIsLoading,
    loadMore,
    selectRow,
    selectedRows,
    selectedData,
    selectAllRows,
    getLabelProps,
    volume,
    submitManualAllocation,
    pageArgs,
    setPageArgs,
  };
};
