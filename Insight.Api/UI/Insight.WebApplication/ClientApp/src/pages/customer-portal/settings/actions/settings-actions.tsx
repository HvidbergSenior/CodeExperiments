import { Button } from "@mui/material";
import { customerPortalTranslations } from "../../../../translations/pages/customer-portal-translations";
import { useAddUserCustomerPortalDialog } from "../add-user-dialog/use-add-user-dialog";

export const SettingsActions = () => {
  const openAddUserDialog = useAddUserCustomerPortalDialog();

  return (
    <Button
      onClick={() => {
        openAddUserDialog();
      }}
      sx={{ textWrap: "nowrap" }}
      variant="contained"
    >
      {customerPortalTranslations.settings.addUser}
    </Button>
  );
};
