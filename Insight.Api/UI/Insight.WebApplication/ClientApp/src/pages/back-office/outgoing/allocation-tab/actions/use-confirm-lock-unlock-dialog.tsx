import { useCallback } from "react";
import { ConfirmDialog } from "../../../../../components/dialogs/confirm-dialog";
import { useDialog } from "../../../../../shared/dialog/use-dialog";
import { useAllocationContext } from "../context/allocation-tab-context";
import { allocationTabTranslations } from "../../../../../translations/pages/allocation-tab-translations";

interface Props {
  toBeLocked: boolean;
}
export function useConfirmLockUnlockDialog({ toBeLocked }: Props) {
  const [openDialog, closeDialog] = useDialog(ConfirmDialog);
  const { lockAllocations, unlockAllocations } = useAllocationContext();

  const lockLabels = {
    title: allocationTabTranslations.lockLabels.title,
    description: allocationTabTranslations.lockLabels.description,
    confirmTitle: allocationTabTranslations.lockLabels.confirmTitle,
    cancel: allocationTabTranslations.lockLabels.cancel,
  };

  const unLockLabels = {
    title: allocationTabTranslations.unlockLabels.title,
    description: allocationTabTranslations.unlockLabels.description,
    confirmTitle: allocationTabTranslations.unlockLabels.confirmTitle,
    cancel: allocationTabTranslations.unlockLabels.cancel,
  };

  const handleSubmit = useCallback(async () => {
    if (toBeLocked) {
      lockAllocations();
    } else {
      unlockAllocations();
    }
    closeDialog();
  }, [toBeLocked]);

  const handleClick = () => {
    openDialog({
      onConfirm: handleSubmit,
      onClose: closeDialog,
      ...(toBeLocked ? lockLabels : unLockLabels),
    });
  };

  return handleClick;
}
