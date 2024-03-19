import { Box, Stack, Typography } from "@mui/material";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";
import {
  spaceBetweenSections,
  spaceBetweenTitleAndBody,
} from "../summary/summary-page";
import { GridText } from "../explanations-first/explanations-first";
import { PdfContainer } from "../../pdf-container";
import { StyledTypography } from "../acronyms-and-glossary/acronyms-and-glossary-one";
interface Props {
  pageNumber: number;
}
export const ExplanationsThird = ({ pageNumber }: Props) => {
  return (
    <PdfContainer title={"Explanations & Definitions"} pageNumber={pageNumber}>
      <Stack mt={spaceBetweenSections}>
        <Typography variant="pdf_h5">
          {
            pdfTranslations.explanationsThirdPageTranslations
              .scienceBasedTargetsInitiativeTitle
          }
        </Typography>
        <Box
          sx={{
            display: "grid",
            gridTemplateColumns: "1fr 7fr",
            mt: spaceBetweenTitleAndBody,
          }}
          gap={2}
        >
          <GridText>
            {pdfTranslations.explanationsThirdPageTranslations.SBTItext11}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsThirdPageTranslations.SBTItext12}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsThirdPageTranslations.SBTItext21}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsThirdPageTranslations.SBTItext22}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsThirdPageTranslations.SBTItext21}
          </GridText>
          <GridText>
            {pdfTranslations.explanationsThirdPageTranslations.SBTItext22}
          </GridText>
        </Box>
        <Stack mt={spaceBetweenSections}>
          <Typography variant="pdf_h5">
            {
              pdfTranslations.explanationsThirdPageTranslations
                .ghgReportProcedureTitle
            }
          </Typography>
          <Typography
            sx={{ whiteSpace: "pre-line" }}
            mt={spaceBetweenTitleAndBody}
            variant="pdf_body4"
          >
            {
              pdfTranslations.explanationsThirdPageTranslations
                .ghgReportProcedureTitleDescription
            }
          </Typography>
        </Stack>
        <Stack mt={spaceBetweenSections}>
          <Typography variant="pdf_h5">
            {
              pdfTranslations.explanationsThirdPageTranslations
                .furtherInformationTitle
            }
          </Typography>
          <Typography
            sx={{ whiteSpace: "pre-line" }}
            mt={spaceBetweenTitleAndBody}
            variant="pdf_body4"
          >
            {
              pdfTranslations.explanationsThirdPageTranslations
                .furtherInformationDescription
            }
          </Typography>
          <Box
            mt={4}
            sx={{
              display: "grid",
              gridTemplateColumns: "1fr 4fr",
            }}
          >
            <StyledTypography>
              {
                pdfTranslations.explanationsThirdPageTranslations
                  .furtherInformationTerm2
              }
            </StyledTypography>
            <StyledTypography>
              <a
                href={
                  pdfTranslations.explanationsThirdPageTranslations
                    .furtherInformationDef2Link
                }
                target="_blank"
                rel="noopener noreferrer"
              >
                {
                  pdfTranslations.explanationsThirdPageTranslations
                    .furtherInformationDef2LinkText
                }
              </a>
            </StyledTypography>
            <StyledTypography>
              {
                pdfTranslations.explanationsThirdPageTranslations
                  .furtherInformationTerm3
              }
            </StyledTypography>
            <StyledTypography>
              <a
                href={
                  pdfTranslations.explanationsThirdPageTranslations
                    .furtherInformationDef3Link
                }
                target="_blank"
                rel="noopener noreferrer"
              >
                {
                  pdfTranslations.explanationsThirdPageTranslations
                    .furtherInformationDef3LinkText
                }
              </a>
            </StyledTypography>
            <StyledTypography>
              {
                pdfTranslations.explanationsThirdPageTranslations
                  .furtherInformationTerm4
              }
            </StyledTypography>
            <StyledTypography>
              <a
                href={
                  pdfTranslations.explanationsThirdPageTranslations
                    .furtherInformationDef4Link
                }
                target="_blank"
                rel="noopener noreferrer"
              >
                {
                  pdfTranslations.explanationsThirdPageTranslations
                    .furtherInformationDef4LinkText
                }
              </a>
            </StyledTypography>
            <StyledTypography>
              {
                pdfTranslations.explanationsThirdPageTranslations
                  .furtherInformationTerm1
              }
            </StyledTypography>
            <StyledTypography>
              <a
                href={
                  pdfTranslations.explanationsThirdPageTranslations
                    .furtherInformationDef1Link
                }
                target="_blank"
                rel="noopener noreferrer"
              >
                {
                  pdfTranslations.explanationsThirdPageTranslations
                    .furtherInformationDef1LinkText
                }
              </a>
            </StyledTypography>
          </Box>
        </Stack>
      </Stack>
    </PdfContainer>
  );
};
