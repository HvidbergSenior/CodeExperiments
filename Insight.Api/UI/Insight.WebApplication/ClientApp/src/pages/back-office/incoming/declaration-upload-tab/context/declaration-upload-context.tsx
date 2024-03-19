import {
  FC,
  ReactNode,
  createContext,
  useCallback,
  useContext,
  useEffect,
  useState,
} from "react";
import { authorizedHttpClient } from "../../../../../api";
import {
  GetIncomingDeclarationsByPageAndPageSizeResponse,
  IncomingDeclarationDto,
} from "../../../../../api/api";
import { useActivityIndicator } from "../../../../../contexts/activity-indicator-context";
import { useFilter } from "../../../../../contexts/filter-context";
import useDeepCompare from "../../../../../hooks/use-deep-compare/use-deep-compare";
import { useSnackBar } from "../../../../../shared/snackbar/use-snackbar";
import { useOrder } from "../../../../../shared/sorting/use-order";
import { declarationUploadTranslations } from "../../../../../translations/pages/declaration-upload-translations";
import { incomingTranslations } from "../../../../../translations/pages/incoming-translations";
import { isAxiosErrorType } from "../../../../../util/errors/predicates";
import useHandleNetworkError from "../../../../../util/errors/use-handle-network-error";
import {
  DeclarationUploadContextProps,
  IncomingApiArgs,
  IncomingResponseKeys,
  QueryState,
} from "../../../../types";

interface Props {
  children: ReactNode;
}

const DeclarationUploadContext = createContext<DeclarationUploadContextProps>(
  {} as DeclarationUploadContextProps,
);

// page may not be below 1
const defaultPageArgs = {
  page: 1,
  pageSize: 100,
  isOrderDescending: false,
  orderByProperty: "company",
};

