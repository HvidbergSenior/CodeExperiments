import {
  ReactNode,
  createContext,
  useCallback,
  useContext,
  useState,
} from "react";
import { authorizedHttpClient } from "../../api";
import { ProductNameEnumeration, UserStatus } from "../../api/api";
import { CustomerNode } from "../../components/customer-access-treeview/customer-node";
import useDeepCompare from "../../hooks/use-deep-compare/use-deep-compare";
import { getDateAYearAgoFromStartOfMonth } from "../../util/date-utils";
import useHandleNetworkError from "../../util/errors/use-handle-network-error";
import { formatDate } from "../../util/formatters/formatters";
import { mapPossibleCustomerPermissions } from "../../util/mapping";

interface Props {
  children: ReactNode | ReactNode[];
}

export type CustomerPortalFilter = {
  fromDate: string;
  toDate: string;
  fuels: ProductNameEnumeration[];
  accountsIds: string[];
};

export type SettingsFilter = {
  accountId: string;
  status: UserStatus;
  email: string;
};

type CustomerPortalContextProps = {
  filter: CustomerPortalFilter;
  setFilter: React.Dispatch<React.SetStateAction<CustomerPortalFilter>>;
  isFilterApplied: () => boolean;
  clearFilter: () => void;
  isSettingsFilterApplied: () => boolean;
  settingsFilter: SettingsFilter;
  setSettingsFilter: React.Dispatch<React.SetStateAction<SettingsFilter>>;
  clearSettingsFilter: () => void;
  availableAccounts: CustomerNode[] | undefined;
  setSearchPredicate: React.Dispatch<React.SetStateAction<string>>;
  searchPredicate: string;
  loadingAvailableCustomers: boolean;
};

export const CustomerPortalContext = createContext<CustomerPortalContextProps>(
  {} as CustomerPortalContextProps,
);

export const CustomerPortalProvider = ({ children }: Props) => {
  const [availableAccounts, setAvailableAccounts] = useState<
    CustomerNode[] | undefined
  >(undefined);

  const [searchPredicate, setSearchPredicate] = useState<string>("");
  const [loadingAvailableCustomers, setLoadingAvailableCustomers] =
    useState<boolean>(false);

  const { showErrorDialog } = useHandleNetworkError();

  const [filter, setFilter] = useState<CustomerPortalFilter>({
    fromDate: formatDate(getDateAYearAgoFromStartOfMonth()),
    toDate: formatDate(new Date()),
    fuels: [],
    accountsIds: [],
  });

  const clearFilter = useCallback(() => {
    setFilter({
      fromDate: formatDate(getDateAYearAgoFromStartOfMonth()),
      toDate: formatDate(new Date()),
      fuels: [],
      accountsIds: [],
    });
    setSearchPredicate("");
  }, [setFilter, setSearchPredicate]);

  const isFilterApplied = useCallback((): boolean => {
    return !(
      filter.fuels.length === 0 &&
      filter.accountsIds.length === 0 &&
      filter.fromDate === formatDate(getDateAYearAgoFromStartOfMonth()) &&
      filter.toDate === formatDate(new Date())
    );
  }, [filter]);

  const [settingsFilter, setSettingsFilter] = useState<SettingsFilter>({
    accountId: "",
    status: "BlockedAndActive",
    email: "",
  });

  const clearSettingsFilter = () => {
    setSettingsFilter({
      accountId: "",
      status: "BlockedAndActive",
      email: "",
    });
  };

  const isSettingsFilterApplied = useCallback((): boolean => {
    return !(
      settingsFilter.email === "" &&
      settingsFilter.accountId === "" &&
      settingsFilter.status === "BlockedAndActive"
    );
  }, [settingsFilter]);

  const getCustomerInformation = useCallback(async () => {
    try {
      setLoadingAvailableCustomers(true);
      const response =
        await authorizedHttpClient.api.getAvailableCustomersPermissions();
      const mappedCustomerPermissions = response.data.customerNodes.map(
        (customer) => mapPossibleCustomerPermissions(customer),
      );
      setAvailableAccounts(
        mappedCustomerPermissions.sort((a, b) => (a.name < b.name ? -1 : 1)),
      );
    } catch (error) {
      showErrorDialog(error);
    } finally {
      setLoadingAvailableCustomers(false);
    }
  }, []);

  useDeepCompare(() => {
    getCustomerInformation();
  }, []);

  const props = {
    filter,
    setFilter,
    isFilterApplied,
    clearFilter,
    isSettingsFilterApplied,
    clearSettingsFilter,
    setSettingsFilter,
    settingsFilter,
    availableAccounts,
    searchPredicate,
    setSearchPredicate,
    loadingAvailableCustomers,
  };

  return (
    <CustomerPortalContext.Provider value={props}>
      {children}
    </CustomerPortalContext.Provider>
  );
};

export const useCustomerPortalContext = () => {
  const context = useContext(CustomerPortalContext);

  if (context === null) {
    throw Error("You called useDialog hook outside its context");
  }

  return context;
};
