import ChevronLeftIcon from "@mui/icons-material/ChevronLeft";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import { Box, Button, Divider, IconButton, List } from "@mui/material";
import { Stack } from "@mui/system";
import { useState } from "react";
import { useAuthContext } from "../../pages/authentication/login/context/auth-context";
import { translations } from "../../translations";
import { authTranslations } from "../../translations/pages/authentication";
import { ListItemLink } from "../list-item-link";

export function NavigationMenu() {
  const { logout } = useAuthContext();
  const [isMenuOpen, setIsMenuOpen] = useState(true);

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  if (!isMenuOpen) {
    return (
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          minWidth: (theme) => theme.spacing(6),
          transition: "min-width 0.3s",
          paddingY: (theme) => theme.spacing(4),
          backgroundColor: (theme) => theme.palette.background.paper,
          boxShadow: "5px 0 6px 0 #EEE",
          justifyContent: "space-between",
        }}
      >
        <Box
          component="img"
          sx={{
            width: "31px",
            mt: "8px",
          }}
          alt="Logo"
          src={`/assets/thumbnail.png`}
        />
        <IconButton onClick={toggleMenu} sx={{ mt: 1, p: 1 }}>
          <ChevronRightIcon />
        </IconButton>
      </Box>
    );
  }

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        minWidth: (theme) => theme.spacing(64),
        transition: "min-width 0.01s",
        paddingY: (theme) => theme.spacing(4),
        backgroundColor: (theme) => theme.palette.background.paper,
        boxShadow: "5px 0 6px 0 #EEE",
        justifyContent: "space-between",
      }}
    >
      <Box
        component="img"
        sx={{
          marginLeft: (theme) => theme.spacing(6),
          marginBottom: (theme) => theme.spacing(6),
          width: "200px",
        }}
        alt="Logo"
        src={`/assets/logo.png`}
      />

      <List sx={{ flexGrow: 1 }}>
        <>
          <Divider />
          <ListItemLink
            to="/incoming"
            primary={translations.incomingTranslations.incoming}
          />
          <Divider />
          <ListItemLink
            to="/outgoing"
            primary={translations.outgoingPageTranslations.outgoing}
          />
          <Divider />
          <ListItemLink
            to="/mass-balance"
            primary={translations.massBalancePageTranslations.massBalanceTitle}
          />
          <Divider />
        </>
      </List>
      <Stack>
        <List sx={{ flexGrow: 1 }}>
          <Divider />
          <ListItemLink
            to="/customer-admin"
            primary={translations.customerAdminPageTranslations.customerAdmin}
          />
          <Divider />
        </List>
        <>
          <List sx={{ flexGrow: 1 }}>
            <ListItemLink
              to="/customer-portal"
              primary={
                translations.customerAdminPageTranslations.customerPortal
              }
            />
            <Divider />
          </List>
          <Box height="10px" />
        </>

        <Button sx={{ boxShadow: "none" }} onClick={() => logout()}>
          {authTranslations.logout}
        </Button>

        <IconButton onClick={toggleMenu} sx={{ mt: 1, p: 1 }}>
          <ChevronLeftIcon />
        </IconButton>
      </Stack>
    </Box>
  );
}
