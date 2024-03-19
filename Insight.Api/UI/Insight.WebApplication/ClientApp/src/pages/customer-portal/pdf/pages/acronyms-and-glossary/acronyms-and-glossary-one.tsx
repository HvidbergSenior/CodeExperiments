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

export const AcronymsAndGlossaryOne = ({ pageNumber }: Props) => {
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
          {pdfTranslations.acronymsAndGlossaryTranslations.termAF}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defAF}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termB100}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defB100}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termCO2eq}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defCO2eq}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termCSRD}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defCSRD}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termESG}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defESG}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termGHG}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defGHG}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termGhgBaseline}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defGghBaseline}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termHVO100}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defHVO100}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termISCC}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defISCC}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termMJ}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defMJ}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termPFAD}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defPFAD}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termPLC}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defPLC}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.termPOME}
        </StyledTypography>
        <StyledTypography>
          {pdfTranslations.acronymsAndGlossaryTranslations.defPOME}
        </StyledTypography>
      </Box>
    </PdfContainer>
  );
};
