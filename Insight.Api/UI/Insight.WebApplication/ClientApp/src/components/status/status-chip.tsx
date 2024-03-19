import type { ChipProps } from "@mui/material/Chip";
import Chip from "@mui/material/Chip";

interface Props extends ChipProps {
  mainColor: string;
  backgroundColor: string;
}

export function StatusChip({ mainColor, backgroundColor, sx, ...props }: Props) {
  return (
    <Chip
      component="span"
      variant="outlined"
      size="small"
      {...props}
      sx={{
        ...sx,
        backgroundColor: (_) => backgroundColor,
        border: "1px solid",
        color: (_) => mainColor,
      }}
    />
  );
}
