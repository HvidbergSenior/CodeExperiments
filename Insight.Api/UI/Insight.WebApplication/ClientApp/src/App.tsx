import { CssBaseline } from "@mui/material";
import { ThemeProvider } from "@mui/material/styles";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { RouterProvider } from "react-router-dom";
import { ActivityIndicatorProvider } from "./contexts/activity-indicator-context";
import { AuthLoading } from "./pages/authentication/login/auth-loading";
import {
  AuthContextProvider,
  AuthenticationConsumer,
} from "./pages/authentication/login/context/auth-context";
import {
  authenticatedRoutesBackOffice,
  unauthenticatedRoutes,
} from "./shared/routing/router-admin";
import { authenticatedRoutesCustomerPortal } from "./shared/routing/router-customer-portal";
import { SnackBarProvider } from "./shared/snackbar/snackbar-context";
import { theme } from "./theme/theme";

export function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <LocalizationProvider dateAdapter={AdapterDateFns}>
        <SnackBarProvider>
          <ActivityIndicatorProvider>
            <AuthContextProvider>
              <AuthenticationConsumer>
                {({ authenticated, authLoading, isUser, isBackOfficeAdmin }) =>
                  authLoading ? (
                    <AuthLoading />
                  ) : (
                    <RouterProvider
                      router={
                        authenticated
                          ? isUser
                            ? authenticatedRoutesCustomerPortal
                            : isBackOfficeAdmin
                            ? authenticatedRoutesBackOffice
                            : unauthenticatedRoutes
                          : unauthenticatedRoutes
                      }
                    />
                  )
                }
              </AuthenticationConsumer>
            </AuthContextProvider>
          </ActivityIndicatorProvider>
        </SnackBarProvider>
      </LocalizationProvider>
    </ThemeProvider>
  );
}
