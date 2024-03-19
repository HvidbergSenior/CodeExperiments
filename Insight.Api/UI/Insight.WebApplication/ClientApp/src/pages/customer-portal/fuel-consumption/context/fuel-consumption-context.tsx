import {
  FC,
  ReactNode,
  createContext,
  useCallback,
  useContext,
  useState,
} from "react";
import { authorizedHttpClient } from "../../../../api";
import {
  GetFuelConsumptionResponse,
  GetFuelConsumptionTransactionsResponse,
} from "../../../../api/api";
import useDeepCompare from "../../../../hooks/use-deep-compare/use-deep-compare";
import { isAxiosErrorType } from "../../../../util/errors/predicates";
import { QueryState } from "../../../types";
import { useCustomerPortalContext } from "../../customer-portal-context";

interface Props {
  children: ReactNode;
}

interface FuelConsumptionContextProps {
  queryStateFuelConsumption: QueryState<GetFuelConsumptionResponse>;
  queryStateFuelConsumptionTransactions: QueryState<GetFuelConsumptionTransactionsResponse>;
}

const FuelConsumptionContext = createContext<FuelConsumptionContextProps>(
  {} as FuelConsumptionContextProps,
);

const defaultPageArgs = {
  page: 1,
  pageSize: 50,
  isOrderDescending: true,
  orderByProperty: "FuelTransactionTimeStamp",
};

export const FuelConsumptionContextProvider: FC<Props> = ({
  children,
}: Props) => {
  const { filter } = useCustomerPortalContext();

  const [queryStateFuelConsumption, setQueryStateFuelConsumption] = useState<
    QueryState<GetFuelConsumptionResponse>
  >({ isLoading: true });

  const [
    queryStateFuelConsumptionTransactions,
    setQueryStateFuelConsumptionTransactions,
  ] = useState<QueryState<GetFuelConsumptionTransactionsResponse>>({
    isLoading: true,
  });

  const setToLoading = () => {
    setQueryStateFuelConsumption((state) => ({
      ...state,
      isLoading: true,
    }));
  };

  const getFueltConsumptionData = useCallback(async () => {
    setToLoading();
    try {
      const response = await authorizedHttpClient.api.getFuelConsumption({
        dateFrom: filter.fromDate,
        dateTo: filter.toDate,
        maxColumns: 30,
        productNames: filter.fuels,
        customerIds: filter.accountsIds,
        customerNumbers: [],
      });

      setQueryStateFuelConsumption((state) => ({
        ...state,
        data: response.data,
        error: undefined,
        hasError: false,
        isLoading: false,
      }));
    } catch (error) {
      const mappedError = isAxiosErrorType(error);
      setQueryStateFuelConsumption((state) => ({
        ...state,
        data: undefined,
        error: mappedError,
        hasError: true,
        isLoading: false,
      }));
    }
  }, [filter]);

  const getFueltConsumptionTransactionsData = useCallback(async () => {
    try {
      const response =
        await authorizedHttpClient.api.getFuelConsumptionTransactions(
          { ...defaultPageArgs },
          {
            dateTo: filter.toDate,
            dateFrom: filter.fromDate,
            productNames: filter.fuels,
            customerIds: filter.accountsIds,
            customerNumbers: [],
            maxColumns: 30,
          },
        );

      setQueryStateFuelConsumptionTransactions((state) => ({
        ...state,
        data: response.data,
        error: undefined,
        hasError: false,
        isLoading: false,
      }));
    } catch (error) {
      const mappedError = isAxiosErrorType(error);
      setQueryStateFuelConsumptionTransactions((state) => ({
        ...state,
        data: undefined,
        error: mappedError,
        hasError: true,
        isLoading: false,
      }));
    }
  }, [defaultPageArgs, filter]);

  useDeepCompare(() => {
    getFueltConsumptionData();
    getFueltConsumptionTransactionsData();
  }, [filter]);

  const props = {
    queryStateFuelConsumption,
    queryStateFuelConsumptionTransactions,
  };

  return (
    <FuelConsumptionContext.Provider value={props}>
      {children}
    </FuelConsumptionContext.Provider>
  );
};

export const useFuelConsumptionContext = () =>
  useContext(FuelConsumptionContext);
