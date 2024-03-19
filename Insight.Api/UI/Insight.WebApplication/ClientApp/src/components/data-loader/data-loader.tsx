import { QueryState } from "../../pages/types";
import { DefaultLoadingSkeleton } from "../default-loading-skeleton/default-loading-skeleton";

interface Props<T> {
  queryState: QueryState<T>;
  children: (data: T) => JSX.Element;
}

export function DataLoader<T>({ queryState, children }: Props<T>) {
  if (queryState.isLoading) {
    return <DefaultLoadingSkeleton />;
  }

  if (queryState.hasError || !queryState.data) {
    throw queryState.error;
  }

  return children(queryState.data);
}
