import { AllUserAdminResponse } from "../../../../api/api";
import { useDialog } from "../../../../shared/dialog/use-dialog";
import { useCustomerAdminContext } from "../context/customer-admin-context";
import { AddUserDialog } from "./add-user-dialog";

export function useAddUserAdminDialog() {
  const [openDialog, closeDialog] = useDialog(AddUserDialog);
  const { handleRegisterUserSubmit } = useCustomerAdminContext();

  const handleSubmit = async () => {
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
