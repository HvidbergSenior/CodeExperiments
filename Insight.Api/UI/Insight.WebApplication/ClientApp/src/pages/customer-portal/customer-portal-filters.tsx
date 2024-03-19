import {
  Box,
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  SelectChangeEvent,
} from "@mui/material";
import { ClearIcon } from "@mui/x-date-pickers";
import { useCallback, useEffect, useState } from "react";
import { ProductNameEnumeration } from "../../api/api";
import { PeriodButton } from "../../components/filter/period-button";
import { filterTranslations } from "../../translations/filter";
import { customerPortalTranslations } from "../../translations/pages/customer-portal-translations";
import { formatDate } from "../../util/formatters/formatters";
import {
  CustomerPortalFilter,
  useCustomerPortalContext,
} from "./customer-portal-context";
import { AccountsTreeViewButton } from "./customer-tree-view";

interface FuelTypeItem {
  name: string;
  value: ProductNameEnumeration;
}

export const fuelTypesAvailable: FuelTypeItem[] = [
  { name: "HVO100", value: "Hvo100" },
  { name: "HVO Diesel", value: "HvoDiesel" },
  { name: "AdBlue", value: "Adblue" },
  { name: "B100", value: "B100" },
  { name: "Diesel", value: "Diesel" },
  { name: "Petrol", value: "Petrol" },
  { name: "Heating Oil", value: "HeatingOil" },
];

export const CustomerPortalFilterBar = () => {
  const {
    filter,
    setFilter,
    isFilterApplied,
    clearFilter,
    searchPredicate,
    setSearchPredicate,
    availableAccounts,
    loadingAvailableCustomers,
  } = useCustomerPortalContext();
  const [filterHolder, setFilterHolder] =
    useState<CustomerPortalFilter>(filter);

  useEffect(() => {
    setFilterHolder(filter);
  }, [filter]);

  const handleSelectChange = useCallback(
    (event: SelectChangeEvent<string[]>, filter: string) => {
      setFilterHolder((prevFilter) => ({
        ...prevFilter,
        [filter]: event.target.value,
      }));
    },
    [setFilterHolder],
  );

  const handleSelectClose = useCallback(() => {
    setFilter(filterHolder);
  }, [filterHolder]);

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: { xs: "column", sm: "row" },
        alignContent: { sm: "flex-start" },
        width: "100%",
        gap: "16px",
      }}
    >
      <PeriodButton
        dateFrom={filter.fromDate}
        dateTo={filter.toDate}
        updateFilter={(fromDate, toDate) =>
          setFilter((prevFilter) => ({
            ...prevFilter,
            fromDate: formatDate(fromDate),
            toDate: formatDate(toDate),
          }))
        }
      />
      <FormControl sx={{ width: { sm: "240px" } }}>
        <InputLabel>{customerPortalTranslations.filter.fuel}</InputLabel>
        <Select
          label="fuels"
          multiple
          value={filterHolder.fuels}
          onChange={(e) => handleSelectChange(e, "fuels")}
          onClose={handleSelectClose}
        >
          {fuelTypesAvailable
            .sort((a, b) => (a < b ? -1 : 1))
            .map((fuel, index) => {
              return (
                <MenuItem key={index} value={fuel.value}>
                  {fuel.name}
                </MenuItem>
              );
            })}
        </Select>
      </FormControl>

      <AccountsTreeViewButton
        availableAccounts={availableAccounts}
        selectedCustomers={filter.accountsIds}
        loadingAvailableAccounts={loadingAvailableCustomers}
        searchPredicate={searchPredicate}
        updateSearchPredicate={(value) => setSearchPredicate(value)}
        updateFilter={(value: string[]) => {
          setFilter((prevFilter) => ({
            ...prevFilter,
            accountsIds: value,
          }));
        }}
      />

      {isFilterApplied() && (
        <Button
          sx={{
            ml: "16px",
            width: "140px",
            textWrap: "noWrap",
            backgroundColor: "transparent",
          }}
          variant="text"
          onClick={() => clearFilter()}
          startIcon={<ClearIcon fontSize="large" />}
        >
          {filterTranslations.clearFilterButtonTitle}
        </Button>
      )}
    </Box>
  );
};
