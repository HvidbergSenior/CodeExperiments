import { CacheUnit } from "./types";

export const useCacheManager = () => {
  const getBaseUrl = () => {
    return window.location.protocol + "//" + window.location.host + "/";
  };

  const addDataToCache = <T>(cacheUnit: CacheUnit<T>) => {
    try {
      var baseUrl = getBaseUrl();

      const data = new Response(JSON.stringify(cacheUnit.cacheData));
      if ("caches" in window) {
        caches.open(cacheUnit.cacheTag).then((cache) => {
          cache.put(baseUrl, data);
        });
      }
    } catch (error) {
      console.log("Add to cache error: ", error);
    }
  };

  const deleteCacheWithTag = (cacheTag: string) => {
    try {
      if ("caches" in window) {
        caches.delete(cacheTag).then(function (res) {
          return res;
        });
      }
    } catch (error) {
      console.log("Delete cache error: ", error);
    }
  };

  const loadDataFromCache = async (cacheTag: string, url: string) => {
    try {
      if ("caches" in window) {
        const cacheStore = await caches.open(cacheTag);
        const response = await cacheStore.match(url);
        if (!response) return null;
        const data = await response.json();
        if (!data) {
          return null;
        } else {
          return data;
        }
      }
    } catch (error) {
      console.log("Load cache error: ", error);
    }
  };

  return {
    addDataToCache,
    loadDataFromCache,
    deleteCacheWithTag,
  };
};
