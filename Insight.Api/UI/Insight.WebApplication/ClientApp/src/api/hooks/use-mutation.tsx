import { AxiosResponse } from "axios";
import { useState } from "react";
import { api } from "..";
import { ErrorDialog } from "../../components/dialogs/error-dialog";
import { MutationState } from "../../pages/types";
import { useDialog } from "../../shared/dialog/use-dialog";
import { isError } from "../../util/errors/predicates";
import { RequestParams } from "../api";

interface Props<
  T,
  TSupportedErrorCodes extends api.Error["type"] = api.Error["type"],
> {
  mutation: (
    data: T,
    params: RequestParams,
  ) => Promise<AxiosResponse<void, any>>;
  handlers?: Partial<Record<TSupportedErrorCodes, (error: api.Error) => void>>;
}

export function useMutation<T>({
  mutation,
  handlers,
}: Props<T>): MutationState<T> {
  const [isComplete, setIsComplete] = useState(false);
  const [openErrorDialog] = useDialog(ErrorDialog);

  const mutateData = async (data: T, params: RequestParams) => {
    setIsComplete(false);
    try {
      await mutation(data, params);
      // Show success with snack bar or other ways
    } catch (error) {
      // determine type of error
      const isOfTypeError = isError(error);
      if (isOfTypeError) {
        executeHandlers(error as api.Error);
      } else {
        openErrorDialog({
          title: "Error",
          description: (error as any).data as string,
        });
      }
      // TODO - handle other errors
    } finally {
      setIsComplete(true);
    }
  };

  const executeHandlers = (error: api.Error) => {
    // TODO - the error argument needs to be of type list as there may be multiple errors in the inputtet data, hence we need to support an error list in the below
    const registeredHandler = handlers?.[error.type] ?? false;
    if (registeredHandler) {
      registeredHandler(error);
    } else {
      // Open an error dialog if no handlers match
      openErrorDialog({
        title: error.title,
        description: error.detail,
      });
    }
  };

  return { isComplete, mutateData };
}
