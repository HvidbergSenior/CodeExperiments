import type { ComponentType } from "react";

export interface DialogBaseProps {
  isOpen: boolean;
  onClose?: () => void;
}

export type TDialogState<P extends object> = {
  isOpen: boolean;
  component: ComponentType<P>;
  componentProps: P;
};

export type UploadState = "Approved" | "Error";
