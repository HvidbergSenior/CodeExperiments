import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Paper from "@mui/material/Paper";

import { Typography } from "@mui/material";
import { Outlet } from "react-router-dom";
import { theme } from "../../../theme/theme";
import { outgoingPageTranslations } from "../../../translations/pages/outgoing-page-translations";
import { OutgoingButtonGroup } from "./button-group";
import { minWidthTablePaper } from "../../../shared/constants/constants";

export const OutgoingPage = () => {
  return (
    <Box width="100%" height="100%" justifyContent="center">
      <Box mb={(theme) => theme.spacing(8)}>
        <Typography variant="h1">
          {outgoingPageTranslations.outgoing}
        </Typography>
      </Box>
      <Paper
        sx={{
          display: "flex",
          flexDirection: "column",
          minWidth: minWidthTablePaper,
        }}
      >
        <Grid
          container
          justifyContent="center"
          alignItems="center"
          sx={{ padding: (theme) => theme.spacing(2), mt: theme.spacing(8) }}
          display="flex"
        >
          <OutgoingButtonGroup />
        </Grid>
        <Outlet />
      </Paper>
    </Box>
  );
};
