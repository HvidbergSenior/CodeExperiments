import { Box, Button } from "@mui/material";
import Skeleton from "@mui/material/Skeleton";
import React, { useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { IncomingDeclarationDto } from "../../../api/api";
import { OperationDialog } from "../operation-dialog";
import { DialogBaseProps } from "../../../shared/types";
import { palette } from "../../../theme/biofuel/palette";
import { commonTranslations } from "../../../translations/common";
import { reconciliationTranslations } from "../../../translations/pages/reconcilliation-translations";
import { EditDeclarationData } from "../../../pages/types";
import { EditDeclarationRow } from "./edit-declaration-row";
import { useEditDeclaration } from "./use-edit-declaration";

interface Props extends DialogBaseProps {
  onSubmit: (editedDeclaration: IncomingDeclarationDto) => Promise<void>;
  declarationId: string;
  openedToEdit: boolean;
  readOnlyView?: boolean;
}

export const DeclarationDetailsDialog = ({
  onSubmit,
  declarationId,
  openedToEdit,
  readOnlyView,
  ...props
}: Props) => {
  const [editActive, setEditActive] = useState(openedToEdit);

  const handleOnSubmit: SubmitHandler<IncomingDeclarationDto> = (data) => {
    onSubmit(data);
  };

  const { declaration, loading } = useEditDeclaration({ declarationId });
  const skeletonRows = ["1", "2", "3", "4", "5"];

  const {
    formState: { isDirty },
    control,
    getValues,
    reset,
    handleSubmit,
  } = useForm<IncomingDeclarationDto>({
    defaultValues: declaration,
  });

  React.useEffect(() => {
    reset(declaration);
  }, [loading]);

  type DeclarationKeys = keyof IncomingDeclarationDto;
  const keysToExclude: DeclarationKeys[] = [
    "incomingDeclarationId",
    "posNumber",
    "incomingDeclarationState",
    "incomingDeclarationUploadId",
    "declarationRowNumber",
  ];

  const keysOfDeclaration = (
    Object.keys(declaration ?? {}) as (keyof IncomingDeclarationDto)[]
  ).filter((key) => !keysToExclude.includes(key));

  const rows: EditDeclarationData[] = keysOfDeclaration.map((key) => {
    const editRow: EditDeclarationData = {
      name: key,
      value: getValues(key),
      control,
    };
    return editRow;
  });

  const showEditButton = !readOnlyView && !editActive;

  return (
    <OperationDialog
      title={`${
        reconciliationTranslations.declarationDetailsViewTitle
      } ${declaration?.posNumber.substring(0, 10)}...`}
      isOpen={props.isOpen}
      onConfirm={handleSubmit(handleOnSubmit)}
      onClose={props.onClose}
      cancelTitle={
        editActive ? commonTranslations.cancel : commonTranslations.close
      }
      disableSubmit={!isDirty}
      maxWidth="xl"
      backgroundColor={palette?.Gray1.main}
      showSubmit={editActive}
      isLoading={loading}
      isDirty={isDirty}
      submitTitle={commonTranslations.save}
    >
      <Box
        height="32px"
        mb={4}
        display="flex"
        flexDirection="row"
        justifyContent="end"
      >
        {showEditButton && (
          <Button
            onClick={() => setEditActive(!editActive)}
            variant="contained"
          >
            {commonTranslations.edit}
          </Button>
        )}
      </Box>
      <Box display="flex" flexDirection="column">
        {rows.map((data) => EditDeclarationRow({ data, editActive }))}
      </Box>
      {loading && (
        <Box>
          <Skeleton height="60px" width="500px" />
          {skeletonRows.map((key) => (
            <Box key={key} display="flex" flexDirection="row">
              <Skeleton height="60px" width="100%" />
            </Box>
          ))}
        </Box>
      )}
    </OperationDialog>
  );
};
