import { Box } from "@mui/material";
import Skeleton from "@mui/material/Skeleton";
import React from "react";
import { Control, useForm } from "react-hook-form";
import { OutgoingDeclarationByIdResponse } from "../../../api/api";
import { DialogBaseProps } from "../../../shared/types";
import { palette } from "../../../theme/biofuel/palette";
import { commonTranslations } from "../../../translations/common";
import { publishedTabTranslations } from "../../../translations/pages/published-tab-translations";
import { OperationDialog } from "../operation-dialog";
import { DeclarationDetailRow } from "./declaration-detail-row";
import { usePublishDeclaration } from "./use-publish-declaration";

interface Props extends DialogBaseProps {
  declarationId: string;
}

export type DeclarationDetailRowInput = {
  control: Control<OutgoingDeclarationByIdResponse, any>;
  name: keyof OutgoingDeclarationByIdResponse;
  value: string | number | boolean | undefined | null;
};

export const PublishedDetailsDialog = ({ declarationId, ...props }: Props) => {
  const { declaration, loading } = usePublishDeclaration({ declarationId });
  const skeletonRows = ["1", "2", "3", "4", "5"];

  const {
    formState: { isDirty },
    control,
    getValues,
    reset,
  } = useForm<OutgoingDeclarationByIdResponse>({
    defaultValues: declaration,
  });

  React.useEffect(() => {
    reset(declaration);
  }, [loading]);

  type DeclarationKeys = keyof OutgoingDeclarationByIdResponse;
  const keysToExclude: DeclarationKeys[] = [
    "outgoingDeclarationId",
    "getOutgoingDeclarationIncomingDeclarationResponse",
  ];

  const keysOfDeclaration = (
    Object.keys(declaration ?? {}) as (keyof OutgoingDeclarationByIdResponse)[]
  ).filter((key) => !keysToExclude.includes(key));

  const rows: DeclarationDetailRowInput[] = keysOfDeclaration.map((key) => {
    // This if statement is not triggered as we have excluded the key in the const above, however, the useform still thinks we get the key, so
    // it throws an error if the below is not present.
    if (key === "getOutgoingDeclarationIncomingDeclarationResponse") {
      return {
        name: "country",
        value: "",
        control,
      };
    }
    const editRow: DeclarationDetailRowInput = {
      name: key,
      value: getValues(key),
      control,
    };
    return editRow;
  });

  return (
    <OperationDialog
      title={publishedTabTranslations.viewDeclarationDialog.title}
      isOpen={props.isOpen}
      showSubmit={false}
      onClose={props.onClose}
      cancelTitle={commonTranslations.close}
      disableSubmit={!isDirty}
      maxWidth="xl"
      backgroundColor={palette?.Gray1.main}
      isLoading={loading}
      isDirty={isDirty}
      submitTitle={commonTranslations.save}
    >
      <Box display="flex" flexDirection="column">
        {rows.map((data) => DeclarationDetailRow({ data }))}
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
