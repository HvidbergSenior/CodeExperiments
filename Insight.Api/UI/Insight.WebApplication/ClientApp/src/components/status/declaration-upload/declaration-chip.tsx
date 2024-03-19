import type { ChipProps } from "@mui/material/Chip";
import { IncomingDeclarationState } from "../../../api/api";
import { theme } from "../../../theme/theme";
import { translations } from "../../../translations";
import { StatusChip } from "../status-chip";

type DeclarationStates = IncomingDeclarationState | "Published" | "Registered";
interface Props extends ChipProps {
  state: DeclarationStates;
}

export function DeclarationStateChip({ state, ...props }: Props) {
  const labels = useLabels();

  return (
    <StatusChip
      {...props}
      label={labels[state]}
      mainColor={useColor()[state]}
      backgroundColor={useBackgroundColor()[state]}
    />
  );
}

export function useLabels(): Record<DeclarationStates, string> {
  return {
    Temporary:
      translations.incomingTranslations.declarationUploadTranslations.status
        .temporary,
    New: translations.incomingTranslations.declarationUploadTranslations.status
      .new,
    Reconciled:
      translations.incomingTranslations.declarationUploadTranslations.status
        .reconciled,
    Allocated:
      translations.incomingTranslations.declarationUploadTranslations.status
        .allocated,
    Published:
      translations.incomingTranslations.declarationUploadTranslations.status
        .published,
    Registered:
      translations.incomingTranslations.declarationUploadTranslations.status
        .registered,
  };
}

export function useColor(): Record<DeclarationStates, string> {
  return {
    Temporary: theme.palette.Yellow1.main,
    New: theme.palette.Yellow1.main,
    Reconciled: theme.palette.Purple1.main,
    Allocated: theme.palette.Blue1.main,
    Published: theme.palette.Green15.main,
    Registered: theme.palette.Purple1.main,
  };
}

export function useBackgroundColor(): Record<DeclarationStates, string> {
  return {
    Temporary: theme.palette.Yellow2.main,
    New: theme.palette.Yellow2.main,
    Reconciled: theme.palette.Purple2.main,
    Allocated: theme.palette.Blue2.main,
    Published: theme.palette.Green16.main,
    Registered: theme.palette.Purple2.main,
  };
}
