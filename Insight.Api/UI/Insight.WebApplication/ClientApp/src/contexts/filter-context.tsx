import { ReactNode, createContext, useContext, useState } from "react";
import { formatDate } from "../util/formatters/formatters";
import { getStartAndEndDateOfLastMonth } from "../util/date-utils";
interface Props {
  children: ReactNode | ReactNode[];
}

type FilterIncoming = {
  dateFrom: string | undefined;
  dateTo: string | undefined;
  product: string;
  company: string;
  supplier: string;
};

type FilterOutgoing = {
  dateFrom: string;
  dateTo: string;
  product: string;
  company: string;
  customer: string;
};

type FilterCustomerAdminUsers = {
  email: string;
  customerName: string;
  customerNumber: string;
};

type FilterContextProps = {
  filterIncoming: FilterIncoming;
  setFilterIncoming: React.Dispatch<React.SetStateAction<FilterIncoming>>;
  filterOutgoing: FilterOutgoing;
  setFilterOutgoing: React.Dispatch<React.SetStateAction<FilterOutgoing>>;
  filterCustomerAdminUsers: FilterCustomerAdminUsers;
  setFilterCustomerAdminUsers: React.Dispatch<
    React.SetStateAction<FilterCustomerAdminUsers>
  >;
  clearFilterIncoming: () => void;
  clearFilterOutgoing: () => void;
  clearFilterCustomerAdminUsers: () => void;
};

export const FilterContext = createContext<FilterContextProps>(
  {} as FilterContextProps,
);

export const FilterProvider = ({ children }: Props) => {
  const [filterIncoming, setFilterIncoming] = useState<FilterIncoming>({
    dateFrom: undefined,
    dateTo: undefined,
    product: "",
    company: "",
    supplier: "",
  });

  const [filterOutgoing, setFilterOutgoing] = useState<FilterOutgoing>({
    dateFrom: formatDate(getStartAndEndDateOfLastMonth().dateFrom),
    dateTo: formatDate(getStartAndEndDateOfLastMonth().dateTo),
    product: "",
    company: "",
    customer: "",
  });

  const [filterCustomerAdminUsers, setFilterCustomerAdminUsers] =
    useState<FilterCustomerAdminUsers>({
      email: "",
      customerName: "",
      customerNumber: "",
    });

  const clearFilterIncoming = () => {
    setFilterIncoming({
      dateFrom: undefined,
      dateTo: undefined,
      product: "",
      company: "",
      supplier: "",
    });
  };

  const clearFilterOutgoing = () => {
    setFilterOutgoing({
      dateFrom: formatDate(getStartAndEndDateOfLastMonth().dateFrom),
      dateTo: formatDate(getStartAndEndDateOfLastMonth().dateTo),
      product: "",
      company: "",
      customer: "",
    });
  };

  const clearFilterCustomerAdminUsers = () => {
    setFilterCustomerAdminUsers({
      email: "",
      customerName: "",
      customerNumber: "",
    });
  };

  return (
    <FilterContext.Provider
      value={{
        filterIncoming,
        setFilterIncoming,
        filterOutgoing,
        setFilterOutgoing,
        filterCustomerAdminUsers,
        setFilterCustomerAdminUsers,
        clearFilterIncoming,
        clearFilterOutgoing,
        clearFilterCustomerAdminUsers,
      }}
    >
      {children}
    </FilterContext.Provider>
  );
};

export const useFilter = () => {
  const context = useContext(FilterContext);
  if (context === null) {
    throw Error("You called useFilter hook outside its context");
  }
  return context;
};
