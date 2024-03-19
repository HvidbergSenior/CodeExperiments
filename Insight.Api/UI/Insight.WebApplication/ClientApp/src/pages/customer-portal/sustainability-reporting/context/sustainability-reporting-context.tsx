import {
  FC,
  ReactNode,
  createContext,
  useCallback,
  useContext,
  useState,
} from "react";
import { authorizedHttpClient } from "../../../../api";
import { GetSustainabilityReportResponse } from "../../../../api/api";
import { QueryState } from "../../../types";
import { useCustomerPortalContext } from "../../customer-portal-context";
import { isAxiosErrorType } from "../../../../util/errors/predicates";
import useDeepCompare from "../../../../hooks/use-deep-compare/use-deep-compare";

interface Props {
  children: ReactNode;
}

interface SustainabilityReportingContextProps {
  queryStateSustainabilityReport: QueryState<GetSustainabilityReportResponse>;
}

const SustainabilityReportingContext =
  createContext<SustainabilityReportingContextProps>(
    {} as SustainabilityReportingContextProps,
  );

export const SustainabilityReportingContextProvider: FC<Props> = ({
  children,
}: Props) => {
  const [queryStateSustainabilityReport, setQueryStateSustainabilityReport] =
    useState<QueryState<GetSustainabilityReportResponse>>({
      isLoading: true,
    });

  const { filter } = useCustomerPortalContext();

  const setToLoading = () => {
    setQueryStateSustainabilityReport((state) => ({
      ...state,
      isLoading: true,
    }));
  };

  const getSustainabilityReportData = useCallback(async () => {
    setToLoading();
    try {
      const response = await authorizedHttpClient.api.getSustainabilityReport({
        dateFrom: filter.fromDate,
        dateTo: filter.toDate,
        maxColumns: 30,
        productNames: filter.fuels,
        customerIds: filter.accountsIds,
        customerNumbers: [],
      });

      setQueryStateSustainabilityReport((state) => ({
        ...state,
        data: response.data,
        error: undefined,
        hasError: false,
        isLoading: false,
      }));
    } catch (error) {
      const mappedError = isAxiosErrorType(error);
      setQueryStateSustainabilityReport((state) => ({
        ...state,
        data: undefined,
        error: mappedError,
        hasError: true,
        isLoading: false,
      }));
    }
  }, [filter]);

  useDeepCompare(() => {
    getSustainabilityReportData();
  }, [filter]);

  const props = { queryStateSustainabilityReport };

  return (
    <SustainabilityReportingContext.Provider value={props}>
      {children}
    </SustainabilityReportingContext.Provider>
  );
};

export const useSustainabilityReportingContext = () =>
  useContext(SustainabilityReportingContext);
