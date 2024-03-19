import { IncomingDeclarationDto } from "../../../../../api/api";
import { useDialog } from "../../../../../shared/dialog/use-dialog";
import { DeclarationDetailsDialog } from "../../../../../components/dialogs/declaration-details-dialog/declaration-details-dialog";
import { useDeclarationUploadContext } from "../context/declaration-upload-context";

export function useDeclarationDetailsDialog() {
  const [openDialog, closeDialog] = useDialog(DeclarationDetailsDialog);
  const { saveEditedDeclaration } = useDeclarationUploadContext();

  const handleSubmit = async (
    declarationToBeUpdated: IncomingDeclarationDto,
  ) => {
    saveEditedDeclaration(declarationToBeUpdated);
    closeDialog();
  };

  const handleClick = (
    declarationId: string,
    openedToEdit: boolean,
    disableEdit?: boolean,
  ) => {
    openDialog({
      onSubmit: handleSubmit,
      onClose: closeDialog,
      declarationId: declarationId,
      openedToEdit: openedToEdit,
      readOnlyView: disableEdit ?? false,
    });
  };

  return handleClick;
}
