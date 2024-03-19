import { InternalAxiosRequestConfig } from "axios";
import type { FC, ReactNode } from "react";
import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
} from "react";
import {
  api,
  authorizedHttpClient,
  unAuthorizedHttpClient,
} from "../../../../api";
import { LoginRequest, UserRole } from "../../../../api/api";
import {
  clearAllCookies,
  loadTokensFromCookies,
  saveAccessTokenForRefreshToCookie,
  saveAccessTokenWithExpireDateToCookie,
  saveRefreshTokenToCookie,
} from "../../../../util/cookies/cookie-manager";
import {
  isApiError,
  isAxiosErrorType,
} from "../../../../util/errors/predicates";
import { parseJwt } from "../../../../util/parse-jwt";
import { AuthContextProps } from "../../../types";
import { AvailableAccessRights } from "../../../../hooks/use-permissions/use-permissions";

interface Props {
  children: ReactNode;
}

export const AuthenticationContext = createContext<AuthContextProps>(
  {} as AuthContextProps,
);

export const AuthenticationConsumer = AuthenticationContext.Consumer;

export const AuthContextProvider: FC<Props> = ({ children }: Props) => {
  const [authenticated, setAuthenticated] = useState<boolean>(false);
  const [loading, setLoading] = useState(false);
  const [authLoading, setAuthLoading] = useState(true);
  const [error, setError] = useState<api.Error | undefined>(undefined);
  const [userRole, setUserRole] = useState<UserRole | undefined>(undefined);
  const [accessRights, setAccessRights] = useState<string[]>([]);
  const [username, setUsername] = useState<string>("-");

  const getUserRole = (accessToken: string): UserRole | undefined => {
    if (accessToken === undefined) {
      return undefined;
    }
    try {
      const parsedToken = parseJwt(accessToken);
      const userRoles: UserRole[] = ["Admin", "User"];
      if (!userRoles.includes(parsedToken.role as UserRole)) {
        return undefined;
      }
      return parsedToken.role as UserRole;
    } catch {
      return undefined;
    }
  };

  const getUsername = (accessToken: string): string | undefined => {
    if (accessToken === undefined) {
      return undefined;
    }
    try {
      const parsedToken = parseJwt(accessToken);
      return parsedToken.unique_name;
    } catch {
      return undefined;
    }
  };

  const setErrorForNoPermission = () => {
    setError({
      detail: "User does not have the required permisions",
      instance: "",
      status: 404,
      title: "",
      type: "",
    });
  };

  const getUserAccesRights = (accessToken: string): string[] | undefined => {
    try {
      const parsedToken = parseJwt(accessToken);

      const availableRights: AvailableAccessRights[] = [
        AvailableAccessRights.admin,
        AvailableAccessRights.fleetManagement,
        AvailableAccessRights.sustainabilityReport,
        AvailableAccessRights.fuelConsumption,
      ];

      let accessRights: string[] = [];

      if (typeof parsedToken.access === "string") {
        accessRights = [parsedToken.access];
      } else {
        accessRights = parsedToken.access;
      }

      if (!availableRights.includes(accessRights[0] as AvailableAccessRights)) {
        // If access rights do not match available rights, log out the user.
        console.log("No rights match avaiable rights");
        return undefined;
      }

      return accessRights;
    } catch (error) {
      console.log("Error parsing user rights");
      return undefined;
    }
  };

  const handleAuthentication = (accessToken: string) => {
    if (!accessToken) {
      console.log("access token is undefined");
      setAuthenticated(false);
      return;
    }

    const username = getUsername(accessToken);
    const userRole = getUserRole(accessToken);
    const accessRights = getUserAccesRights(accessToken);

    if (username) {
      setUsername(username);
    }

    if (userRole) {
      setUserRole(userRole);
    }

    if (accessRights) {
      setAccessRights(accessRights);
    }

    const isAuthenticated = !!userRole && !!accessRights;

    setError(undefined);

    if (!isAuthenticated) {
      clearAllCookies();
      setErrorForNoPermission();
    }
    setAuthenticated(isAuthenticated);
  };

  const isUser = useMemo((): boolean => {
    return userRole === "User";
  }, [userRole]);

  const isBackOfficeAdmin = useMemo((): boolean => {
    return userRole === "Admin";
  }, [userRole]);

  const login = useCallback(async (loginRequest: LoginRequest) => {
    setLoading(true);
    try {
      const {
        data: { accessToken, refreshToken },
      } = await unAuthorizedHttpClient.api.login(loginRequest);
      saveTokensToCookies(accessToken, refreshToken);
      handleAuthentication(accessToken);
    } catch (error) {
      const errorData = isAxiosErrorType(error);
      if (isApiError(errorData?.response?.data)) {
        setError(errorData?.response?.data as api.Error);
      }
      setAuthenticated(false);
    } finally {
      setLoading(false);
    }
  }, []);

  const getAccessToken = useCallback(async () => {
    const { accessToken, refreshToken } = loadTokensFromCookies();
    let token = accessToken;

    if (!token && refreshToken) {
      const { renewedAccessToken } = (await getAccessTokenSilently()) || {};
      token = renewedAccessToken || accessToken;
    }
    setAuthLoading(false);
    handleAuthentication(token);
    return token;
  }, []);

  const getAccessTokenSilently = async () => {
    try {
      const { accessTokenForRefresh, refreshToken } = loadTokensFromCookies();
      const {
        data: {
          accessToken: renewedAccessToken,
          refreshToken: renewedRefreshToken,
        },
      } = await unAuthorizedHttpClient.api.refresh({
        accessToken: accessTokenForRefresh,
        refreshToken,
      });

      saveTokensToCookies(renewedAccessToken, renewedRefreshToken);

      return { renewedAccessToken, renewedRefreshToken };
    } catch (error) {
      return null;
    }
  };

  const saveTokensToCookies = (accessToken: string, refreshToken: string) => {
    saveAccessTokenWithExpireDateToCookie(accessToken);
    saveAccessTokenForRefreshToCookie(accessToken);
    saveRefreshTokenToCookie(refreshToken);
  };

  useEffect(() => {
    getAccessToken();
    authorizedHttpClient.instance.interceptors.request.use(async (config) => {
      const token = await getAccessToken();
      return {
        ...config,
        headers: {
          ...config.headers,
          Authorization: `Bearer ${token}`,
        },
        paramsSerializer: { indexes: null },
      } as InternalAxiosRequestConfig;
    });

    authorizedHttpClient.instance.interceptors.response.use(
      async (response) => {
        if (response.status === 401) {
          setAuthenticated(false);
        }
        return response;
      },
    );
  }, []);

  const logout = () => {
    clearAllCookies();
    setAuthenticated(false);
  };

  const authProps = {
    authenticated,
    login,
    authLoading,
    loading,
    error,
    logout,
    isUser,
    isBackOfficeAdmin,
    accessRights,
    username,
  };

  return (
    <AuthenticationContext.Provider value={authProps}>
      {children}
    </AuthenticationContext.Provider>
  );
};

export const useAuthContext = () => useContext(AuthenticationContext);
