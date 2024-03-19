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
import { GetOutgoingFuelTransactionsQueryResponse } from "../../../../../api/api";
import { useActivityIndicator } from "../../../../../contexts/activity-indicator-context";
import { useFilter } from "../../../../../contexts/filter-context";
import useDeepCompare from "../../../../../hooks/use-deep-compare/use-deep-compare";
import { useSnackBar } from "../../../../../shared/snackbar/use-snackbar";
import { useOrder } from "../../../../../shared/sorting/use-order";
import { outgoingTabTranslations } from "../../../../../translations/pages/outgoing-tab-translations";
import { isAxiosErrorType } from "../../../../../util/errors/predicates";
import useHandleNetworkError from "../../../../../util/errors/use-handle-network-error";
import {
  OutgoingApiArgs,
  OutgoingContextProps,
  OutgoingFuelTransactionKeys,
  QueryState,
} from "../../../../types";

interface Props {
  children: ReactNode;
}

const OutgoingTabContext = createContext<OutgoingContextProps>(
  {} as OutgoingContextProps,
);

const defaultPageArgs = {
  page: 1,
  pageSize: 100,
  isOrderDescending: false,
  orderByProperty: "customerName",
};

export const OutgoingTabContextProvider: FC<Props> = ({ children }: Props) => {
  const { filterOutgoing } = useFilter();
  const [apiArgs, setApiArgs] = useState<OutgoingApiArgs>({
    ...defaultPageArgs,
    ...filterOutgoing,
  });
  const { showErrorDialog } = useHandleNetworkError();
  const { showSnackBar } = useSnackBar();
  const { direction, orderBy, getLabelProps } =
    useOrder<OutgoingFuelTransactionKeys>("customerName");
  const [queryStateFuelTransactions, setQueryStateFuelTransactions] = useState<
    QueryState<GetOutgoingFuelTransactionsQueryResponse>
  >({ isLoading: true });
  const { showAcitvityIndicator } = useActivityIndicator();

  const getOutgoingFuelTransactions = useCallback(async () => {
    try {
      const response =
        await authorizedHttpClient.api.getOutgoingFuelTransactions({
          ...apiArgs,
        });
      if (apiArgs.page === 1) {
        setQueryStateFuelTransactions({
          data: response.data,
          isLoading: false,
        });
      } else {
        setQueryStateFuelTransactions((state) => ({
          ...state,
          data: {
            hasMoreOutgoingFuelTransactions:
              response.data.hasMoreOutgoingFuelTransactions,
            outgoingFuelTransactions: [
              ...(state.data?.outgoingFuelTransactions ?? []),
              ...response.data.outgoingFuelTransactions,
            ],
            totalAmountOfOutgoingFuelTransactions:
              response.data.totalAmountOfOutgoingFuelTransactions,
            totalQuantity: response.data.totalQuantity,
          },
        }));
      }
    } catch (error) {
      const mappedError = isAxiosErrorType(error);
      setQueryStateFuelTransactions((state) => ({
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
    if (!queryStateFuelTransactions.data?.hasMoreOutgoingFuelTransactions) {
      return;
    }
    setApiArgs((state) => ({
      ...state,
      page: state.page + 1,
    }));
  };

  const handleSubmitManualAllocation = async () => {
    setApiArgs((state) => ({
      ...state,
      page: 1,
    }));
    // Because pageArgs is updated the useEffect below will trigger a reload of data.
  };

  useDeepCompare(() => {
    getOutgoingFuelTransactions();
  }, [apiArgs]);

  useDeepCompare(() => {
    showAcitvityIndicator(true);
    setApiArgs((prev) => ({ ...prev, ...filterOutgoing, page: 1 }));
  }, [filterOutgoing]);

  const handleAllocateConfirm = useCallback(async () => {
    try {
      showAcitvityIndicator(true, outgoingTabTranslations.allocating);
      await authorizedHttpClient.api.postAutomaticAllocation({
        startDate: filterOutgoing.dateFrom,
        endDate: filterOutgoing.dateTo,
        company: filterOutgoing.company,
        customer: filterOutgoing.customer,
        product: filterOutgoing.product,
      });

      showSnackBar(
        outgoingTabTranslations.snackbarMessages.automaticAllocationSuccess,
        "success",
      );

      if (apiArgs.page === 1) {
        await getOutgoingFuelTransactions();
      } else {
        setApiArgs((prev) => ({ ...prev, page: 1 }));
      }
    } catch (error) {
      showErrorDialog(error);
    } finally {
      showAcitvityIndicator(false);
    }
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

  const props = {
    queryStateFuelTransactions: queryStateFuelTransactions,
    loadMore,
    handleSubmitManualAllocation,
    handleAllocateConfirm,
    getLabelProps,
  };

  return (
    <OutgoingTabContext.Provider value={props}>
      {children}
    </OutgoingTabContext.Provider>
  );
};

export const useOutgoingContext = () => useContext(OutgoingTabContext);
