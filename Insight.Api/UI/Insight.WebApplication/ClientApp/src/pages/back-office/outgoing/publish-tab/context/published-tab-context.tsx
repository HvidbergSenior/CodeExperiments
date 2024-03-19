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
import { GetOutgoingDeclarationsByPageAndPageSizeResponse } from "../../../../../api/api";
import { useActivityIndicator } from "../../../../../contexts/activity-indicator-context";
import { useFilter } from "../../../../../contexts/filter-context";
import useDeepCompare from "../../../../../hooks/use-deep-compare/use-deep-compare";
import {
  VolumeAndGhgInfo,
  calculateGhgWeightedAverage,
} from "../../../../../util/calculations";
import { isAxiosErrorType } from "../../../../../util/errors/predicates";
import {
  OutgoingApiArgs,
  PublishedContextProps,
  QueryState,
} from "../../../../types";

interface Props {
  children: ReactNode;
}

const PublishedTabContext = createContext<PublishedContextProps>(
  {} as PublishedContextProps,
);

const defaultPageArgs = {
  page: 1,
  pageSize: 100,
  isOrderDescending: false,
  orderByProperty: "country",
};

export const PublishedTabContextProvider: FC<Props> = ({ children }: Props) => {
  const { filterOutgoing } = useFilter();
  const [apiArgs, setApiArgs] = useState<OutgoingApiArgs>({
    ...defaultPageArgs,
    ...filterOutgoing,
  });

  const [queryStateDeclarations, setQueryStateDeclarations] = useState<
    QueryState<GetOutgoingDeclarationsByPageAndPageSizeResponse>
  >({ isLoading: true });
  const [volume, setVolume] = useState<number | undefined>(undefined);
  const [ghgWeightedAvg, setGhgWeightedAvg] = useState<number | undefined>(
    undefined,
  );
  const { showAcitvityIndicator } = useActivityIndicator();

  const getPublishedDeclarations = useCallback(async () => {
    try {
      const response =
        await authorizedHttpClient.api.getOutgoingDeclarationsByPageAndPageSize(
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
            outgoingDeclarationsByPageAndPageSizeResponse: [
              ...(state.data?.outgoingDeclarationsByPageAndPageSizeResponse ??
                []),
              ...response.data.outgoingDeclarationsByPageAndPageSizeResponse,
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
  }, [apiArgs, filterOutgoing]);

  const loadMore = () => {
    if (!queryStateDeclarations.data?.hasMoreDeclarations) {
      return;
    }
    setApiArgs((state) => ({
      ...state,
      page: state.page + 1,
    }));
  };

  useDeepCompare(() => {
    getPublishedDeclarations();
  }, [apiArgs]);

  useDeepCompare(() => {
    showAcitvityIndicator(true);
    setApiArgs((prev) => ({ ...prev, ...filterOutgoing, page: 1 }));
  }, [filterOutgoing]);

  const handleSubmitViewPublishedDeclaration = async (id: string) => {
    // TODO: BKN - we may not need to do anything here
    console.log("View of published declaration complete " + id);
  };

  useEffect(() => {
    if (!queryStateDeclarations.data) {
      return;
    }

    const volumeCalc =
      queryStateDeclarations.data?.outgoingDeclarationsByPageAndPageSizeResponse
        .map((tx) => tx.volumeTotal)
        .reduce((accumulated, value) => accumulated + value, 0);
    setVolume(
      queryStateDeclarations.data?.outgoingDeclarationsByPageAndPageSizeResponse
        .length === 0
        ? undefined
        : volumeCalc,
    );

    // GHG weighted avg
    const volumeAndGhgInfoList =
      queryStateDeclarations.data?.outgoingDeclarationsByPageAndPageSizeResponse.map(
        (tx) =>
          ({
            volume: tx.volumeTotal,
            ghgReduction: tx.ghgReduction,
            volumeMultiplier: tx.fossilFuelComparatorgCO2EqPerMJ,
          }) as VolumeAndGhgInfo,
      );
    if (volumeAndGhgInfoList !== undefined) {
      setGhgWeightedAvg(calculateGhgWeightedAverage(volumeAndGhgInfoList));
    }
  }, [queryStateDeclarations]);

  const props = {
    queryStateDeclarations,
    loadMore,
    handleSubmitViewPublishedDeclaration,
    volume,
    ghgWeightedAvg,
  };

  return (
    <PublishedTabContext.Provider value={props}>
      {children}
    </PublishedTabContext.Provider>
  );
};

export const usePublishedContext = () => useContext(PublishedTabContext);
