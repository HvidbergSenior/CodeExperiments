import { ConfirmDialog } from "../../../../../components/dialogs/confirm-dialog";
import { useDialog } from "../../../../../shared/dialog/use-dialog";
import { translations } from "../../../../../translations";
import { reconciliationTranslations } from "../../../../../translations/pages/reconcilliation-translations";

interface Props {
  reconcileDeclarations: () => void;
}
export function useConfirmReconciliationDialog({
  reconcileDeclarations,
}: Props) {
  const [openDialog, closeDialog] = useDialog(ConfirmDialog);

  const handleSubmit = async () => {
    reconcileDeclarations();
    closeDialog();
  };

  const handleClick = () => {
    openDialog({
      onConfirm: handleSubmit,
      onClose: closeDialog,
      title: reconciliationTranslations.reconcileConfirmTitle,
      description: reconciliationTranslations.reconcileConfirmDescription,
      confirmTitle: reconciliationTranslations.reconcileConfirmSubmit,
      cancelTitle: translations.commonTranslations.cancel,
    });
  };

  return handleClick;
}
