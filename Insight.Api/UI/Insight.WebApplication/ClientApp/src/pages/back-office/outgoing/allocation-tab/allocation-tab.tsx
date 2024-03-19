import { Paper } from "@mui/material";
import { FilterOutgoingBar } from "../../../../components/filter/filter-outgoing";
import { Topbar } from "../../../../components/top-bar/topbar";
import { AllocationTabActions } from "./actions/allocation-tab-actions";
import { AllocationTabContextProvider } from "./context/allocation-tab-context";
import { DataDisplay } from "./data-display/data-display";

export const AllocationTab = () => {
  return (
    <AllocationTabContextProvider>
      <Paper sx={{ padding: (theme) => theme.spacing(4) }}>
        <Topbar>
          <FilterOutgoingBar />
          <AllocationTabActions />
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
    </AllocationTabContextProvider>
  );
};
