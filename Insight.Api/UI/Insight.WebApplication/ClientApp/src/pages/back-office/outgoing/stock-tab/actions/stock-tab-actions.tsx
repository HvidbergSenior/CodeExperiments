import { Button, Stack } from "@mui/material";
import { translations } from "../../../../../translations";
import { useConfirmAllocationDialog } from "../../outgoing-tab/actions/use-confirm-allocation-dialog";
import { useCreateStockDialog } from "../create-stock-dialog/use-create-stock-dialog";

export function StockTabActions() {
  const openCreateStockDialog = useCreateStockDialog();
  const openConfirmAllocateDialog = useConfirmAllocationDialog();

  const handleCreateStockClick = (
    _event: React.MouseEvent<HTMLButtonElement>,
  ) => {
    openCreateStockDialog();
  };

  const handleAllocateClick = (_event: React.MouseEvent<HTMLButtonElement>) => {
    openConfirmAllocateDialog();
  };

  return (
    <Stack direction="row" spacing="10px">
      <Button
        variant="contained"
        onClick={handleCreateStockClick}
        sx={{ whiteSpace: "nowrap" }}
      >
        {
          translations.outgoingPageTranslations.stockTabTranslations
            .createStockButton
        }
      </Button>
      <Button variant="contained" onClick={handleAllocateClick}>
        {
          translations.outgoingPageTranslations.outgoingTabTranslations
            .allocateButton
        }
      </Button>
    </Stack>
  );
}
