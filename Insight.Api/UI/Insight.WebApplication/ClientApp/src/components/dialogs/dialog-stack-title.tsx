import DialogTitle from "@mui/material/DialogTitle";
import Stack from "@mui/material/Stack";
import type { ReactNode } from "react";
import { CloseDialog } from "./close-dialog";

interface Props {
  children: ReactNode;
  onClose?: () => void;
}

export function DialogStackTitle({ children, onClose }: Props) {
  return (
    <DialogTitle>
      <Stack spacing={1}>{children}</Stack>
      {onClose && <CloseDialog onClose={onClose} />}
    </DialogTitle>
  );
}
