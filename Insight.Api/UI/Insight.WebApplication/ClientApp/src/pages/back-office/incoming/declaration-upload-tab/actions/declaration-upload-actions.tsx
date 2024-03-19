import AddIcon from "@mui/icons-material/Add";
import { Button, Menu, MenuItem, Stack } from "@mui/material";
import { useState } from "react";
import { IncomingDeclarationSupplier } from "../../../../../api/api";
import { translations } from "../../../../../translations";
import { useUploadDeclarationDialog } from "../upload-dialog/use-upload-declaration-dialog";
import { useConfirmReconciliationDialog } from "./use-confirm-reconciliation-dialog";
import { useDeclarationUploadContext } from "../context/declaration-upload-context";

export function DeclarationUploadActions() {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const isUploadMenuOpen = Boolean(anchorEl);
  const handleUploadClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };
  const { selectedRows, reconcileDeclarations } = useDeclarationUploadContext();
  const openReconcileConfirm = useConfirmReconciliationDialog({
    reconcileDeclarations,
  });
  const fileTemplateSupplier: IncomingDeclarationSupplier[] = ["BFE", "Neste"];
  const openUploadDialog = useUploadDeclarationDialog();
  const handleClose = () => {
    setAnchorEl(null);
  };
  const handleUploadMenuItemClick = (
    incomingDeclarationSupplier: IncomingDeclarationSupplier,
  ) => {
    openUploadDialog(incomingDeclarationSupplier);
    setAnchorEl(null);
  };

  return (
    <Stack direction="row" spacing="10px">
      <Button
        disabled={selectedRows.length === 0}
        variant="contained"
        onClick={() => openReconcileConfirm()}
      >
        {`${
          translations.incomingTranslations.declarationUploadTranslations
            .reconcileButton
        }${selectedRows.length === 0 ? "" : ` (${selectedRows.length})`}`}
      </Button>
      <Button
        variant="contained"
        endIcon={<AddIcon />}
        onClick={handleUploadClick}
      >
        {
          translations.incomingTranslations.declarationUploadTranslations
            .uploadButton
        }
      </Button>
      <Menu anchorEl={anchorEl} open={isUploadMenuOpen} onClose={handleClose}>
        {fileTemplateSupplier.map((supplier, _) => (
          <MenuItem
            key={supplier}
            onClick={() => handleUploadMenuItemClick(supplier)}
          >
            {supplier}
          </MenuItem>
        ))}
      </Menu>
    </Stack>
  );
}
