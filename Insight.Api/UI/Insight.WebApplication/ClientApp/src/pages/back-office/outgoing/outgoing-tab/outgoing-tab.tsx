import { Paper } from "@mui/material";
import { FilterOutgoingBar } from "../../../../components/filter/filter-outgoing";
import { Topbar } from "../../../../components/top-bar/topbar";
import { minWidthTablePaper } from "../../../../shared/constants/constants";
import { OutgoingTabActions } from "./actions/outgoing-tab-actions";
import { OutgoingTabContextProvider } from "./context/outgoing-tab-context";
import { DataDisplay } from "./data-display/data-display";

export const OutgoingTab = () => {
  return (
    <OutgoingTabContextProvider>
      <Paper sx={{ padding: (theme) => theme.spacing(4) }}>
        <Topbar>
          <FilterOutgoingBar />
          <OutgoingTabActions />
        </Topbar>
        <Paper
          sx={{
            backgroundColor: (theme) => theme.palette.Gray1.main,
            padding: (theme) => theme.spacing(4),
            minWidth: minWidthTablePaper,
          }}
        >
          <DataDisplay />
        </Paper>
      </Paper>
    </OutgoingTabContextProvider>
  );
};
