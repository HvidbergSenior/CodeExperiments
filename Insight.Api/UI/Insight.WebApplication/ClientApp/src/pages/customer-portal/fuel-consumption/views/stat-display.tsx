import { Box, Stack, Typography } from "@mui/material";
interface Props {
  header: string;
  middle: number | string;
  unit: string;
  spaceBetweenTitleAndBody: string;
}
export const StatDisplay = ({
  header,
  middle,
  unit,
  spaceBetweenTitleAndBody,
}: Props) => {
  return (
    <Stack>
      <Box display="flex" flexDirection="row" alignItems="center">
        <Typography mr="3.8px" variant="h5">
          {header}
        </Typography>
      </Box>
      <Stack
        mt={spaceBetweenTitleAndBody}
        display="flex"
        flexDirection="row"
        alignItems="center"
      >
        <Box display="flex" flexDirection="row" alignItems="center">
          <Typography variant="h2">{`${middle}`}</Typography>
        </Box>
      </Stack>
      <Typography variant="subtitle1">{unit}</Typography>
    </Stack>
  );
};
