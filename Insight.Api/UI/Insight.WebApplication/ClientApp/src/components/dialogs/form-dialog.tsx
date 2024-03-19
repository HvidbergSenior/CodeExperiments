import Backdrop from "@mui/material/Backdrop";
import Button from "@mui/material/Button";
import CircularProgress from "@mui/material/CircularProgress";
import Dialog, { DialogProps } from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import Typography from "@mui/material/Typography";
import { Fragment, ReactNode, useCallback } from "react";
import { FieldValues, FormState } from "react-hook-form";
import { DialogBaseProps } from "../../shared/types";
import { DialogStackTitle } from "./dialog-stack-title";
import { LoadingButton } from "../loading-button.tsx/loading-button";
import { useDialog } from "../../shared/dialog/use-dialog";
import { ConfirmDialog } from "./confirm-dialog";

export type FormTexts = {
  title: string;
  description?: string;
  cancel?: string;
  submit?: string;
};

interface Props<FormData extends FieldValues = FieldValues>
  extends DialogBaseProps,
    Omit<DialogProps, "onClose" | "open"> {
  texts: FormTexts;
  children: ReactNode;
  isLoading?: boolean;
  disableHint?: string;
  formState: FormState<FormData>;
  onSubmit: () => Promise<void>;
  promptUnsavedWork?: boolean;
}

export function FormDialog<FormData extends FieldValues = FieldValues>({
  onSubmit,
  onClose,
  isOpen,
  isLoading,
  texts,
  disableHint,
  formState,
  children,
  promptUnsavedWork = false,
  ...props
}: Props<FormData>) {
  const confirmClose = useConfirmClose(
    formState.isDirty && promptUnsavedWork,
    onClose,
  );

  const handleOnClose = useCallback(
    (_?: unknown, reason?: "backdropClick" | "escapeKeyDown") => {
      if (reason === "backdropClick") {
        return;
      }

      confirmClose();
    },
    [confirmClose],
  );

  return (
    <Fragment>
      <Backdrop
        open={Boolean(isLoading)}
        sx={{
          color: (theme) => theme.palette.common.white,
          zIndex: (theme) => theme.zIndex.modal + 1,
        }}
      >
        <CircularProgress color="inherit" />
      </Backdrop>
      <Dialog {...props} open={isOpen} onClose={handleOnClose}>
        <form onSubmit={onSubmit} style={{ display: "contents" }}>
          <DialogStackTitle onClose={handleOnClose}>
            <Typography component="span" variant="h2">
              {texts.title}
            </Typography>
            {texts.description && (
              <Typography component="span" variant="subheading1" color="Gray6.main">
                {texts.description}
              </Typography>
            )}
          </DialogStackTitle>
          <DialogContent>{children}</DialogContent>
          <DialogActions>
            <Button variant="dismiss" onClick={handleOnClose}>
              {texts.cancel ?? <Typography>Cancel</Typography>}
            </Button>
            <LoadingButton
              type="submit"
              variant="contained"
              color="primary"
              disabled={disableHint !== undefined}
              hint={disableHint}
              loading={formState.isSubmitting}
            >
              {texts.submit ?? "Submit"}
            </LoadingButton>
          </DialogActions>
        </form>
      </Dialog>
    </Fragment>
  );
}

function useConfirmClose(isDirty: boolean, onClose: (() => void) | undefined) {
  const [openUnsavedChanges, closeUnsavedChanges] = useDialog(ConfirmDialog);

  const confirmClose = useCallback(() => {
    if (isDirty) {
      openUnsavedChanges({
        onConfirm: () => {
          closeUnsavedChanges();
          onClose?.();
        },
        title: "You have unsaved changes",
        description:
          "You are about to discard changes in the current form. Confirm that you wish to leave the form.",
        cancelTitle: "Stay",
        confirmTitle: "Discard Changes",
      });
    } else {
      onClose?.();
    }
  }, [isDirty, onClose, openUnsavedChanges, closeUnsavedChanges]);

  return confirmClose;
}
