import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import Typography from "@mui/material/Typography";
import type { ReactNode } from "react";
import { DialogBaseProps } from "../../shared/types";
import { DialogStackTitle } from "./dialog-stack-title";
import { errorTranslations } from "../../translations/errors";

interface Props extends DialogBaseProps {
  title: string;
  closeButtonTitle?: string;
  children?: ReactNode;
  description?: string;
  traceID?: string | null | undefined;
}

export const ErrorDialog = ({
  title,
  description,
  closeButtonTitle = "Close",
  children,
  traceID,
  ...props
}: Props) => {
  return (
    <Dialog maxWidth="xs" open={props.isOpen}>
      <DialogStackTitle onClose={props.onClose}>
        <Typography component="span" variant="h2">
          {title}
        </Typography>
      </DialogStackTitle>
      <DialogContent>
        {description && <Typography variant="body1">{description}</Typography>}
        {traceID && (
          <Typography mt="8px" variant="subtitle1">
            {errorTranslations.supportId + traceID}
          </Typography>
        )}
        {children && children}
      </DialogContent>
      <DialogActions>
        <Button variant="contained" color="primary" onClick={props.onClose}>
          {closeButtonTitle}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
