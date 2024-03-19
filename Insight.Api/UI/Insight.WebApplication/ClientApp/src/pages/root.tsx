import { Box, Grid } from "@mui/material";
import { Outlet } from "react-router-dom";
import { NavigationMenu } from "../components/navigation";
import { FilterProvider } from "../contexts/filter-context";
import { DialogProvider } from "../shared/dialog/dialog-provider";
import { ErrorBoundary } from "../shared/error-boundary";

export const Root = () => {
  return (
    <DialogProvider>
      <Box sx={{ display: "flex", height: "100vh" }}>
        <NavigationMenu />

        <Box
          sx={{
            width: "100%",
            height: "100%",
            overflowY: "auto",
          }}
        >
          <Grid
            container
            direction="column"
            flexWrap="nowrap"
            height="100%"
            sx={{ padding: 8, pt: 2 }}
          >
            <ErrorBoundary key={location.pathname}>
              <FilterProvider>
                <Box sx={{ margin: 2 }} />
                <Outlet />
              </FilterProvider>
            </ErrorBoundary>
          </Grid>
        </Box>
      </Box>
    </DialogProvider>
  );
};
