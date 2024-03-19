import { Box, Stack, CircularProgress } from "@mui/material";

export function AuthLoading() {
  return (
    <Box
      display="flex"
      width="100%"
      height="100vh"
      justifyContent="center"
      alignItems="center"
    >
      <Stack direction="column" spacing="10px" alignItems="center">
        <CircularProgress />
      </Stack>
    </Box>
  );
}
