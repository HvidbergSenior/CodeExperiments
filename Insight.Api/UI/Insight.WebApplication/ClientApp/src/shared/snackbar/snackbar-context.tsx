import { Alert, AlertColor, Snackbar, Typography } from "@mui/material";
import { useState } from "react";

import { createContext } from "react";

export type SnackBarContextActions = {
  showSnackBar: (text: string, typeColor: AlertColor) => void;
};

export const SnackBarContext = createContext({} as SnackBarContextActions);

interface SnackBarContextProviderProps {
  children: React.ReactNode;
}

export const SnackBarProvider = ({
  children,
}: SnackBarContextProviderProps) => {
  const [open, setOpen] = useState<boolean>(false);
  const [message, setMessage] = useState<string>("");
  const [typeColor, setTypeColor] = useState<AlertColor>("warning");

  const showSnackBar = (text: string, color: AlertColor) => {
    setMessage(text);
    setTypeColor(color);
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  return (
    <SnackBarContext.Provider value={{ showSnackBar }}>
      <Snackbar
        open={open}
        autoHideDuration={20000}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
        onClose={handleClose}
        sx={{ display: "flex", alignItems: "center" }}
      >
        <Alert
          sx={{ display: "flex", alignItems: "center" }}
          onClose={handleClose}
          severity={typeColor}
        >
          <Typography>{message}</Typography>
        </Alert>
      </Snackbar>
      {children}
    </SnackBarContext.Provider>
  );
};
