import { AxiosError } from "axios";
import { api } from "../../api";

export function isError(
  error: api.Error | undefined | unknown,
): error is api.Error {
  if (error === undefined) {
    return false;
  }

  if (typeof error !== "object") {
    return false;
  }

  if (!(error as api.Error)) {
    return false;
  }
  return (error as api.Error).detail !== undefined;
}

export const isApiError = (error: unknown): false | api.Error | undefined => {
  return isError(error) && error;
};

export const isAxiosErrorType = (error: unknown) => {
  if (
    (error as AxiosError).name !== undefined &&
    (error as AxiosError).message !== undefined &&
    ((error as AxiosError).response !== undefined &&
      (error as AxiosError).response?.headers) !== undefined
  ) {
    return error as AxiosError;
  }
};
