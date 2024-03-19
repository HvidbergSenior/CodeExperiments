import { Box, Paper } from "@mui/material";
import { SettingsActions } from "./actions/settings-actions";
import { SettingsContextProvider } from "./context/settings-context";
import { DataDisplay } from "./data-display/data-display";
import { SettingsFilter } from "./actions/settings-filter";

export const SettingsPage = () => {
  return (
    <SettingsContextProvider>
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          alignSelf: "center",
          width: { xs: "100%", sm: "60vw" },
          gap: { xs: "20px", sm: "80px" },
        }}
      >
        <Box
          sx={{
            display: "flex",
            flexDirection: { xs: "column", sm: "row" },
            gap: "10px",
            alignItems: "center",
            justifyContent: "space-between",
          }}
        >
          <SettingsFilter />
          <SettingsActions />
        </Box>
        <Paper>
          <DataDisplay />
        </Paper>
      </Box>
    </SettingsContextProvider>
  );
};
