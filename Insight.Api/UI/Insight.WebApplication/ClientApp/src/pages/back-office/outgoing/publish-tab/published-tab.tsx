import { Paper } from "@mui/material";
import { FilterOutgoingBar } from "../../../../components/filter/filter-outgoing";
import { Topbar } from "../../../../components/top-bar/topbar";
import { PublishedTabContextProvider } from "./context/published-tab-context";
import { DataDisplay } from "./data-display/data-display";

export const PublishedTab = () => {
  return (
    <PublishedTabContextProvider>
      <Paper sx={{ padding: (theme) => theme.spacing(4) }}>
        <Topbar>
          <FilterOutgoingBar />
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
    </PublishedTabContextProvider>
  );
};
