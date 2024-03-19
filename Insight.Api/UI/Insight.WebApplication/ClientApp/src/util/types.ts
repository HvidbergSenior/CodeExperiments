export type OrderDirection = "asc" | "desc";

export type CacheUnit<T> = {
  cacheTag: string;
  cacheData: T;
};
