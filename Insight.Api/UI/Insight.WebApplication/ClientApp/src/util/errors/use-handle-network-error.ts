import { ErrorDialog } from "../../components/dialogs/error-dialog";
import { useDialog } from "../../shared/dialog/use-dialog";
import { errorTranslations } from "../../translations/errors";
import { isAxiosErrorType, isApiError } from "./predicates";

const useHandleNetworkError = () => {
  const [openErrorDialog] = useDialog(ErrorDialog);

  const showErrorDialog = (error: unknown, traceId?: string) => {
    const axiosError = isAxiosErrorType(error);
    if (axiosError) {
      const apiError = isApiError(axiosError.response?.data);
      if (apiError) {
        openErrorDialog({
          title: apiError.title,
          description: apiError.detail,
          traceID: axiosError.response?.headers["x-trace-id"],
        });
      } else {
        openErrorDialog({
          title: errorTranslations.unknownNetworkErrorTitle,
          description: axiosError.message,
          traceID: axiosError.response?.headers["x-trace-id"],
        });
      }
    } else {
      openErrorDialog({
        title: errorTranslations.unknownNetworkErrorTitle,
        description: errorTranslations.unknownNetworkErrorDescription,
        traceID: traceId,
      });
    }
  };

  return { showErrorDialog };
};

export default useHandleNetworkError;
