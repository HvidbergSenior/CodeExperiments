import { Box, Stack, Typography } from "@mui/material";
import { commonTranslations } from "../../translations/common";
import { formatPercentage } from "../../util/formatters/formatters";

interface Props {
  volume?: number;
  ghgWeightedAvg?: number;
  totalNumberOfEntries: number;
  numberOfEntriesLoaded: number;
  customEntriesText?: string;
}

export const TableSummation = ({
  volume,
  ghgWeightedAvg,
  totalNumberOfEntries,
  numberOfEntriesLoaded,
  customEntriesText,
}: Props) => {
  if (ghgWeightedAvg || volume || numberOfEntriesLoaded) {
    return (
      <Stack gap="24px" display="flex" flexDirection="row" paddingTop="16px">
        <Typography variant="body1" fontWeight="600">
          {commonTranslations.tableSummation.summation}
        </Typography>
        {volume && (
          <Box display="flex" flexDirection="row">
            <Typography mr="8px" variant="body1" fontWeight="500">
              {commonTranslations.tableSummation.volume}
            </Typography>
            <Typography variant="body1" fontWeight="400">
              {volume.toLocaleString("da-dk", {
                minimumFractionDigits: 0,
              })}
            </Typography>
          </Box>
        )}

        {ghgWeightedAvg && (
          <Box display="flex" flexDirection="row">
            <Typography mr="8px" variant="body1" fontWeight="500">
              {commonTranslations.tableSummation.ghgWeightedAvg}
            </Typography>
            <Typography variant="body1" fontWeight="400">
              {formatPercentage(ghgWeightedAvg)}
            </Typography>
          </Box>
        )}

        <Box display="flex" flexDirection="row">
          <Typography mr="8px" variant="body1" fontWeight="500">
            {customEntriesText
              ? customEntriesText
              : commonTranslations.tableSummation.entriesLoaded}
          </Typography>
          <Typography variant="body1" fontWeight="400">
            {numberOfEntriesLoaded + " out of " + totalNumberOfEntries}
          </Typography>
        </Box>
      </Stack>
    );
  }
  return <></>;
};
