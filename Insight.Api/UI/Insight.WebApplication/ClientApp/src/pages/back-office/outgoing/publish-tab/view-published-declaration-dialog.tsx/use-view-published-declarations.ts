import { useCallback, useEffect, useState } from "react";
import { authorizedHttpClient } from "../../../../../api";
import {
  GetOutgoingDeclarationIncomingDeclarationResponse,
  GetOutgoingDeclarationByIdResponse,
  OutgoingDeclarationResponse,
} from "../../../../../api/api";
import { useOrder } from "../../../../../shared/sorting/use-order";
import { isAxiosErrorType } from "../../../../../util/errors/predicates";
import { PageArgs, QueryState } from "../../../../types";

// page may not be below 1
const defaultPageArgs = {
  page: 1,
  pageSize: 100,
  isOrderDescending: false,
  orderByProperty: "company",
};

export const useViewPublishedDeclarations = (
  publishedDeclaration: OutgoingDeclarationResponse,
) => {
  const [isLoading, setIsLoading] = useState(false);

  const disableSubmit = true;

  const [pageArgs, setPageArgs] = useState<PageArgs>(defaultPageArgs);
  const { direction, orderBy, getLabelProps } =
    useOrder<keyof GetOutgoingDeclarationIncomingDeclarationResponse>(
      "company",
    );
  const [volume, setVolume] = useState<number | undefined>(undefined);

  const [queryStateDeclarations, setQueryStateDeclarations] = useState<
    QueryState<GetOutgoingDeclarationByIdResponse>
  >({ isLoading: true });

  const getOutgoingDeclarationById = useCallback(async () => {
    try {
      const response =
        await authorizedHttpClient.api.getOutgoingDeclarationById(
          publishedDeclaration.outgoingDeclarationId,
        );

      setQueryStateDeclarations({
        data: response.data,
        isLoading: false,
      });
    } catch (error) {
      const mappedError = isAxiosErrorType(error);
      setQueryStateDeclarations((state) => ({
        ...state,
        data: undefined,
        error: mappedError,
        hasError: true,
        isLoading: false,
      }));
    }
  }, [publishedDeclaration]);

  useEffect(() => {
    getOutgoingDeclarationById();
  }, []);

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
    if (
      !queryStateDeclarations.data?.outgoingDeclarationByIdResponse
        ?.getOutgoingDeclarationIncomingDeclarationResponse
    )
      return;
    const volumeCalc =
      queryStateDeclarations.data?.outgoingDeclarationByIdResponse?.getOutgoingDeclarationIncomingDeclarationResponse.map(
        (tx) => tx.quantity,
      );

    let volumeSum: number = 0;
    volumeCalc.forEach((value) => {
      if (value === undefined) {
        setVolume(undefined);
        return;
      } else {
        volumeSum = value + volumeSum;
      }
    });

    setVolume(volumeSum);
  }, [queryStateDeclarations]);

  return {
    queryStateDeclarations,
    isLoading,
    disableSubmit,
    setIsLoading,
    getLabelProps,
    volume,
  };
};
