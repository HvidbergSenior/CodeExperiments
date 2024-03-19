import { OutgoingFuelTransactionResponse } from "../../../../../api/api";
import { useDialog } from "../../../../../shared/dialog/use-dialog";
import { useStockContext } from "../context/stock-tab-context";
import { CreateStockDialog } from "./create-stock-dialog";

export function useCreateStockDialog() {
  const [openDialog, closeDialog] = useDialog(CreateStockDialog);
  const { handleCreateStockSubmit } = useStockContext();

  const handleSubmit = async () => {
    await handleCreateStockSubmit();
    closeDialog();
  };

  const handleClick = (fuelTransaction?: OutgoingFuelTransactionResponse) => {
    openDialog({
      fuelTransaction: fuelTransaction,
      onSubmit: handleSubmit,
      onClose: closeDialog,
    });
  };

  return handleClick;
}
