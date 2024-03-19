import { OutgoingDeclarationResponse } from "../../../../../api/api";
import { useDialog } from "../../../../../shared/dialog/use-dialog";
import { ViewPublishedDeclarationDialog } from "./view-published-declaration-dialog";

export function useViewPublishedDeclarationDialog() {
  const [openDialog, closeDialog] = useDialog(ViewPublishedDeclarationDialog);

  const handleClick = (publishedDeclaration: OutgoingDeclarationResponse) => {
    openDialog({
      publishedDeclaration: publishedDeclaration,
      onClose: closeDialog,
    });
  };

  return handleClick;
}
