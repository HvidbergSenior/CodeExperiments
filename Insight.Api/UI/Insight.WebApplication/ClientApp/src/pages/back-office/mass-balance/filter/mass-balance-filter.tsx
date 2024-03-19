import Box from "@mui/material/Box";

import { useMassBalanceContext } from "../context/mass-balance-context";
import { PeriodButton } from "../../../../components/filter/period-button";
import { formatDate } from "../../../../util/formatters/formatters";

export const MassBalanceFilter = () => {
  const { filter, setFilter } = useMassBalanceContext();
  return (
    <Box sx={{ display: "flex", gap: (theme) => theme.spacing(2) }}>
      <PeriodButton
        dateFrom={filter.dateFrom}
        dateTo={filter.dateTo}
        updateFilter={(dateFrom, dateTo) =>
          setFilter((prevFilter) => ({
            ...prevFilter,
            dateFrom: formatDate(dateFrom),
            dateTo: formatDate(dateTo),
          }))
        }
        height="37px"
      />
    </Box>
  );
};
