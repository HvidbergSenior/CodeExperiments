import { PublishedDetailsDialog } from "../../../../../components/dialogs/published-details-dialog/published-details-dialog";
import { useDialog } from "../../../../../shared/dialog/use-dialog";

export function useDeclarationDetailsDialog() {
  const [openDialog, closeDialog] = useDialog(PublishedDetailsDialog);

  const handleClick = (declarationId: string) => {
    openDialog({
      onClose: closeDialog,
      declarationId: declarationId,
    });
  };

  return handleClick;
}
