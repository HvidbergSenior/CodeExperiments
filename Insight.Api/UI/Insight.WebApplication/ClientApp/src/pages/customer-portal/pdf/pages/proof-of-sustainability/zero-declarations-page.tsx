import { Stack, Typography } from "@mui/material";
import { proofOfSustainabilityPageTranslations } from "../../../../../translations/pdf/proof-of-sustainability-page-translations";
import { PdfContainer } from "../../pdf-container";
import "./../styles.css"; // adjust the path based on your project structure
import { SELECTOR_FOR_PDF_DOWNLOAD } from "./proof-of-sustainability-page";

interface Props {
  pageNumber: number;
}
export const ZeroDeclarations = ({ pageNumber }: Props) => {
  return (
    <PdfContainer
      title={proofOfSustainabilityPageTranslations.pageTitle}
      pageNumber={pageNumber}
    >
      <Stack
        mt="0.7cm"
        display="flex"
        className={SELECTOR_FOR_PDF_DOWNLOAD} // selector that must be visible before puppeteer downloads the pdf
      >
        <Typography>
          {proofOfSustainabilityPageTranslations.zeroDeclarationsDescription}
        </Typography>
      </Stack>
    </PdfContainer>
  );
};
