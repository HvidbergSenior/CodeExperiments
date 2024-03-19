import React, {
  ReactNode,
  createContext,
  useCallback,
  useContext,
} from "react";
import { useCacheManager } from "../util/use-cache-manager";

interface CacheContextProps {
  addToCache: (cacheTag: string, cacheData: any) => void;
  loadFromCache: (cacheTag: string) => Promise<any>;
  deleteFromCache: (cacheTag: string) => void;
}

const CacheContext = createContext<CacheContextProps | undefined>(undefined);

export const CacheProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const { addDataToCache, loadDataFromCache, deleteCacheWithTag } =
    useCacheManager();

  const getBaseUrl = () => {
    return window.location.protocol + "//" + window.location.host + "/";
  };

  const addToCache = useCallback((cacheTag: string, cacheData: any) => {
    addDataToCache({ cacheTag, cacheData });
  }, []);

  const deleteFromCache = useCallback((cacheTag: string) => {
    deleteCacheWithTag(cacheTag);
  }, []);

  const loadFromCache = useCallback(async (cacheTag: string) => {
    return await loadDataFromCache(cacheTag, getBaseUrl());
  }, []);

  const contextValue = {
    loadFromCache,
    addToCache,
    deleteFromCache,
  };

  return (
    <CacheContext.Provider value={contextValue}>
      {children}
    </CacheContext.Provider>
  );
};

export const useCache = () => {
  const context = useContext(CacheContext);
  if (!context) {
    throw new Error("useCache must be used within a CacheProvider");
  }
  return context;
};
