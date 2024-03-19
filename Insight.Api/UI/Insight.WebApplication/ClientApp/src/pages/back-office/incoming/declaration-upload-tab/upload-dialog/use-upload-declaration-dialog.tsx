import { IncomingDeclarationSupplier } from "../../../../../api/api";
import { useDialog } from "../../../../../shared/dialog/use-dialog";
import { useDeclarationUploadContext } from "../context/declaration-upload-context";
import { UploadDeclarationDialog } from "./upload-declaration-dialog";

export function useUploadDeclarationDialog() {
  const [openDialog, closeDialog] = useDialog(UploadDeclarationDialog);
  const { handleSubmitOfUploadDeclarations } = useDeclarationUploadContext();

  const handleSubmit = async (
    batchId: string,
    oldestEntryDate?: string,
    newestEntryDate?: string,
  ) => {
    await handleSubmitOfUploadDeclarations(
      batchId,
      oldestEntryDate,
      newestEntryDate,
    );
    closeDialog();
  };

  const handleClick = (
    incomingDeclarationSupplier: IncomingDeclarationSupplier,
  ) => {
    openDialog({
      onSubmit: handleSubmit,
      incomingDeclarationSupplier: incomingDeclarationSupplier,
      onClose: closeDialog,
    });
  };

  return handleClick;
}
