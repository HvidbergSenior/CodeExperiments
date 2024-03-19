import type { ChipProps } from "@mui/material/Chip";
import { theme } from "../../../theme/theme";
import { translations } from "../../../translations";
import { StatusChip } from "../status-chip";
import { UploadState } from "../../../shared/types";

interface Props extends ChipProps {
  state: UploadState;
}

export function UploadStateChip({ state, ...props }: Props) {
  const labels = useLabels();

  return (
    <StatusChip
      {...props}
      label={labels[state]}
      mainColor={useColor()[state]}
      backgroundColor={useBackgroundColor()[state]}
      sx={{ width: 76 }}
    />
  );
}

export function useLabels(): Record<UploadState, string> {
  return {
    Approved:
      translations.incomingTranslations.declarationUploadTranslations
        .uploadDeclarations.status.approved,
    Error:
      translations.incomingTranslations.declarationUploadTranslations
        .uploadDeclarations.status.error,
  };
}

export function useColor(): Record<UploadState, string> {
  return {
    Approved: theme.palette.Green11.main,
    Error: theme.palette.Red1.main,
  };
}

export function useBackgroundColor(): Record<UploadState, string> {
  return {
    Approved: theme.palette.Green12.main,
    Error: theme.palette.Red2.main,
  };
}
