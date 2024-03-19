import { Box } from "@mui/material";
import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}
export const Topbar = ({ children }: Props) => {
  return (
    <Box
      sx={{
        padding: (theme) => theme.spacing(4),
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        gap: (theme) => theme.spacing(4),
      }}
    >
      {children}
    </Box>
  );
};
