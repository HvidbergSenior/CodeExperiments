import type { ButtonProps } from "@mui/material/Button";
import Button from "@mui/material/Button";
import CircularProgress from "@mui/material/CircularProgress";
import type { TooltipProps } from "@mui/material/Tooltip";
import Tooltip from "@mui/material/Tooltip";

interface Props extends ButtonProps {
  loading?: boolean;
  hint?: string;
}

export function LoadingButton({ loading, hint, ...props }: Props) {
  const tooltipProps: Omit<TooltipProps, "children"> = {
    arrow: true,
    placement: "top",
    title: props.disabled ? hint ?? "" : "",
  };

  return (
    <Tooltip {...tooltipProps}>
      <div>
        <Button
          {...props}
          onClick={(event) => {
            if (!loading) props.onClick?.(event);
          }}
        >
          {loading ? (
            <CircularProgress size={18} sx={{ color: "inherit" }} />
          ) : (
            props.children
          )}
        </Button>
      </div>
    </Tooltip>
  );
}
