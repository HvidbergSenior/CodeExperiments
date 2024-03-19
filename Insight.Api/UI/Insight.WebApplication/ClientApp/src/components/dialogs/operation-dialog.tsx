import { Backdrop, Breakpoint, CircularProgress } from "@mui/material";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import Typography from "@mui/material/Typography";
import React, { Fragment, type ReactNode } from "react";
import { DialogBaseProps } from "../../shared/types";
import { commonTranslations } from "../../translations/common";
import { DialogStackTitle } from "./dialog-stack-title";
import { useDialog } from "../../shared/dialog/use-dialog";
import { ConfirmDialog } from "./confirm-dialog";

interface Props extends DialogBaseProps {
  onConfirm?: () => void;
  title: string;
  cancelTitle?: string;
  submitTitle?: string;
  children?: ReactNode;
  description?: string;
  disableSubmit?: boolean;
  isLoading?: boolean;
  maxWidth?: Breakpoint | false;
  backgroundColor?: string;
  showSubmit?: boolean;
  scroll?: "paper" | "body" | undefined;
  isDirty?: boolean;
}

export const OperationDialog = ({
  onClose,
  onConfirm,
  title,
  description,
  cancelTitle = commonTranslations.cancel,
  submitTitle = commonTranslations.submit,
  children,
  disableSubmit = false,
  isLoading = false,
  maxWidth = "md",
  backgroundColor = "white",
  showSubmit = true,
  scroll = "paper",
  isDirty = false,
  ...props
}: Props) => {
  const confirmClose = useConfirmClose(isDirty, onClose);

  return (
    <Fragment>
      <Backdrop
        open={Boolean(isLoading)}
        sx={{
          color: (theme) => theme.palette.primary.main,
          zIndex: (theme) => theme.zIndex.modal + 1,
        }}
      >
        <CircularProgress color="inherit" />
      </Backdrop>
      <Dialog open={props.isOpen} maxWidth={maxWidth} scroll={scroll}>
        <DialogStackTitle onClose={() => confirmClose()}>
          <Typography component="span" variant="h2">
            {title}
          </Typography>
        </DialogStackTitle>
        <DialogContent>
          {description && (
            <Typography variant="caption" color={"primary.main"}>
              {description}
            </Typography>
          )}
          {children && children}
        </DialogContent>
        <DialogActions>
          {cancelTitle && (
            <Button variant="outlined" onClick={() => confirmClose()}>
              {cancelTitle}
            </Button>
          )}
          {showSubmit && (
            <Button
              variant="contained"
              color="primary"
              onClick={onConfirm}
              disabled={disableSubmit}
            >
              {submitTitle}
            </Button>
          )}
        </DialogActions>
      </Dialog>
    </Fragment>
  );
};

function useConfirmClose(isDirty: boolean, onClose: (() => void) | undefined) {
  const [openUnsavedChanges, closeUnsavedChanges] = useDialog(ConfirmDialog);

  const confirmClose = React.useCallback(() => {
    if (isDirty) {
      openUnsavedChanges({
        onConfirm: () => {
          closeUnsavedChanges();
          onClose?.();
        },
        title: commonTranslations.exitDirtyForm.title,
        description: commonTranslations.exitDirtyForm.description,
        cancelTitle: commonTranslations.exitDirtyForm.stay,
        confirmTitle: commonTranslations.exitDirtyForm.discardChanges,
      });
    } else {
      onClose?.();
    }
  }, [isDirty, onClose, openUnsavedChanges, closeUnsavedChanges]);

  return confirmClose;
}
