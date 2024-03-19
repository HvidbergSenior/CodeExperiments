import { AllUserAdminResponse } from "../../../../api/api";
import { useDialog } from "../../../../shared/dialog/use-dialog";
import { useSettingsContext } from "../context/settings-context";
import { AddUserCustomerPortalDialog } from "./add-user-dialog";

export function useAddUserCustomerPortalDialog() {
  const { handleRegisterUserSubmit } = useSettingsContext();
  const [openDialog, closeDialog] = useDialog(AddUserCustomerPortalDialog);

  const handleSubmit = () => {
    handleRegisterUserSubmit();
    closeDialog();
  };

  const handleClick = (userData?: AllUserAdminResponse) => {
    openDialog({
      userData: userData,
      onSubmit: handleSubmit,
      onClose: closeDialog,
    });
  };

  return handleClick;
}
