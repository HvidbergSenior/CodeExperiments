import IconButton from "@mui/material/IconButton";
import CloseIcon from "@mui/icons-material/Close";
interface Props {
  onClose?: () => void;
}

export function CloseDialog({ onClose }: Props) {
  return (
    <IconButton
      onClick={onClose}
      sx={{
        position: "absolute",
        right: 20,
        top: 20,
        color: (theme) => theme.palette.grey[500],
      }}
    >
      <CloseIcon />
    </IconButton>
  );
}
