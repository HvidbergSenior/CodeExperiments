import { OutgoingFuelTransactionResponse } from "../../../../../api/api";
import { useFilter } from "../../../../../contexts/filter-context";
import { useDialog } from "../../../../../shared/dialog/use-dialog";
import { useOutgoingContext } from "../context/outgoing-tab-context";
import { ManualAllocationDialog } from "./manual-allocation-dialog";

export function useManualAllocationDialog() {
  const [openDialog, closeDialog] = useDialog(ManualAllocationDialog);
  const { handleSubmitManualAllocation } = useOutgoingContext();
  const { filterOutgoing } = useFilter();

  const handleSubmit = async () => {
    closeDialog();
    await handleSubmitManualAllocation();
  };

  const handleClick = (fuelTransaction: OutgoingFuelTransactionResponse) => {
    openDialog({
      fuelTransaction: fuelTransaction,
      onSubmit: handleSubmit,
      onClose: closeDialog,
      filterDates: {
        fromDate: filterOutgoing.dateFrom,
        toDate: filterOutgoing.dateTo,
      },
    });
  };

  return handleClick;
}
