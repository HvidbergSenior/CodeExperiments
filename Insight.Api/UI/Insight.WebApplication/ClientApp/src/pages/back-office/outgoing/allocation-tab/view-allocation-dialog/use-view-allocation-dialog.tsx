import { AllocationByIdResponse } from "../../../../../api/api";
import { useDialog } from "../../../../../shared/dialog/use-dialog";
import { useAllocationContext } from "../context/allocation-tab-context";
import { ViewAllocationDialog } from "./view-allocation-dialog";

export function useViewAllocationDialog() {
  const [openDialog, closeDialog] = useDialog(ViewAllocationDialog);
  const { handleSubmitViewAllocation } = useAllocationContext();

  const handleSubmit = async (id: string) => {
    await handleSubmitViewAllocation(id);
    closeDialog();
  };

  const handleClick = (allocation: AllocationByIdResponse) => {
    openDialog({
      allocationId: allocation.id,
      onSubmit: handleSubmit,
      onClose: closeDialog,
    });
  };

  return handleClick;
}
