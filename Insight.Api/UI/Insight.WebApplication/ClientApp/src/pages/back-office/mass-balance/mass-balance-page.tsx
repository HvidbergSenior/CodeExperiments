import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";

import { Typography } from "@mui/material";
import { minWidthTablePaper } from "../../../shared/constants/constants";
import { massBalancePageTranslations } from "../../../translations/pages/mass-balance-page-translations";
import { MassBalanceContent } from "./content/mass-balance-content";

export const MassBalancePage = () => {
  return (
    <Box width="100%" height="100%" justifyContent="center">
      <Box mb={(theme) => theme.spacing(8)}>
        <Typography variant="h1">
          {massBalancePageTranslations.massBalanceTitle}
        </Typography>
      </Box>
      <Paper
        sx={{
          display: "flex",
          flexDirection: "column",
          minWidth: minWidthTablePaper,
        }}
      >
        <MassBalanceContent />
      </Paper>
    </Box>
  );
};
