import {
  FC,
  ReactNode,
  createContext,
  useCallback,
  useContext,
  useEffect,
  useState,
} from "react";
import { authorizedHttpClient } from "../../../../api";
import {
  AllUserResponse,
  GetAllUsersResponse,
  UserStatus,
} from "../../../../api/api";
import useDeepCompare from "../../../../hooks/use-deep-compare/use-deep-compare";
import { isAxiosErrorType } from "../../../../util/errors/predicates";
import { QueryState } from "../../../types";
import { useOrder } from "../../../../shared/sorting/use-order";
import { useCustomerPortalContext } from "../../customer-portal-context";

interface Props {
  children: ReactNode;
}

interface SettingsContextProps {
  queryStateUsers: QueryState<GetAllUsersResponse>;
  setApiArgs: React.Dispatch<React.SetStateAction<ApiArgs>>;
  loadMore: () => void;
  getLabelProps: (key: keyof AllUserResponse) => {
    active: boolean;
    direction: "asc" | "desc";
    onClick: () => void;
  };
  handleRegisterUserSubmit: () => void;
}

const SettingsContext = createContext<SettingsContextProps>(
  {} as SettingsContextProps,
);

type ApiArgs = {
  page: number;
  pageSize: number;
  isOrderDescending: boolean;
  orderByProperty: keyof AllUserResponse;
  accountId: string;
  status: UserStatus;
  email: string;
};

const defaultApiArgs: ApiArgs = {
  page: 1,
  pageSize: 50,
  isOrderDescending: true,
  orderByProperty: "userName",
  accountId: "",
  status: "BlockedAndActive",
  email: "",
};

export const SettingsContextProvider: FC<Props> = ({ children }: Props) => {
  const [queryStateUsers, setQueryStateUsers] = useState<
    QueryState<GetAllUsersResponse>
  >({ isLoading: true });
  const { direction, orderBy, getLabelProps } =
    useOrder<keyof AllUserResponse>("firstName");
  const [apiArgs, setApiArgs] = useState<ApiArgs>(defaultApiArgs);
  const { settingsFilter } = useCustomerPortalContext();

  const getAllUsers = useCallback(async () => {
    try {
      const response = await authorizedHttpClient.api.getAllUsers({
        ...apiArgs,
        customerName: apiArgs.accountId,
        customerNumber: apiArgs.accountId,
      });

      if (apiArgs.page === 1) {
        setQueryStateUsers((state) => ({
          ...state,
          data: response.data,
          error: undefined,
          hasError: false,
          isLoading: false,
        }));
      } else {
        setQueryStateUsers((state) => ({
          ...state,
          data: {
            users: [...(state.data?.users ?? []), ...response.data.users],
            hasMoreUsers: response.data.hasMoreUsers,
            totalAmountOfUsers: response.data.totalAmountOfUsers,
          },
          error: undefined,
          hasError: false,
          isLoading: false,
        }));
      }
    } catch (error) {
      const mappedError = isAxiosErrorType(error);
      setQueryStateUsers((state) => ({
        ...state,
        data: undefined,
        error: mappedError,
        hasError: true,
        isLoading: false,
      }));
    }
  }, [apiArgs, settingsFilter]);

  useDeepCompare(() => {
    getAllUsers();
  }, [apiArgs]);

  useDeepCompare(() => {
    setApiArgs((prev) => ({
      ...prev,
      ...settingsFilter,
      page: 1,
    }));
  }, [settingsFilter]);

  useEffect(() => {
    if (
      apiArgs.isOrderDescending !== (direction === "desc") ||
      apiArgs.orderByProperty !== orderBy
    ) {
      setApiArgs((prev) => ({
        ...prev,
        page: 1,
        orderByProperty: orderBy,
        isOrderDescending: direction === "desc",
      }));
    }
  }, [orderBy, direction]);

  const loadMore = useCallback(() => {
    if (!queryStateUsers.data?.hasMoreUsers) {
      return;
    }
    setApiArgs((state) => ({
      ...state,
      page: state.page + 1,
    }));
  }, [queryStateUsers.data?.hasMoreUsers, setApiArgs]);

  const handleRegisterUserSubmit = () => {
    if (apiArgs.page === 1) {
      getAllUsers();
    } else {
      setApiArgs((prev) => ({ ...prev, page: 1 }));
    }
  };

  const props = {
    queryStateUsers,
    setApiArgs,
    loadMore,
    getLabelProps,
    handleRegisterUserSubmit,
  };

  return (
    <SettingsContext.Provider value={props}>
      {children}
    </SettingsContext.Provider>
  );
};

export const useSettingsContext = () => useContext(SettingsContext);
