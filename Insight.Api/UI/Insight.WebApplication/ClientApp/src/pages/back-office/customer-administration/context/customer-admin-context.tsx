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
  AllUserAdminResponse,
  GetAllUsersAdminResponse,
} from "../../../../api/api";
import { useFilter } from "../../../../contexts/filter-context";
import useDeepCompare from "../../../../hooks/use-deep-compare/use-deep-compare";
import { useOrder } from "../../../../shared/sorting/use-order";
import { isAxiosErrorType } from "../../../../util/errors/predicates";
import { CustomerAdminContextProps, QueryState } from "../../../types";

// TODO: BKN - temp model to be replaced by one from api
export interface UserDto {
  id: string;
  userName: string;
  customerName: string;
  customerNumber: number;
  status: string;
  email: string;
  userType: string;
  accessFuelConsumption: boolean;
  accessSustainabilityReport: boolean;
  accessFleetManagement: boolean;
}

interface Props {
  children: ReactNode;
}

const CustomerAdminContext = createContext<CustomerAdminContextProps>(
  {} as CustomerAdminContextProps,
);

type ApiArgs = {
  page: number;
  pageSize: number;
  isOrderDescending: boolean;
  orderByProperty: keyof AllUserAdminResponse;
  accountId: string;
  status: string;
  email: string;
};

const defaultApiArgs: ApiArgs = {
  page: 1,
  pageSize: 50,
  isOrderDescending: false,
  orderByProperty: "userName",
  accountId: "",
  status: "BlockedAndActive",
  email: "",
};

export const CustomerAdminContextProvider: FC<Props> = ({
  children,
}: Props) => {
  const { filterCustomerAdminUsers } = useFilter();
  const [apiArgs, setApiArgs] = useState<ApiArgs>(defaultApiArgs);
  const { direction, orderBy, getLabelProps } =
    useOrder<keyof AllUserAdminResponse>("userName");
  const [queryStateUsers, setQueryStateUsers] = useState<
    QueryState<GetAllUsersAdminResponse>
  >({ isLoading: true });

  const getAllUsers = useCallback(async () => {
    try {
      const response = await authorizedHttpClient.api.getAllUsersAdmin({
        ...apiArgs,
      });
      if (apiArgs.page === 1) {
        setQueryStateUsers({
          data: response.data,
          isLoading: false,
        });
      } else {
        setQueryStateUsers((state) => ({
          ...state,
          data: {
            hasMoreUsers: response.data.hasMoreUsers,
            users: [...(state.data?.users ?? []), ...response.data.users],
            totalAmountOfUsers: response.data.totalAmountOfUsers,
          },
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
  }, [apiArgs, filterCustomerAdminUsers]);

  const loadMore = useCallback(() => {
    if (!queryStateUsers.data?.hasMoreUsers) {
      return;
    }
    setApiArgs((state) => ({
      ...state,
      page: state.page + 1,
    }));
  }, [queryStateUsers.data?.hasMoreUsers]);

  useDeepCompare(() => {
    getAllUsers();
  }, [apiArgs]);

  useDeepCompare(() => {
    setApiArgs((prev) => ({ ...prev, ...filterCustomerAdminUsers, page: 1 }));
  }, [filterCustomerAdminUsers]);

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

  const handleRegisterUserSubmit = () => {
    if (apiArgs.page === 1) {
      getAllUsers();
    } else {
      setApiArgs((prev) => ({ ...prev, page: 1 }));
    }
  };

  const props = {
    queryStateUsers,
    loadMore,
    getLabelProps,
    handleRegisterUserSubmit,
  };

  return (
    <CustomerAdminContext.Provider value={props}>
      {children}
    </CustomerAdminContext.Provider>
  );
};

export const useCustomerAdminContext = () => useContext(CustomerAdminContext);
