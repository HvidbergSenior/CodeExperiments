import { useCallback, useEffect, useState } from "react";
import { authorizedHttpClient } from "../../../../../api";
import { GetAllocationByIdResponse } from "../../../../../api/api";
import { useOrder } from "../../../../../shared/sorting/use-order";
import { isAxiosErrorType } from "../../../../../util/errors/predicates";
import { IncomingResponseKeys, PageArgs, QueryState } from "../../../../types";
import useDeepCompare from "../../../../../hooks/use-deep-compare/use-deep-compare";

// page may not be below 1
const defaultPageArgs = {
  page: 1,
  pageSize: 100,
  isOrderDescending: false,
  orderByProperty: "company",
};

export const useViewAllocation = (allocationId: string) => {
  const [pageArgs, setPageArgs] = useState<PageArgs>(defaultPageArgs);

  const { direction, orderBy, getLabelProps } =
    useOrder<IncomingResponseKeys>("company");

  const [queryStateAllocationById, setQueryStateAllocationById] = useState<
    QueryState<GetAllocationByIdResponse>
  >({ data: {}, isLoading: true });

  const getAllocationById = useCallback(async () => {
    try {
      const response = await authorizedHttpClient.api.getAllocationById(
        allocationId,
        {
          isOrderDescending: pageArgs.isOrderDescending,
          orderByProperty: pageArgs.orderByProperty,
        },
      );
      setQueryStateAllocationById({ data: response.data, isLoading: false });
    } catch (error) {
      const mappedError = isAxiosErrorType(error);
      setQueryStateAllocationById({
        data: undefined,
        error: mappedError,
        hasError: true,
        isLoading: false,
      });
    }
  }, [pageArgs]);

  useEffect(() => {
    getAllocationById();
  }, []);

  useDeepCompare(() => {
    getAllocationById();
  }, [pageArgs]);

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

  return {
    queryStateAllocationById,
    getLabelProps,
  };
};
