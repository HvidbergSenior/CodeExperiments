import type { FC, ReactNode } from "react";
import { createContext, useCallback, useContext, useState } from "react";
import { api, unAuthorizedHttpClient } from "../../../../api";
import { ForgotPasswordRequest } from "../../../../api/api";
import { useSnackBar } from "../../../../shared/snackbar/use-snackbar";
import { authTranslations } from "../../../../translations/pages/authentication";
import { isApiError, isAxiosErrorType } from "../../../../util/errors/predicates";
import { useNavigate } from "react-router-dom";

interface Props {
  children: ReactNode;
}

export interface ForgotPasswordContextProps {
  forgotPassword: (
    forgotPasswordRequest: ForgotPasswordRequest,
  ) => Promise<void>;
  loading: boolean;
  error: api.Error | undefined;
}

export const ForgotPasswordContext = createContext<ForgotPasswordContextProps>(
  {} as ForgotPasswordContextProps,
);

export const ForgotPasswordContextProvider: FC<Props> = ({
  children,
}: Props) => {
  const navigate = useNavigate();
  const { showSnackBar } = useSnackBar();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<api.Error | undefined>(undefined);

  const forgotPassword = useCallback(
    async (forgotPasswordRequest: ForgotPasswordRequest) => {
      setLoading(true);
      try {
        await unAuthorizedHttpClient.api.forgotPassword(forgotPasswordRequest);
        setError(undefined);
        showSnackBar(authTranslations.forgotPasswordPage.success, "success");
        navigate("/login");
      } catch (error) {
        const errorData = isAxiosErrorType(error);
        if (isApiError(errorData?.response?.data)) {
          setError(errorData?.response?.data as api.Error);
        }
      } finally {
        setLoading(false);
      }
    },
    [],
  );

  const props = {
    forgotPassword,
    loading,
    error,
  };

  return (
    <ForgotPasswordContext.Provider value={props}>
      {children}
    </ForgotPasswordContext.Provider>
  );
};

export const useForgotPasswordContext = () => useContext(ForgotPasswordContext);
