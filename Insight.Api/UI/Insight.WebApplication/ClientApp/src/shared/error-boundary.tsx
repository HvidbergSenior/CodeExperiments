import ErrorOutlineIcon from "@mui/icons-material/ErrorOutline";
import { Stack, Typography } from "@mui/material";
import type { ReactNode } from "react";
import { Component } from "react";
import { translations } from "../translations";

interface Props {
  children: ReactNode;
}

interface State {
  hasError: boolean;
  message?: string;
  showProblemDetails: boolean;
}

export class ErrorBoundary extends Component<Props, State> {
  public state: State = {
    hasError: false,
    showProblemDetails: false,
  };

  public static getDerivedStateFromError(error: Error): State {
    return {
      hasError: true,
      showProblemDetails: false,
      message: error.message,
    };
  }

  public render() {
    if (this.state.hasError) {
      return (
        <Stack
          spacing={2}
          display="flex"
          flexDirection="column"
          alignItems="center"
        >
          <ErrorOutlineIcon color="error" sx={{ fontSize: "40px" }} />
          <Typography component="span" variant="h5">
            {translations.errorTranslations.errorBoundaryMessage}
          </Typography>
          <Typography
            component="span"
            variant="body1"
            width={400}
            align="center"
          >
            {this.state.message}
          </Typography>
        </Stack>
      );
    }

    return this.props.children;
  }
}
