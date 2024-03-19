import { Button, Stack } from "@mui/material";
import { translations } from "../../../../../translations";
import { useConfirmAllocationDialog } from "./use-confirm-allocation-dialog";

export function OutgoingTabActions() {
  const openConfirmAllocateDialog = useConfirmAllocationDialog();

  const handleAllocateClick = (_event: React.MouseEvent<HTMLButtonElement>) => {
    openConfirmAllocateDialog();
  };

  return (
    <Stack direction="row" spacing="10px">
      <Button
        sx={{ textWrap: "nowrap" }}
        variant="contained"
        onClick={handleAllocateClick}
      >
        {
          translations.outgoingPageTranslations.outgoingTabTranslations
            .allocateButton
        }
      </Button>
    </Stack>
  );
}
