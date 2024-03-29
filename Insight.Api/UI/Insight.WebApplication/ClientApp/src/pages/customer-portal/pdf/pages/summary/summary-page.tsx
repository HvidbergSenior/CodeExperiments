import { Box, Typography } from "@mui/material";
import { ConsumptionStats, Emissionsstats } from "../../../../../api/api";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";
import { formatDate } from "../../../../../util/formatters/formatters";
import { PdfContainer } from "../../pdf-container";
import "./../styles.css"; // adjust the path based on your project structure
import { OverviewConsumption } from "./overview-consumption";
import { OverviewEmission } from "./overview-emissions";

export const spaceBetweenTitleAndBody = "0.3cm";
export const spaceBetweenSections = "1.5cm";

interface Props {
  pageNumber: number;
  consumptionStats: ConsumptionStats;
  emissionsStats: Emissionsstats;
  username: string;
}

export const SummaryPage = ({
  pageNumber,
  consumptionStats,
  emissionsStats,
  username,
}: Props) => {
  return (
    <PdfContainer
      title={pdfTranslations.summaryPageTranslations.pageTitle}
      pageNumber={pageNumber}
    >
      <Box mt="1.5cm" display="flex" flexDirection="column">
        <Typography variant="pdf_h5">
          {pdfTranslations.summaryPageTranslations.introductionTitle}
        </Typography>
        <Typography mt={spaceBetweenTitleAndBody} variant="pdf_body1">
          {pdfTranslations.summaryPageTranslations.introductionDescription}
        </Typography>
      </Box>
      <Box mt={spaceBetweenSections} display="flex" flexDirection="column">
        <Box mt="1cm" display="flex" flexDirection="row" justifyContent="start">
          <OverviewConsumption consumptionStats={consumptionStats} />
          <Box width="1cm" />
          <OverviewEmission emissionsStats={emissionsStats} />
        </Box>
      </Box>
      <Box
        display="flex"
        flexDirection="column"
        sx={{ position: "absolute", bottom: "3cm" }}
      >
        <Typography variant="pdf_h6">
          {pdfTranslations.summaryPageTranslations.aboutThisReportTitle}
        </Typography>
        <Typography mt={spaceBetweenTitleAndBody} variant="pdf_body4">
          {`Generated by ${username} on ${formatDate(new Date())}.`}
        </Typography>
      </Box>
    </PdfContainer>
  );
};