export const DeclarationUploadContextProvider: FC<Props> = ({
  children,
}: Props) => {
  const { filterIncoming, setFilterIncoming } = useFilter();
  const [apiArgs, setApiArgs] = useState<IncomingApiArgs>({
    ...defaultPageArgs,
    ...filterIncoming,
  });
  const { showErrorDialog } = useHandleNetworkError();
  const { showSnackBar } = useSnackBar();
  const { direction, orderBy, getLabelProps } =
    useOrder<IncomingResponseKeys>("company");
  const [selectedRows, setSelectedRows] = useState<string[]>([]);
  const [volume, setVolume] = useState<number | undefined>(undefined);
  const { showAcitvityIndicator } = useActivityIndicator();
  const [queryStateDeclarations, setQueryStateDeclarations] = useState<
    QueryState<GetIncomingDeclarationsByPageAndPageSizeResponse>
  >({ isLoading: true });

  const getIncomingDeclarations = useCallback(async () => {
    try {
      const response =
        await authorizedHttpClient.api.getIncomingDeclarationsByPageAndPageSize(
          {
            ...apiArgs,
          },
        );
      if (apiArgs.page === 1) {
        setQueryStateDeclarations({
          data: response.data,
          isLoading: false,
        });
      } else {
        setQueryStateDeclarations((state) => ({
          ...state,
          data: {
            hasMoreDeclarations: response.data.hasMoreDeclarations,
            incomingDeclarationsByPageAndPageSize: [
              ...(state.data?.incomingDeclarationsByPageAndPageSize ?? []),
              ...response.data.incomingDeclarationsByPageAndPageSize,
            ],
            totalAmountOfDeclarations: response.data.totalAmountOfDeclarations,
          },
        }));
      }
    } catch (error) {
      const mappedError = isAxiosErrorType(error);
      setQueryStateDeclarations((state) => ({
        ...state,
        data: undefined,
        error: mappedError,
        hasError: true,
        isLoading: false,
      }));
    } finally {
      showAcitvityIndicator(false);
    }
  }, [apiArgs, filterIncoming]);

  const loadMore = useCallback(() => {
    if (!queryStateDeclarations.data?.hasMoreDeclarations) {
      return;
    }
    setApiArgs((state) => ({
      ...state,
      page: state.page + 1,
    }));
  }, [queryStateDeclarations.data?.hasMoreDeclarations]);

  useDeepCompare(() => {
    getIncomingDeclarations();
  }, [apiArgs]);

  useDeepCompare(() => {
    showAcitvityIndicator(true);
    setApiArgs((prev) => ({ ...prev, ...filterIncoming, page: 1 }));
  }, [filterIncoming]);

  const selectRow = useCallback(
    (id: string) => {
      if (selectedRows.includes(id)) {
        setSelectedRows((current) => current.filter((rowId) => id !== rowId));
      } else {
        setSelectedRows((current) => [...current, id]);
      }
    },
    [setSelectedRows, selectedRows],
  );

  const selectAllRows = useCallback(() => {
    const allIds =
      queryStateDeclarations.data?.incomingDeclarationsByPageAndPageSize.map(
        (c) => c.id,
      );
    if (
      selectedRows.length ===
      queryStateDeclarations.data?.incomingDeclarationsByPageAndPageSize.length
    ) {
      setSelectedRows([]);
    } else {
      setSelectedRows(allIds ?? []);
    }
  }, [queryStateDeclarations, setSelectedRows, selectedRows]);

  const handleSubmitOfUploadDeclarations = async (
    uploadId: string,
    oldestEntryDate?: string,
    newestEntryDate?: string,
  ) => {
    try {
      await authorizedHttpClient.api.approveIncomingDeclarationUpload({
        incomingDeclarationUploadId: uploadId,
      });

      showSnackBar(
        incomingTranslations.declarationUploadTranslations.snackbarMessages
          .uploadDeclarationsSuccessMessage,
        "success",
      );

      if (oldestEntryDate === undefined || newestEntryDate === undefined) {
        if (apiArgs.page === 1) {
          await getIncomingDeclarations();
        } else {
          setApiArgs((state) => ({
            ...state,
            page: 1,
          }));
        }
      } else {
        if (apiArgs.page === 1) {
          setFilterIncoming((state) => ({
            ...state,
            dateFrom: oldestEntryDate,
            dateTo: newestEntryDate,
          }));
          // Because filter is updated the useEffect above will trigger a reload of data.
        } else {
          setApiArgs((state) => ({
            ...state,
            page: 1,
          }));
          setFilterIncoming((state) => ({
            ...state,
            dateFrom: oldestEntryDate,
            dateTo: newestEntryDate,
          }));
        }
      }
    } catch (error) {
      showErrorDialog(error);
    }
  };

  useEffect(() => {
    if (
      apiArgs.isOrderDescending !== (direction === "desc") ||
      apiArgs.orderByProperty !== orderBy
    ) {
      showAcitvityIndicator(true);
      setApiArgs((prev) => ({
        ...prev,
        page: 1,
        orderByProperty: orderBy,
        isOrderDescending: direction === "desc",
      }));
    }
  }, [orderBy, direction]);

  useEffect(() => {
    if (!queryStateDeclarations.data) {
      return;
    }

    const volumeCalc =
      queryStateDeclarations.data?.incomingDeclarationsByPageAndPageSize
        .map((tx) => tx.quantity)
        .reduce((accumulated, value) => accumulated + value, 0);
    setVolume(volumeCalc === 0 ? undefined : volumeCalc);
  }, [queryStateDeclarations]);

  const reconcileDeclarations = useCallback(async () => {
    try {
      // reconcile the selected declarations
      await authorizedHttpClient.api.reconcileIncomingDeclaration({
        incomingDeclarationIds: selectedRows,
      });
      setSelectedRows([]);
      showSnackBar(
        incomingTranslations.declarationUploadTranslations.snackbarMessages
          .reconcileDeclarationSuccessMessage,
        "success",
      );
      // refetch incoming declaration as the reconciled declarations will have been removed from the list
      if (apiArgs.page === 1) {
        await getIncomingDeclarations();
      } else {
        setApiArgs((state) => ({
          ...state,
          page: 1,
        }));
      }

      // Because pageArgs is updated the useEffect above will trigger a reload of data.
    } catch (error) {
      showErrorDialog(error);
    }
  }, [selectedRows]);

  const saveEditedDeclaration = async (
    declarationToBeUpdated: IncomingDeclarationDto,
  ) => {
    try {
      await authorizedHttpClient.api.updateIncomingDeclaration(
        declarationToBeUpdated.incomingDeclarationId,
        declarationToBeUpdated,
      );
      showSnackBar(
        declarationUploadTranslations.snackbarMessages.saveEditSuccessMessage,
        "success",
      );
      await getIncomingDeclarations();
    } catch (error) {
      showErrorDialog(error);
    }
  };

  const incomingProps = {
    queryStateDeclarations,
    selectRow,
    selectedRows,
    selectAllRows,
    handleSubmitOfUploadDeclarations,
    loadMore,
    reconcileDeclarations,
    getLabelProps,
    saveEditedDeclaration,
    volume,
  };

  return (
    <DeclarationUploadContext.Provider value={incomingProps}>
      {children}
    </DeclarationUploadContext.Provider>
  );
};

export const useDeclarationUploadContext = () =>
  useContext(DeclarationUploadContext);
