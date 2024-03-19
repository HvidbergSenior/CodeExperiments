import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import Typography from "@mui/material/Typography";
import { DialogBaseProps } from "../../shared/types";
import { DialogStackTitle } from "./dialog-stack-title";
import type { ReactNode } from "react";

interface Props extends DialogBaseProps {
  onConfirm: () => void;
  title: string;
  confirmTitle?: string;
  cancelTitle?: string;
  children?: ReactNode;
  description?: string;
}

export const ConfirmDialog = ({
  onConfirm,
  title,
  description,
  cancelTitle,
  confirmTitle = "Confirm",
  children,
  ...props
}: Props) => {
  return (
    <Dialog open={props.isOpen}>
      <DialogStackTitle onClose={props.onClose}>
        <Typography component="span" variant="h2">
          {title}
        </Typography>
      </DialogStackTitle>
      <DialogContent>
        {description && <Typography variant="body1">{description}</Typography>}
        {children && children}
      </DialogContent>
      <DialogActions>
        {cancelTitle && (
          <Button variant="contained" onClick={props.onClose}>
            {cancelTitle}
          </Button>
        )}
        <Button variant="contained" onClick={onConfirm}>
          {confirmTitle}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
