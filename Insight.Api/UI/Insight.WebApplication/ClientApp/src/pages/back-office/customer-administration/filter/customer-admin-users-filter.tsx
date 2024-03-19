import Box from "@mui/material/Box";

import ClearIcon from "@mui/icons-material/Clear";
import { Button } from "@mui/material";
import { useCallback } from "react";
import { DebouncedTextfield } from "../../../../components/filter/debounced-textfield";
import { useFilter } from "../../../../contexts/filter-context";
import { filterTranslations } from "../../../../translations/filter";
import { customerAdminPageTranslations } from "../../../../translations/pages/customer-admin-page-translations";

export const CustomerAdminUsersFilterBar = () => {
  const {
    filterCustomerAdminUsers: filter,
    setFilterCustomerAdminUsers: setFilter,
    clearFilterCustomerAdminUsers: clearFilter,
  } = useFilter();

  const isFilterApplied = useCallback((): boolean => {
    return !(
      filter.email === "" &&
      filter.customerName === "" &&
      filter.customerNumber === ""
    );
  }, [filter.email, filter.customerName, filter.customerNumber]);
  return (
    <Box sx={{ display: "flex", gap: (theme) => theme.spacing(2) }}>
      <DebouncedTextfield
        startValue={filter.email}
        label={customerAdminPageTranslations.filter.email}
        updateFilter={(userName) =>
          setFilter((prevFilter) => ({ ...prevFilter, email: userName }))
        }
      />
      <DebouncedTextfield
        startValue={filter.customerName}
        label={customerAdminPageTranslations.filter.customerName}
        updateFilter={(customerName) =>
          setFilter((prevFilter) => ({ ...prevFilter, customerName }))
        }
      />
      <DebouncedTextfield
        startValue={filter.customerNumber}
        label={customerAdminPageTranslations.filter.customerNumber}
        updateFilter={(customerNumber) =>
          setFilter((prevFilter) => ({ ...prevFilter, customerNumber }))
        }
      />
      {isFilterApplied() && (
        <Button
          sx={{ backgroundColor: "transparent" }}
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
