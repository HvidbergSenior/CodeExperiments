import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Paper from "@mui/material/Paper";

import { Typography } from "@mui/material";
import { Topbar } from "../../../components/top-bar/topbar";
import { theme } from "../../../theme/theme";
import { customerAdminPageTranslations } from "../../../translations/pages/customer-admin-page-translations";
import { CustomerAdminContextProvider } from "./context/customer-admin-context";
import { CustomerAdminActions } from "./actions/customer-admin-actions";
import { DataDisplay } from "./data-display/data-display";
import { CustomerAdminUsersFilterBar } from "./filter/customer-admin-users-filter";

export const CustomerAdminPage = () => {
  return (
    <Box width="100%" height="100%" justifyContent="center">
      <Box mb={(theme) => theme.spacing(8)}>
        <Typography variant="h1">
          {customerAdminPageTranslations.customerAdmin}
        </Typography>
      </Box>
      <Paper sx={{ display: "flex", flexDirection: "column" }}>
        <Grid
          container
          justifyContent="center"
          alignItems="center"
          sx={{ padding: (theme) => theme.spacing(2), mt: theme.spacing(8) }}
          display="flex"
        ></Grid>
        <CustomerAdminContextProvider>
          <Paper sx={{ padding: (theme) => theme.spacing(4) }}>
            <Topbar>
              <CustomerAdminUsersFilterBar />
              <CustomerAdminActions />
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
        </CustomerAdminContextProvider>
      </Paper>
    </Box>
  );
};
