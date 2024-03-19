import { Paper } from "@mui/material";
import { FilterIncomingBar } from "../../../../components/filter/filter-incoming";
import { Topbar } from "../../../../components/top-bar/topbar";
import { DeclarationUploadActions } from "./actions/declaration-upload-actions";
import { DeclarationUploadContextProvider } from "./context/declaration-upload-context";
import { DataDisplay } from "./data-display/data-display";

export const Incoming = () => {
  return (
    <DeclarationUploadContextProvider>
      <Paper
        sx={{
          padding: (theme) => theme.spacing(4),
        }}
      >
        <Topbar>
          <FilterIncomingBar />
          <DeclarationUploadActions />
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
    </DeclarationUploadContextProvider>
  );
};
