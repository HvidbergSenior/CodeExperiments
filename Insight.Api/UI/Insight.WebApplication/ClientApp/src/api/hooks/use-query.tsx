import { AxiosResponse } from "axios";
import { useCallback, useEffect, useState } from "react";
import { useCache } from "../../contexts/cache-context";
import { QueryState } from "../../pages/types";
import { RequestParams } from "../api";

interface Props<T> {
  query: (params?: RequestParams) => Promise<AxiosResponse<T, any>>;
  cacheTag: string;
}

export const useQuery = <T extends object>({
  query,
  cacheTag,
}: Props<T>): QueryState<T> => {
  const [data, setData] = useState<T | undefined>(undefined);
  const [isLoading, setIsLoading] = useState(true);
  const [hasError, setHasError] = useState(false);
  const { addToCache, loadFromCache } = useCache();

  const getData = useCallback(async () => {
    setIsLoading(true);
    setHasError(false);
    try {
      const cachedData = (await loadFromCache(cacheTag)) as T;
      if (cachedData) {
        setData(cachedData);
      } else {
        const { data } = await query();
        addToCache(cacheTag, data);
        setData(data);
      }
    } catch (error) {
      setHasError(true);
    } finally {
      setIsLoading(false);
    }
  }, [cacheTag, setData]);

  useEffect(() => {
    getData();
  }, []);

  return { data, isLoading, hasError };
};
