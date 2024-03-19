import { Box, Typography } from "@mui/material";
import { GridTable } from "../../../../components/table/grid-table/grid-table";
import { useMassBalanceContext } from "../context/mass-balance-context";
import { massBalanceColumns } from "./mass-balance-columns";

export const MassBalanceTable = () => {
  const { dataToDisplay } = useMassBalanceContext();

  return (
    <Box mt="1rem" height="calc(100vh - 250px)" overflow="auto">
      {dataToDisplay.companies.map((location) => (
        <Box p={4} display="flex" gap={4} flexDirection="column" mb={8}>
          <Typography variant="h3">{location.locationName}</Typography>
          <GridTable columns={massBalanceColumns} data={location.data} />
        </Box>
      ))}
      {dataToDisplay.companies.length === 0 && (
        <Box p={4} display="flex" gap={4} flexDirection="column" mb={8}>
          <GridTable
            columns={massBalanceColumns}
            data={[]}
            emptyText="No mass balance data available"
          />
        </Box>
      )}
    </Box>
  );
};
