import AddIcon from "@mui/icons-material/Add";
import { Box, Button } from "@mui/material";
import { TableComponents } from "react-virtuoso";
import { IncomingDeclarationSupplier } from "../../../../../api/api";
import { OperationDialog } from "../../../../../components/dialogs/operation-dialog";
import { getFixedHeaderAndRowContent } from "../../../../../components/table/virtual-table/header-row-content";
import VirtualTable from "../../../../../components/table/virtual-table/virtual-table";
import { genericVirtualTableComponents } from "../../../../../components/table/virtual-table/virtual-table-components";
import {
  TableContext,
  VirtuosoTableComponentsGroup,
} from "../../../../../components/types";
import { DialogBaseProps } from "../../../../../shared/types";
import { commonTranslations } from "../../../../../translations/common";
import { declarationUploadTranslations } from "../../../../../translations/pages/declaration-upload-translations";
import { UploadResult, useUpload } from "./use-upload";
import { getUploadDialogVirtualColumns } from "./virtual-upload-dialog-columns";

interface Props extends DialogBaseProps {
  onSubmit: (
    batchId: string,
    oldestEntryDate?: string,
    newestEntryDate?: string,
  ) => Promise<void>;
  incomingDeclarationSupplier: IncomingDeclarationSupplier;
}

export const uploadDialogVirtualTableComponents: TableComponents<
  UploadResult,
  TableContext
> = genericVirtualTableComponents;

export const UploadDeclarationDialog = ({
  onSubmit,
  incomingDeclarationSupplier,
  ...props
}: Props) => {
  const {
    isLoading,
    setIsLoading,
    disableSubmit,
    handleFileChange,
    handleUploadClick,
    declarationUploadResult,
    hiddenFileInputRef,
    errorCount,
    uploadId,
    disableUploadDeclarationButton,
    cancelUpload,
  } = useUpload(incomingDeclarationSupplier);

  const handleOnSubmit = () => {
    setIsLoading(true);
    onSubmit(
      uploadId ?? "-",
      declarationUploadResult?.oldestEntryDate,
      declarationUploadResult?.newestEntryDate,
    );
  };

  const fixedHeaderAndRowContentFuelTransaction = getFixedHeaderAndRowContent({
    data:
      declarationUploadResult === undefined ? [] : [declarationUploadResult],
    columns: getUploadDialogVirtualColumns(errorCount),
  });

  const virtualComponentsForTableFuelTransaction: VirtuosoTableComponentsGroup<UploadResult> =
    {
      virtuosoTableComponents: uploadDialogVirtualTableComponents,
      fixedHeaderContent:
        fixedHeaderAndRowContentFuelTransaction.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContentFuelTransaction.rowContent,
    };

  const handleClose = () => {
    cancelUpload();
    props.onClose?.();
  };

  return (
    <OperationDialog
      title={
        declarationUploadTranslations.uploadDeclarations.title +
        ` (${incomingDeclarationSupplier})`
      }
      isOpen={props.isOpen}
      onConfirm={() => handleOnSubmit()}
      onClose={handleClose}
      cancelTitle={commonTranslations.cancel}
      disableSubmit={disableSubmit}
      isLoading={isLoading}
    >
      <Box sx={{ m: 6 }} />
      <Button
        onClick={handleUploadClick}
        disabled={disableUploadDeclarationButton}
        sx={{
          backgroundColor: (theme) => theme.palette.Gray1.main,
          width: "100%",
          height: "68px",
          borderRadius: "20px",
          borderColor: (theme) => theme.palette.Green1.main,
          border: (theme) => `1px dashed ${theme.palette.Gray2.main}`,
          textTransform: "capitalize",
          fontWeight: "600",
          fontSize: "16px",
        }}
        variant="text"
        endIcon={<AddIcon />}
      >
        {commonTranslations.upload}
      </Button>
      <input
        type="file"
        accept=".xlsx"
        ref={hiddenFileInputRef}
        onChange={handleFileChange}
        style={{ display: "none" }}
      />
      <Box sx={{ m: 13 }} />
      <Box height="250px">
        <VirtualTable
          virtualTableComponents={virtualComponentsForTableFuelTransaction}
          data={
            declarationUploadResult === undefined
              ? []
              : [declarationUploadResult]
          }
          loadMore={() => {}}
          tableContext={{
            hasMore: false,
            emptyText:
              declarationUploadTranslations.emptyTextDeclarationUploadTable,
          }}
        />
      </Box>
    </OperationDialog>
  );
};
