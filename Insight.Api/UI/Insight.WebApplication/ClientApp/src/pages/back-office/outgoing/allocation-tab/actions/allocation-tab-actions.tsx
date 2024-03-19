import { Button, Stack, Chip } from "@mui/material";
import { allocationTabTranslations } from "../../../../../translations/pages/allocation-tab-translations";
import { useConfirmPublishAllocationsDialog } from "./use-confirm-publish-allocations-dialog";
import { useConfirmLockUnlockDialog } from "./use-confirm-lock-unlock-dialog";
import { useAllocationContext } from "../context/allocation-tab-context";
import { useConfirmClearAllocations } from "./use-confirm-clear-allocations";

export function AllocationTabActions() {
  const openConfirmPublishAllocationsDialog =
    useConfirmPublishAllocationsDialog();
  const openConfirmClearAllocationsDialog = useConfirmClearAllocations();
  const {
    queryStateAllocations: { data },
  } = useAllocationContext();

  const openConfirmLockUnlock = useConfirmLockUnlockDialog({
    toBeLocked: !data?.isDraftLocked ?? false,
  });

  return (
    <Stack direction="row" spacing="10px">
      {data?.isDraftLocked && (
        <Chip
          sx={{
            fontSize: "14px",
            padding: "0 4px 0 4px",
            fontWeight: 500,
            border: "1px solid",
            color: (theme) => theme.palette.common.white,
            backgroundColor: (theme) => theme.palette.common.black,
          }}
          label={allocationTabTranslations.draftIsLockedLabel}
        />
      )}
      <Button
        onClick={() => openConfirmClearAllocationsDialog()}
        variant="contained"
        disabled={data?.allocations.length === 0}
      >
        {allocationTabTranslations.clearButton}
      </Button>
      <Button
        onClick={() => openConfirmLockUnlock()}
        variant="contained"
        disabled={data?.allocations.length === 0 && !data?.isDraftLocked}
      >
        {data?.isDraftLocked
          ? allocationTabTranslations.unlockButton
          : allocationTabTranslations.lockButton}
      </Button>
      <Button
        onClick={() => openConfirmPublishAllocationsDialog()}
        variant="contained"
        disabled={!data?.isDraftLocked || data?.allocations.length === 0}
      >
        {allocationTabTranslations.publishButton}
      </Button>
    </Stack>
  );
}
