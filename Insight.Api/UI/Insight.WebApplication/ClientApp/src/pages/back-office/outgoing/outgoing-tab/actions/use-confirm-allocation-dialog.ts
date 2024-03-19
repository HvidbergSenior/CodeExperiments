import { ConfirmDialog } from "../../../../../components/dialogs/confirm-dialog";
import { useFilter } from "../../../../../contexts/filter-context";
import { useDialog } from "../../../../../shared/dialog/use-dialog";
import { translations } from "../../../../../translations";
import { outgoingTabTranslations } from "../../../../../translations/pages/outgoing-tab-translations";
import { useOutgoingContext } from "../context/outgoing-tab-context";

export function useConfirmAllocationDialog() {
  const [openDialog, closeDialog] = useDialog(ConfirmDialog);
  const { handleAllocateConfirm } = useOutgoingContext();
  const { filterOutgoing } = useFilter();

  const handleSubmit = async () => {
    handleAllocateConfirm();
    closeDialog();
  };

  const handleClick = () => {
    openDialog({
      onConfirm: handleSubmit,
      onClose: closeDialog,
      title: outgoingTabTranslations.confirmAllocateTitle,
      description: outgoingTabTranslations.confirmAllocateDescription(
        filterOutgoing.dateFrom,
        filterOutgoing.dateTo,
      ),
      confirmTitle: outgoingTabTranslations.confirmAllocateSubmit,
      cancelTitle: translations.commonTranslations.cancel,
    });
  };

  return handleClick;
}
