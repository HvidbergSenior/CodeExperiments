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
import { reconciliationTranslations } from "../../../../../translations/pages/reconcilliation-translations";
import { isAxiosErrorType } from "../../../../../util/errors/predicates";
import useHandleNetworkError from "../../../../../util/errors/use-handle-network-error";
import {
  IncomingApiArgs,
  IncomingResponseKeys,
  QueryState,
  ReconciliationContextProps,
} from "../../../../types";

interface Props {
  children: ReactNode;
}

const ReconciliationContext = createContext<ReconciliationContextProps>(
  {} as ReconciliationContextProps,
);

// page may not be below 1
const defaultPageArgs = {
  page: 1,
  pageSize: 100,
  isOrderDescending: false,
  orderByProperty: "company",
};

export const ReconciliationContextProvider: FC<Props> = ({
  children,
}: Props) => {
  const { filterIncoming } = useFilter();
  const [apiArgs, setApiArgs] = useState<IncomingApiArgs>({
    ...defaultPageArgs,
    ...filterIncoming,
  });
  const { direction, orderBy, getLabelProps } =
    useOrder<IncomingResponseKeys>("company");
  const { showSnackBar } = useSnackBar();
  const { showErrorDialog } = useHandleNetworkError();
  const [volume, setVolume] = useState<number | undefined>(undefined);
  const { showAcitvityIndicator } = useActivityIndicator();

  const [queryStateReconciliation, setQueryStateReconciliation] = useState<
    QueryState<GetIncomingDeclarationsByPageAndPageSizeResponse>
  >({ isLoading: true });

  const getIncomingDeclarations = useCallback(async () => {
    try {
      const response =
        await authorizedHttpClient.api.getReconciledIncomingDeclarations({
          ...apiArgs,
        });
      if (apiArgs.page === 1) {
        setQueryStateReconciliation({
          data: response.data,
          isLoading: false,
        });
      } else {
        setQueryStateReconciliation((state) => ({
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
      setQueryStateReconciliation((state) => ({
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

  const loadMore = () => {
    if (!queryStateReconciliation.data?.hasMoreDeclarations) {
      return;
    }
    setApiArgs((state) => ({
      ...state,
      page: state.page + 1,
    }));
  };

  useDeepCompare(() => {
    getIncomingDeclarations();
  }, [apiArgs]);

  useDeepCompare(() => {
    showAcitvityIndicator(true);
    setApiArgs((prev) => ({ ...prev, ...filterIncoming, page: 1 }));
  }, [filterIncoming]);

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

  const saveEditedDeclaration = async (
    declarationToBeUpdated: IncomingDeclarationDto,
  ) => {
    try {
      await authorizedHttpClient.api.updateIncomingDeclaration(
        declarationToBeUpdated.incomingDeclarationId,
        declarationToBeUpdated,
      );
      showSnackBar(
        reconciliationTranslations.snackbarMessages.saveEditSuccessfulMessage,
        "success",
      );
      setApiArgs((state) => ({
        ...state,
        page: 1,
      }));
    } catch (error) {
      showErrorDialog(error);
    }
  };

  useEffect(() => {
    if (!queryStateReconciliation.data) {
      return;
    }
    const volumeCalc =
      queryStateReconciliation.data?.incomingDeclarationsByPageAndPageSize
        .map((tx) => tx.quantity)
        .reduce((accumulated, value) => accumulated + value, 0);
    setVolume(volumeCalc === 0 ? undefined : volumeCalc);
  }, [queryStateReconciliation]);

  const reconciliationProps = {
    queryStateReconciliation,
    loadMore,
    getLabelProps,
    saveEditedDeclaration,
    volume,
  };

  return (
    <ReconciliationContext.Provider value={reconciliationProps}>
      {children}
    </ReconciliationContext.Provider>
  );
};

export const useReconciliationContext = () => useContext(ReconciliationContext);
