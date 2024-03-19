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
  GetStockTransactionsQueryResponse,
  StockTransactionResponse,
} from "../../../../../api/api";
import { useFilter } from "../../../../../contexts/filter-context";
import useDeepCompare from "../../../../../hooks/use-deep-compare/use-deep-compare";
import { useSnackBar } from "../../../../../shared/snackbar/use-snackbar";
import { useOrder } from "../../../../../shared/sorting/use-order";
import { outgoingTabTranslations } from "../../../../../translations/pages/outgoing-tab-translations";
import { isAxiosErrorType } from "../../../../../util/errors/predicates";
import useHandleNetworkError from "../../../../../util/errors/use-handle-network-error";
import {
  OutgoingApiArgs,
  QueryState,
  StockContextProps,
} from "../../../../types";
import { useActivityIndicator } from "../../../../../contexts/activity-indicator-context";

interface Props {
  children: ReactNode;
}

const StockTabContext = createContext<StockContextProps>(
  {} as StockContextProps,
);

const defaultPageArgs = {
  page: 1,
  pageSize: 100,
  isOrderDescending: false,
  orderByProperty: "companyName",
};

export const StockTabContextProvider: FC<Props> = ({ children }: Props) => {
  const { filterOutgoing } = useFilter();
  const [apiArgs, setApiArgs] = useState<OutgoingApiArgs>({
    ...defaultPageArgs,
    ...filterOutgoing,
  });
  const { showErrorDialog } = useHandleNetworkError();
  const { showSnackBar } = useSnackBar();
  const { direction, orderBy, getLabelProps } =
    useOrder<keyof StockTransactionResponse>("companyName");
  const { showAcitvityIndicator } = useActivityIndicator();

  const [queryStateStocks, setQueryStateStocks] = useState<
    QueryState<GetStockTransactionsQueryResponse>
  >({ isLoading: true });

  const getStocks = useCallback(async () => {
    try {
      const response = await authorizedHttpClient.api.getStockTransactions({
        ...apiArgs,
      });
      if (apiArgs.page === 1) {
        setQueryStateStocks({
          data: response.data,
          isLoading: false,
        });
      } else {
        setQueryStateStocks((state) => ({
          ...state,
          data: {
            hasMoreStockTransactions: response.data.hasMoreStockTransactions,
            stockTransactions: [
              ...(state.data?.stockTransactions ?? []),
              ...response.data.stockTransactions,
            ],
            totalAmountOfStockTransactions:
              response.data.totalAmountOfStockTransactions,
          },
        }));
      }
    } catch (error) {
      const mappedError = isAxiosErrorType(error);
      setQueryStateStocks((state) => ({
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
    if (!queryStateStocks.data?.hasMoreStockTransactions) {
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

  const handleCreateStockSubmit = async () => {
    setApiArgs((state) => ({
      ...state,
      page: 1,
    }));
    // Because pageArgs is updated the useEffect below will trigger a reload of data.
  };

  useDeepCompare(() => {
    getStocks();
  }, [apiArgs]);

  useDeepCompare(() => {
    showAcitvityIndicator(true);
    setApiArgs((prev) => ({ ...prev, ...filterOutgoing, page: 1 }));
  }, [filterOutgoing]);

  const handleAllocateConfirm = useCallback(async () => {
    try {
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
        await getStocks();
      } else {
        setApiArgs((prev) => ({ ...prev, page: 1 }));
      }
    } catch (error) {
      showErrorDialog(error);
    }
  }, [filterOutgoing]);

  useEffect(() => {
    if (
      apiArgs.isOrderDescending !== (direction === "desc") ||
      apiArgs.orderByProperty !== orderBy
    ) {
      setApiArgs((prev) => ({
        ...prev,
        page: 1,
        orderByProperty: orderBy,
        isOrderDescending: direction === "desc",
      }));
    }
  }, [orderBy, direction]);

  const props = {
    queryStateStocks,
    loadMore,
    handleSubmitManualAllocation,
    handleCreateStockSubmit,
    handleAllocateConfirm,
    getLabelProps,
  };

  return (
    <StockTabContext.Provider value={props}>
      {children}
    </StockTabContext.Provider>
  );
};

export const useStockContext = () => useContext(StockTabContext);
