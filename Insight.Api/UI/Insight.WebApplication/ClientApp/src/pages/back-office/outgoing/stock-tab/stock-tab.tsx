import { Paper } from "@mui/material";
import { FilterOutgoingBar } from "../../../../components/filter/filter-outgoing";
import { Topbar } from "../../../../components/top-bar/topbar";
import { StockTabActions } from "./actions/stock-tab-actions";
import { StockTabContextProvider } from "./context/stock-tab-context";
import { DataDisplay } from "./data-display/data-display";

export const StockTab = () => {
  return (
    <StockTabContextProvider>
      <Paper sx={{ padding: (theme) => theme.spacing(4) }}>
        <Topbar>
          <FilterOutgoingBar />
          <StockTabActions />
        </Topbar>
        <Paper
          sx={{
            backgroundColor: (theme) => theme.palette.Gray1.main,
            padding: (theme) => theme.spacing(4),
          }}
        >
          <DataDisplay />
        </Paper>
      </Paper>
    </StockTabContextProvider>
  );
};
