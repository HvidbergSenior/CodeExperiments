import { Button, Stack } from "@mui/material";
import { customerAdminPageTranslations } from "../../../../translations/pages/customer-admin-page-translations";
import { useAddUserAdminDialog } from "../add-user-dialog/use-add-user-dialog";

export function CustomerAdminActions() {
  const openAddUserDialog = useAddUserAdminDialog();

  return (
    <Stack direction="row" spacing="10px">
      <Button variant="contained" onClick={() => openAddUserDialog()}>
        {customerAdminPageTranslations.addUser}
      </Button>
    </Stack>
  );
}
