import { ConfirmDialog } from "../../../../../components/dialogs/confirm-dialog";
import { useDialog } from "../../../../../shared/dialog/use-dialog";
import { allocationTabTranslations } from "../../../../../translations/pages/allocation-tab-translations";
import { useAllocationContext } from "../context/allocation-tab-context";

export function useConfirmPublishAllocationsDialog() {
  const [openDialog, closeDialog] = useDialog(ConfirmDialog);
  const { publishAllocations } = useAllocationContext();

  const handleSubmit = async () => {
    publishAllocations();
    closeDialog();
  };

  const handleClick = () => {
    openDialog({
      onConfirm: handleSubmit,
      onClose: closeDialog,
      title: allocationTabTranslations.confirmPublishAllocations.title,
      description:
        allocationTabTranslations.confirmPublishAllocations.description,
      confirmTitle: allocationTabTranslations.confirmPublishAllocations.confirm,
      cancelTitle: allocationTabTranslations.confirmPublishAllocations.cancel,
    });
  };

  return handleClick;
}
