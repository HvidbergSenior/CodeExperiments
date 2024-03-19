import { IncomingDeclarationDto } from "../../../../../api/api";
import { useDialog } from "../../../../../shared/dialog/use-dialog";
import { useReconciliationContext } from "../context/reconciliation-context";
import { DeclarationDetailsDialog } from "../../../../../components/dialogs/declaration-details-dialog/declaration-details-dialog";

export function useDeclarationDetailsDialog() {
  const [openDialog, closeDialog] = useDialog(DeclarationDetailsDialog);
  const { saveEditedDeclaration } = useReconciliationContext();

  const handleSubmit = async (
    declarationToBeUpdated: IncomingDeclarationDto,
  ) => {
    saveEditedDeclaration(declarationToBeUpdated);
    closeDialog();
  };

  const handleClick = (declarationId: string, openedToEdit: boolean) => {
    openDialog({
      onSubmit: handleSubmit,
      onClose: closeDialog,
      declarationId: declarationId,
      openedToEdit: openedToEdit,
    });
  };

  return handleClick;
}
