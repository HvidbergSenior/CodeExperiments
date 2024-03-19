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
  AllocationResponse,
  GetAllocationsResponse,
} from "../../../../../api/api";
import { useFilter } from "../../../../../contexts/filter-context";
import useDeepCompare from "../../../../../hooks/use-deep-compare/use-deep-compare";
import { useSnackBar } from "../../../../../shared/snackbar/use-snackbar";
import { useOrder } from "../../../../../shared/sorting/use-order";
import { allocationTabTranslations } from "../../../../../translations/pages/allocation-tab-translations";

import { useActivityIndicator } from "../../../../../contexts/activity-indicator-context";
import {
  VolumeAndGhgInfo,
  calculateGhgWeightedAverage,
} from "../../../../../util/calculations";
import { isAxiosErrorType } from "../../../../../util/errors/predicates";
import useHandleNetworkError from "../../../../../util/errors/use-handle-network-error";
import {
  AllocationContextProps,
  OutgoingApiArgs,
  QueryState,
} from "../../../../types";

interface Props {
  children: ReactNode;
}

const AllocationTabContext = createContext<AllocationContextProps>(
  {} as AllocationContextProps,
);

const defaultPageArgs = {
  page: 1,
  pageSize: 100,
  isOrderDescending: false,
  orderByProperty: "company",
};

export const AllocationTabContextProvider: FC<Props> = ({
  children,
}: Props) => {
  const { filterOutgoing } = useFilter();
  const [apiArgs, setApiArgs] = useState<OutgoingApiArgs>({
    ...defaultPageArgs,
    ...filterOutgoing,
  });
  const { direction, orderBy, getLabelProps } =
    useOrder<keyof AllocationResponse>("company");
  const [volume, setVolume] = useState<number | undefined>(undefined);
  const [ghgWeightedAvg, setGhgWeightedAvg] = useState<number | undefined>(
    undefined,
  );

  const { showSnackBar } = useSnackBar();
  const { showErrorDialog } = useHandleNetworkError();
  const [queryStateAllocations, setQueryStateAllocations] = useState<
    QueryState<GetAllocationsResponse>
  >({ isLoading: true });
  const { showAcitvityIndicator } = useActivityIndicator();

  const getAllocations = useCallback(async () => {
    try {
      const response = await authorizedHttpClient.api.getAllocations({
        ...apiArgs,
      });
      if (apiArgs.page === 1) {
        setQueryStateAllocations({
          data: response.data,
          isLoading: false,
        });
      } else {
        setQueryStateAllocations((state) => ({
          ...state,
          data: {
            hasMoreAllocations: response.data.hasMoreAllocations,
            allocations: [
              ...(state.data?.allocations ?? []),
              ...response.data.allocations,
            ],
            isDraftLocked: response.data.isDraftLocked,
            totalAmountOfAllocations: response.data.totalAmountOfAllocations,
          },
        }));
      }
    } catch (error) {
      const mappedError = isAxiosErrorType(error);
      setQueryStateAllocations((state) => ({
        ...state,
        data: undefined,
        error: mappedError,
        hasError: true,
        isLoading: false,
      }));
    } finally {
      showAcitvityIndicator(false);
    }
  }, [apiArgs, filterOutgoing]);

  const loadMore = () => {
    if (!queryStateAllocations.data?.hasMoreAllocations) {
      return;
    }
    setApiArgs((state) => ({
      ...state,
      page: state.page + 1,
    }));
  };

  useDeepCompare(() => {
    getAllocations();
  }, [apiArgs]);

  useDeepCompare(() => {
    showAcitvityIndicator(true);
    setApiArgs((prev) => ({ ...prev, ...filterOutgoing, page: 1 }));
  }, [filterOutgoing]);

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

  const handleSubmitViewAllocation = async (allocationId: string) => {
    // TODO: BKN - we may not need to do anything here
    console.log("Manual allocation complete " + allocationId);
  };

  const resetTable = useCallback(() => {
    if (apiArgs.page === 1) {
      getAllocations();
    } else {
      setApiArgs((prev) => ({
        ...prev,
        page: 1,
      }));
    }
  }, [apiArgs, setApiArgs]);

  const publishAllocations = async () => {
    try {
      await authorizedHttpClient.api.publishAllocations();
      showSnackBar(
        allocationTabTranslations.snackbarMessages
          .publishAllocationsSuccessfull,
        "success",
      );
      resetTable();
    } catch (error) {
      showErrorDialog(error);
    }
  };

  useEffect(() => {
    if (!queryStateAllocations.data) {
      return;
    }
    const volumeCalc = queryStateAllocations.data.allocations
      .map((tx) => tx.volume)
      .reduce((accumulated, value) => accumulated + value, 0);
    setVolume(
      queryStateAllocations.data.allocations.length === 0
        ? undefined
        : volumeCalc,
    );

    // GHG weighted avg
    const volumeAndGhgInfoList = queryStateAllocations.data?.allocations.map(
      (tx) =>
        ({
          volume: tx.volume,
          ghgReduction: tx.ghgReduction,
          volumeMultiplier: tx.fossilFuelComparatorgCO2EqPerMJ,
        }) as VolumeAndGhgInfo,
    );
    if (volumeAndGhgInfoList !== undefined) {
      setGhgWeightedAvg(calculateGhgWeightedAverage(volumeAndGhgInfoList));
    }
  }, [queryStateAllocations]);

  const clearAllocations = async () => {
    try {
      await authorizedHttpClient.api.clearAllocations();
      showSnackBar(
        allocationTabTranslations.snackbarMessages.clearAllocationsSuccess,
        "success",
      );
      resetTable();
    } catch (error) {
      showErrorDialog(error);
    }
  };

  const lockAllocations = async () => {
    try {
      await authorizedHttpClient.api.lockAllocations();
      showSnackBar(
        allocationTabTranslations.snackbarMessages.lockAllocationsSuccess,
        "success",
      );
      resetTable();
    } catch (error) {
      showErrorDialog(error);
    }
  };

  const unlockAllocations = async () => {
    try {
      await authorizedHttpClient.api.unlockAllocations();
      showSnackBar(
        allocationTabTranslations.snackbarMessages.unlockAllocationsSuccess,
        "success",
      );
      resetTable();
    } catch (error) {
      showErrorDialog(error);
    }
  };

  const props = {
    queryStateAllocations,
    loadMore,
    getLabelProps,
    handleSubmitViewAllocation,
    ghgWeightedAvg,
    volume,
    publishAllocations,
    lockAllocations,
    unlockAllocations,
    clearAllocations,
  };

  return (
    <AllocationTabContext.Provider value={props}>
      {children}
    </AllocationTabContext.Provider>
  );
};

export const useAllocationContext = () => useContext(AllocationTabContext);
