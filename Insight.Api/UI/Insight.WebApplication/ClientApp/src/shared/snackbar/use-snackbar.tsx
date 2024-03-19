import { useContext } from "react";
import { SnackBarContext, SnackBarContextActions } from "./snackbar-context";

export const useSnackBar = (): SnackBarContextActions => {
  const context = useContext(SnackBarContext);

  if (!context) {
    throw new Error("useSnackBar must be used within an SnackBarProvider");
  }

  return context;
};
