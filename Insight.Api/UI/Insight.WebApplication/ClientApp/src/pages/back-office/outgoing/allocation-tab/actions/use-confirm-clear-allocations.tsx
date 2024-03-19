import { ConfirmDialog } from "../../../../../components/dialogs/confirm-dialog";
import { useDialog } from "../../../../../shared/dialog/use-dialog";
import { commonTranslations } from "../../../../../translations/common";
import { allocationTabTranslations } from "../../../../../translations/pages/allocation-tab-translations";
import { useAllocationContext } from "../context/allocation-tab-context";

export function useConfirmClearAllocations() {
  const [openDialog, closeDialog] = useDialog(ConfirmDialog);
  const { clearAllocations } = useAllocationContext();

  const handleSubmit = async () => {
    clearAllocations();
    closeDialog();
  };

  const handleClick = () => {
    openDialog({
      onConfirm: handleSubmit,
      onClose: closeDialog,
      title: allocationTabTranslations.confirmClearAllocations.title,
      description:
        allocationTabTranslations.confirmClearAllocations.description,
      confirmTitle: allocationTabTranslations.confirmClearAllocations.confirm,
      cancelTitle: commonTranslations.cancel,
    });
  };

  return handleClick;
}
