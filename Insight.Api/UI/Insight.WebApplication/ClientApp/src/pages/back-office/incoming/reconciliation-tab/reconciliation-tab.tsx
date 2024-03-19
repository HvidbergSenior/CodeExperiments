import { Paper } from "@mui/material";
import { FilterIncomingBar } from "../../../../components/filter/filter-incoming";
import { Topbar } from "../../../../components/top-bar/topbar";
import { ReconciliationContextProvider } from "./context/reconciliation-context";
import { ReconciliationDataDisplay } from "./data-display/reconciliation-data-display";

export const Reconciliation = () => {
  return (
    <ReconciliationContextProvider>
      <Paper sx={{ padding: (theme) => theme.spacing(4) }}>
        <Topbar>
          <FilterIncomingBar />
        </Topbar>
        <Paper
          sx={{
            backgroundColor: (theme) => theme.palette.Gray1.main,
          }}
        >
          <ReconciliationDataDisplay />
        </Paper>
      </Paper>
    </ReconciliationContextProvider>
  );
};
