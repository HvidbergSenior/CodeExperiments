import { Box, Divider, Link, Typography } from "@mui/material";
import { ReactNode } from "react";
import { pdfTranslations } from "../../../translations/pdf/pdf-translations";

interface Props {
  title: string;
  pageNumber: number;
  children?: ReactNode | ReactNode[];
}

export const PdfContainer = ({ title, children, pageNumber }: Props) => {
  const webAddress = "www.biofuel-express.com";

  return (
    <div
      style={{
        position: "relative",
        height: "29.7cm",
        width: "21cm",
        backgroundColor: "white",
        printColorAdjust: "exact",
        WebkitPrintColorAdjust: "exact",
        padding: "1.9cm 1.9cm 1.9cm 1.9cm",
        border: "0px solid black",
      }}
    >
      <Box
        display="flex"
        flexDirection="row"
        justifyContent="space-between"
        alignItems="start"
      >
        <Box display="flex" flexDirection="column">
          <Typography variant="pdfHeaderTitle">
            {pdfTranslations.headerTitle}
          </Typography>
          <Typography variant="pdfPageTitle">{title}</Typography>
        </Box>
        <div style={{ marginTop: "-0.45cm" }}>
          <img src="assets/menu_logo.png" />
        </div>
      </Box>
      {children}
      <Box
        display="flex"
        flexDirection="column"
        justifyContent="center"
        sx={{ bottom: "0.8cm", position: "absolute", ml: "-1.9cm" }}
        width="21cm"
      >
        <Divider
          sx={{
            width: "100%",
            backgroundColor: (theme) => theme.palette.primary.light,
          }}
        />
        <Box
          display="flex"
          flexDirection="row"
          justifyContent="space-between"
          width="100%"
          padding="0.8cm 1.9cm 0cm 1.9cm"
        >
          <Link
            sx={{
              color: (theme) => theme.palette.common.black,
              textDecoration: "none",
            }}
            variant="body2"
          >
            {webAddress}
          </Link>
          <Typography variant="body2">{pageNumber}</Typography>
        </Box>
      </Box>
    </div>
  );
};
