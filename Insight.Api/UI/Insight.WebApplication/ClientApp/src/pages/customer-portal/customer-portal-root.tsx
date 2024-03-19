import { Box, Grid } from "@mui/material";
import { ThemeProvider } from "@mui/material/styles";
import { Outlet } from "react-router-dom";
import { DialogProvider } from "../../shared/dialog/dialog-provider";
import { ErrorBoundary } from "../../shared/error-boundary";
import { customerPortalTheme } from "../../theme/customer-portal-theme";
import { CustomerPortalProvider } from "./customer-portal-context";
import { CustomerPortalHeader } from "./customer-portal-header";

export const CustomerPortalRoot = () => {
  return (
    <ThemeProvider theme={customerPortalTheme}>
      <DialogProvider>
        <CustomerPortalRootMain />
      </DialogProvider>
    </ThemeProvider>
  );
};

export const CustomerPortalRootMain = () => {
  return (
    <CustomerPortalProvider>
      <CustomerPortalRootContent />
    </CustomerPortalProvider>
  );
};

export const CustomerPortalRootContent = () => {
  return (
    <Box sx={{ display: "flex", height: "100vh" }}>
      <Box
        sx={{
          width: "100%",
          height: "100%",
          overflowY: "auto",
        }}
      >
        <CustomerPortalHeader />

        <Grid
          container
          direction="column"
          flexWrap="nowrap"
          height="100%"
          width="100%"
          sx={{ padding: 12, pt: 2 }}
        >
          <ErrorBoundary key={location.pathname}>
            <Box sx={{ margin: 2 }} />
            <Outlet />
          </ErrorBoundary>
        </Grid>
      </Box>
    </Box>
  );
};
