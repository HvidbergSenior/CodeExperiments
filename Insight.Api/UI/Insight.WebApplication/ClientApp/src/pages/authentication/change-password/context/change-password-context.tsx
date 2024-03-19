import type { FC, ReactNode } from "react";
import { createContext, useCallback, useContext, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { api, unAuthorizedHttpClient } from "../../../../api";
import { useSnackBar } from "../../../../shared/snackbar/use-snackbar";
import { authTranslations } from "../../../../translations/pages/authentication";
import {
  isApiError,
  isAxiosErrorType,
} from "../../../../util/errors/predicates";

interface Props {
  children: ReactNode;
}

interface ChangePasswordRequest {
  password: string;
}

export interface ChangePasswordContextProps {
  changePassword: (
    changePasswordRequest: ChangePasswordRequest,
  ) => Promise<void>;
  loading: boolean;
  error: api.Error | undefined;
}

export const ChangePasswordContext = createContext<ChangePasswordContextProps>(
  {} as ChangePasswordContextProps,
);

export const ChangePasswordContextProvider: FC<Props> = ({
  children,
}: Props) => {
  const navigate = useNavigate();
  const { showSnackBar } = useSnackBar();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<api.Error | undefined>(undefined);
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);

  const getCustomErrorMessage = (details: string) => {
    return {
      detail: details,
      instance: "",
      status: 1,
      title: "",
      type: "",
    };
  };

  const changePassword = useCallback(
    async (changePasswordRequest: ChangePasswordRequest) => {
      const username = queryParams.get("username");
      const token = queryParams.get("token");
      console.log("Change password: " + username + ", " + token);
      if (!username || !token) {
        setError(
          getCustomErrorMessage(
            authTranslations.changePasswordPage.noPermissionToChangePassword,
          ),
        );
        return;
      }
      setLoading(true);
      try {
        await unAuthorizedHttpClient.api.resetPassword({
          userName: username,
          token: token,
          newPassword: changePasswordRequest.password,
        });
        setError(undefined);
        showSnackBar(authTranslations.changePasswordPage.success, "success");
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
    changePassword,
    loading,
    error,
  };

  return (
    <ChangePasswordContext.Provider value={props}>
      {children}
    </ChangePasswordContext.Provider>
  );
};

export const useChangePasswordContext = () => useContext(ChangePasswordContext);
