import { Box, Stack, Typography, styled } from "@mui/material";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";
import { PdfContainer } from "../../pdf-container";
import {
  spaceBetweenSections,
  spaceBetweenTitleAndBody,
} from "../summary/summary-page";

interface Props {
  pageNumber: number;
}

export const GridText = styled(Typography)({
  fontWeight: 400,
  fontSize: "12px",
  lineHeight: "15px",
  color: "black",
  fontFamily: "Open Sans",
});

export const ExplanationsFirst = ({ pageNumber }: Props) => {
  return (
    <PdfContainer title="Explanations & Definitions" pageNumber={pageNumber}>
      <Stack mt={spaceBetweenSections}>
        <Typography mb="1cm" variant="pdf_h5">
          {pdfTranslations.explanationsFirstPageTranslations.supplyChainTitle}
        </Typography>
        <div>
          <img
            src="assets/bfe_supply_chain_qx.jpg" //TODO: replace with correct asset
            style={{ width: "600px", height: "auto" }}
          />
        </div>
        <Box display="flex" flexDirection="column" justifyContent="center">
          <Typography mt="0.3cm" variant="pdf_body4" textAlign="left">
            {
              pdfTranslations.explanationsFirstPageTranslations
                .supplyChainDescriptionPartOne
            }
          </Typography>
          <Typography mt="0.3cm" variant="pdf_body4" textAlign="left">
            {
              pdfTranslations.explanationsFirstPageTranslations
                .supplyChainDescriptionPartTwo
            }
          </Typography>
          <Typography mt="0.3cm" variant="pdf_body4" textAlign="left">
            {
              pdfTranslations.explanationsFirstPageTranslations
                .supplyChainDescriptionPartThree
            }
          </Typography>
        </Box>
      </Stack>
      <Stack mt={spaceBetweenSections}>
        <Typography variant="pdf_h5">
          {
            pdfTranslations.explanationsFirstPageTranslations
              .greenHouseGasEmissionTitle
          }
        </Typography>
        <Box
          sx={{
            mt: spaceBetweenTitleAndBody,
            display: "grid",
            gridTemplateColumns: "1fr 6fr",
            width: "100%",
          }}
          gap={2}
        >
          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.term1}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.def1}
          </GridText>

          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.term2}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.def2}
          </GridText>

          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.term3}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.def3}
          </GridText>

          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.term4}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.def4}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.term5}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.def5}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.term6}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.def6}
          </GridText>

          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.term7}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.def7}
          </GridText>

          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.term8}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.def8}
          </GridText>

          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.term9}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsFirstPageTranslations.def9}
          </GridText>
        </Box>
      </Stack>
    </PdfContainer>
  );
};
