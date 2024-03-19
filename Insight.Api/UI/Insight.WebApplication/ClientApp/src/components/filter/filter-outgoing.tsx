import Box from "@mui/material/Box";

import { useFilter } from "../../contexts/filter-context";
import { filterTranslations } from "../../translations/filter";
import { DebouncedTextfield } from "./debounced-textfield";
import { PeriodButton } from "./period-button";
import { Button } from "@mui/material";
import ClearIcon from "@mui/icons-material/Clear";
import { useCallback } from "react";
import { formatDate } from "../../util/formatters/formatters";
import { getStartAndEndDateOfLastMonth } from "../../util/date-utils";

export const FilterOutgoingBar = () => {
  const { filterOutgoing, setFilterOutgoing, clearFilterOutgoing } =
    useFilter();

  const isFilterApplied = useCallback((): boolean => {
    return !(
      filterOutgoing.company === "" &&
      filterOutgoing.product === "" &&
      filterOutgoing.customer === "" &&
      filterOutgoing.dateFrom ===
        formatDate(getStartAndEndDateOfLastMonth().dateFrom) &&
      filterOutgoing.dateTo ===
        formatDate(getStartAndEndDateOfLastMonth().dateTo)
    );
  }, [
    filterOutgoing.product,
    filterOutgoing.customer,
    filterOutgoing.dateFrom,
    filterOutgoing.dateTo,
    filterOutgoing.company,
  ]);
  return (
    <Box sx={{ display: "flex", gap: (theme) => theme.spacing(2) }}>
      <PeriodButton
        dateFrom={filterOutgoing.dateFrom}
        dateTo={filterOutgoing.dateTo}
        updateFilter={(fromDate, toDate) =>
          setFilterOutgoing((prevFilter) => ({
            ...prevFilter,
            dateFrom: formatDate(fromDate),
            dateTo: formatDate(toDate),
          }))
        }
      />
      <DebouncedTextfield
        startValue={filterOutgoing.product}
        label={filterTranslations.productPlaceholder}
        updateFilter={(product) =>
          setFilterOutgoing((prevFilter) => ({ ...prevFilter, product }))
        }
      />
      <DebouncedTextfield
        startValue={filterOutgoing.company}
        label={filterTranslations.companyPlaceholder}
        updateFilter={(company) =>
          setFilterOutgoing((prevFilter) => ({ ...prevFilter, company }))
        }
      />
      <DebouncedTextfield
        startValue={filterOutgoing.customer}
        label={filterTranslations.customerPlaceholder}
        updateFilter={(customer) =>
          setFilterOutgoing((prevFilter) => ({
            ...prevFilter,
            customer,
          }))
        }
      />
      {isFilterApplied() && (
        <Button
          sx={{ backgroundColor: "transparent" }}
          variant="text"
          onClick={() => clearFilterOutgoing()}
          startIcon={<ClearIcon fontSize="large" />}
        >
          {filterTranslations.clearFilterButtonTitle}
        </Button>
      )}
    </Box>
  );
};
