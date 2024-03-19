import { Box, CircularProgress, Stack, Typography } from "@mui/material";
import { useContext, useState } from "react";

import { createContext } from "react";
import { commonTranslations } from "../translations/common";

export type ActivityIndicatorContextActions = {
  showAcitvityIndicator: (loading: boolean, indicatorSubtitle?: string) => void;
};

export const ActivityIndicatorContext = createContext(
  {} as ActivityIndicatorContextActions,
);

interface ActivityIndicatorContextProviderProps {
  children: React.ReactNode;
}

export const ActivityIndicatorProvider = ({
  children,
}: ActivityIndicatorContextProviderProps) => {
  const [loading, setLoading] = useState<boolean>(false);
  const [subtitle, setSubtitle] = useState<string | undefined>("");

  const showAcitvityIndicator = (
    loading: boolean,
    indicatorSubtitle?: string,
  ) => {
    setSubtitle(indicatorSubtitle);
    setLoading(loading);
  };

  return (
    <ActivityIndicatorContext.Provider value={{ showAcitvityIndicator }}>
      {loading && (
        <Box sx={{}}>
          <Stack
            display="flex"
            flexDirection="row"
            width="200px"
            height="48px"
            position="absolute"
            sx={{
              backgroundColor: (theme) => theme.palette.Gray1.main,
              borderRadius: "100px",
              zIndex: (theme) => theme.zIndex.modal + 1,
              right: "40px",
              top: "24px",
            }}
            alignItems="center"
            justifyContent="center"
            gap="16px"
          >
            <CircularProgress size="25px" color="primary" />
            <Typography variant="h6">
              {subtitle ? subtitle : commonTranslations.loading}
            </Typography>
          </Stack>
        </Box>
      )}

      {children}
    </ActivityIndicatorContext.Provider>
  );
};

export const useActivityIndicator = (): ActivityIndicatorContextActions => {
  const context = useContext(ActivityIndicatorContext);

  if (!context) {
    throw new Error(
      "activity indicator must be used within an ActivityIndicator provider",
    );
  }

  return context;
};
