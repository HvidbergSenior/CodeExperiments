import { Box, Paper } from "@mui/material";
import { MassBalanceContextProvider } from "../context/mass-balance-context";
import { MassBalanceTabs } from "../tabs/mass-balance-tabs";
import { MassBalanceFilter } from "../filter/mass-balance-filter";
import { DataDisplay } from "../data-display.tsx/data-display";
import { MassBalanceActions } from "../actions/mass-balance-actions";

export const MassBalanceContent = () => {
  return (
    <MassBalanceContextProvider>
      <Paper sx={{ padding: (theme) => theme.spacing(4), mt: "2rem" }}>
        <Paper
          sx={{
            backgroundColor: (theme) => theme.palette.Gray1.main,
          }}
        >
          <Box display="flex" alignItems="center">
            <MassBalanceTabs />
            <Box display="flex" gap="1rem">
              <MassBalanceActions />
              <MassBalanceFilter />
            </Box>
          </Box>
          <DataDisplay />
        </Paper>
      </Paper>
    </MassBalanceContextProvider>
  );
};
