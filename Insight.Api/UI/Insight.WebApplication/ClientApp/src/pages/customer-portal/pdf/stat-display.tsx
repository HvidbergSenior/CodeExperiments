import { Box, Stack, Typography } from "@mui/material";
import { spaceBetweenTitleAndBody } from "./pages/summary/summary-page";
interface Props {
  header: string;
  middle: number | string;
  unit: string;
}
export const StatDisplay = ({ header, middle, unit }: Props) => {
  return (
    <Stack>
      <Box
        display="flex"
        flexDirection="row"
        alignItems="center"
        width={"200px"}
      >
        <Typography mr="0.1cm" variant="pdf_h5">
          {header}
        </Typography>
      </Box>
      <Stack
        mt={spaceBetweenTitleAndBody}
        gap="0.1cm"
        display="flex"
        flexDirection="row"
        alignItems="center"
      >
        <Box display="flex" flexDirection="row" alignItems="center">
          <Typography variant="pdf_h2">{`${middle}`}</Typography>
        </Box>
      </Stack>
      <Typography variant="pdf_subtitle1">{unit}</Typography>
    </Stack>
  );
};
