import Box from "@mui/material/Box";

import ClearIcon from "@mui/icons-material/Clear";
import { Button } from "@mui/material";
import { useCallback } from "react";
import { useFilter } from "../../contexts/filter-context";
import { filterTranslations } from "../../translations/filter";
import { formatDate } from "../../util/formatters/formatters";
import { DebouncedTextfield } from "./debounced-textfield";
import { PeriodButton } from "./period-button";

export const FilterIncomingBar = () => {
  const { filterIncoming, setFilterIncoming, clearFilterIncoming } =
    useFilter();

  const isFilterApplied = useCallback((): boolean => {
    return !(
      filterIncoming.company === "" &&
      filterIncoming.product === "" &&
      filterIncoming.supplier === "" &&
      filterIncoming.dateFrom === undefined &&
      filterIncoming.dateTo === undefined
    );
  }, [
    filterIncoming.product,
    filterIncoming.supplier,
    filterIncoming.dateFrom,
    filterIncoming.dateTo,
    filterIncoming.company,
  ]);
  return (
    <Box sx={{ display: "flex", gap: (theme) => theme.spacing(2) }}>
      <PeriodButton
        dateFrom={filterIncoming.dateFrom}
        dateTo={filterIncoming.dateTo}
        updateFilter={(fromDate, toDate) =>
          setFilterIncoming((prevFilter) => ({
            ...prevFilter,
            dateFrom: formatDate(fromDate),
            dateTo: formatDate(toDate),
          }))
        }
      />
      <DebouncedTextfield
        startValue={filterIncoming.product}
        label={filterTranslations.productPlaceholder}
        updateFilter={(product) =>
          setFilterIncoming((prevFilter) => ({ ...prevFilter, product }))
        }
      />
      <DebouncedTextfield
        startValue={filterIncoming.company}
        label={filterTranslations.companyPlaceholder}
        updateFilter={(company) =>
          setFilterIncoming((prevFilter) => ({ ...prevFilter, company }))
        }
      />
      <DebouncedTextfield
        startValue={filterIncoming.supplier}
        label={filterTranslations.supplierPlaceholder}
        updateFilter={(supplier) =>
          setFilterIncoming((prevFilter) => ({ ...prevFilter, supplier }))
        }
      />
      {isFilterApplied() && (
        <Button
          sx={{ backgroundColor: "transparent" }}
          variant="text"
          onClick={() => clearFilterIncoming()}
          startIcon={<ClearIcon fontSize="large" />}
        >
          {filterTranslations.clearFilterButtonTitle}
        </Button>
      )}
    </Box>
  );
};
