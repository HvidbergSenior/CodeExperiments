import { Box, Typography, styled } from "@mui/material";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";
import { PdfContainer } from "../../pdf-container";
import { spaceBetweenSections } from "../summary/summary-page";
interface Props {
  pageNumber: number;
}
export const StyledTypography = styled(Typography)({
  fontWeight: "500",
  marginBottom: "0.4cm",
  fontSize: "13px",
  fontFamily: "Open Sans",
});

export const AcronymsAndGlossaryTwo = ({ pageNumber }: Props) => {
  return (
    <PdfContainer title="Acronyms & Glossary" pageNumber={pageNumber}>
      <Box
        sx={{
          display: "grid",
          gridTemplateColumns: "1fr 3fr",
          mt: spaceBetweenSections,
        }}
      >
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termPoS}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defPoS}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termRED}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defRED}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termRSO}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defRSO}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termSBTi}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defSBTi}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termTCO2e}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defTCO2e}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termTTW}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defTTW}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termUCO}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defUCO}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termWTT}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defWTT}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termWTW}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defWTW}
        </StyledTypography>
      </Box>
    </PdfContainer>
  );
};
