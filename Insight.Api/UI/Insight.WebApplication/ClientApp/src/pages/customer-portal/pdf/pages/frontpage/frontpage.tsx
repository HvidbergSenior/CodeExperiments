import { Box, Typography } from "@mui/material";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";

interface Props {
  title1: string;
  title2: string;
  dateRangeForReport: { dateTo: string; dateFrom: string };
}

export const Frontpage = ({ title1, title2, dateRangeForReport }: Props) => {
  return (
    <div
      style={{
        height: "29.7cm",
        width: "21cm",
        backgroundColor: "white",
        printColorAdjust: "exact",
        WebkitPrintColorAdjust: "exact",
        border: "1px solid black",
      }}
    >
      <img
        src="assets/pdf-frontpage.jpg"
        style={{ width: "100%", height: "100%", objectFit: "cover" }}
      />
      <Box
        sx={{
          position: "absolute",
          top: "21.5cm",
          left: "2.4cm",
          lineHeight: "48px",
        }}
      >
        <Typography
          color="white"
          fontSize="45px"
          lineHeight="normal"
          fontWeight="400"
        >
          {title1}
        </Typography>
        <Typography
          color="white"
          fontSize="45px"
          fontWeight="600"
          lineHeight="normal"
        >
          {title2}
        </Typography>
        <Typography
          fontFamily="Open Sans"
          color="white"
          fontSize="22px"
          fontWeight="400"
          lineHeight="normal"
          mt="0.5cm"
        >
          {`${
            pdfTranslations.frontpageTranslations.datePeriodText
          } ${dateRangeForReport.dateFrom //TODO: figure out if they want standard en-GB "DD/MM/YYYY" or if they want "DD.MM.YYYY"
            .replace(/\-/g, ".")} ${
            pdfTranslations.frontpageTranslations.datePeriodPreposition
          } ${dateRangeForReport.dateTo.replace(/\-/g, ".")}`}
        </Typography>
      </Box>
    </div>
  );
};
