import LogoutIcon from "@mui/icons-material/Logout";
import SettingsIcon from "@mui/icons-material/Settings";
import { Box, Button, Divider } from "@mui/material";
import { Stack } from "@mui/system";
import { useLocation, useNavigate } from "react-router-dom";
import { theme } from "../../theme/theme";
import { customerPortalTranslations } from "../../translations/pages/customer-portal-translations";
import { useAuthContext } from "../authentication/login/context/auth-context";
import { CustomerPortalTabMenu } from "./customer-portal-tab-menu";
import AdminPanelSettingsIcon from "@mui/icons-material/AdminPanelSettings";
import {
  AvailableAccessRights,
  usePermissions,
} from "../../hooks/use-permissions/use-permissions";

export const CustomerPortalHeader = () => {
  const { logout, isBackOfficeAdmin } = useAuthContext();
  const navigate = useNavigate();
  const location = useLocation();
  const { hasAccessTo } = usePermissions();

  const onSettings = location.pathname.endsWith("settings");

  return (
    <Stack>
      <Stack
        direction="row"
        justifyContent={{ xs: "center", sm: "end" }}
        alignItems={"center"}
        mr={{ sm: "50px" }}
        mt={{ sm: "2px" }}
        mb={{ sm: "2px" }}
      >
        {isBackOfficeAdmin && (
          <Button
            variant="text"
            size="small"
            startIcon={<AdminPanelSettingsIcon />}
            onClick={() => navigate("/incoming")}
          >
            {"Back office"}
          </Button>
        )}
        {hasAccessTo(AvailableAccessRights.admin) && (
          <Button
            variant="text"
            size="small"
            startIcon={
              <SettingsIcon
                htmlColor={onSettings ? theme.palette.Green1.main : "black"}
              />
            }
            onClick={() => navigate("/customer-portal/settings")}
          >
            {customerPortalTranslations.menuItemSettings}
          </Button>
        )}

        <Button
          variant="text"
          onClick={() => logout()}
          size="small"
          startIcon={<LogoutIcon />}
        >
          {customerPortalTranslations.logout}
        </Button>
      </Stack>

      <Divider
        sx={{
          borderBottomWidth: 2,
        }}
      />
      <Box
        display="flex"
        flexDirection={{ xs: "column", sm: "row" }}
        justifyContent="space-between"
        ml={{ sm: 9 }}
        mr={{ sm: 12 }}
        height={{ xs: "auto", sm: 80 }}
      >
        <Box
          alignSelf="center"
          component="img"
          sx={{
            marginBottom: { xs: 2, sm: 0 },
          }}
          alt="Logo"
          src={`/assets/menu_logo.png`}
        />
        <Box
          sx={{
            alignSelf: "center",
          }}
        >
          <CustomerPortalTabMenu />
        </Box>
      </Box>
      <Box sx={{ margin: 2 }} />
      <Divider
        sx={{
          borderBottomWidth: 2,
          color: (theme) => theme.palette.Gray1.main,
        }}
      />
    </Stack>
  );
};
