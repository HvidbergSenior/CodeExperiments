import Grid from "@mui/material/Grid";
import CircularProgress from "@mui/material/CircularProgress";

export function DefaultLoadingSkeleton() {
  return (
    <Grid
      container
      justifyContent="center"
      alignItems="center"
      sx={{ width: "100%", minHeight: "320px" }}
    >
      <CircularProgress />
    </Grid>
  );
}
