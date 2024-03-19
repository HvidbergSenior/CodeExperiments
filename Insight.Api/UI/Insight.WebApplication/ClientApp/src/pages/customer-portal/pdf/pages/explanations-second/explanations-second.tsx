import { Box, Stack, Typography } from "@mui/material";
import { PdfContainer } from "../../pdf-container";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";
import {
  spaceBetweenSections,
  spaceBetweenTitleAndBody,
} from "../summary/summary-page";
import { GridText } from "../explanations-first/explanations-first";
interface Props {
  pageNumber: number;
}
export const ExplanationsSecond = ({ pageNumber }: Props) => {
  return (
    <PdfContainer title={"Explanations & Definitions"} pageNumber={pageNumber}>
      <Stack>
        <Typography mt={spaceBetweenSections} variant="pdf_h5">
          {
            pdfTranslations.explanationsSecondPageTranslations
              .greenHouseGasEmissionSavingsTitle
          }
        </Typography>
        <Typography
          mt={spaceBetweenTitleAndBody}
          sx={{ whiteSpace: "pre-line" }}
          variant="pdf_body4"
        >
          {pdfTranslations.explanationsSecondPageTranslations.greenHouseText}
        </Typography>
      </Stack>
      <Stack>
        <Typography mt={spaceBetweenSections} variant="pdf_h5">
          {
            pdfTranslations.explanationsSecondPageTranslations
              .fossilFuelComparatorsTitle
          }
        </Typography>
        <Typography
          sx={{ whiteSpace: "pre-line" }}
          mt={spaceBetweenTitleAndBody}
          variant="pdf_body4"
        >
          {
            pdfTranslations.explanationsSecondPageTranslations
              .fossibleFuelComparatorDescription
          }
        </Typography>
        <Box
          sx={{
            display: "grid",
            gridTemplateColumns: "4fr 1fr",
            marginTop: spaceBetweenTitleAndBody,
            alignItems: "flex-end",
          }}
          gap={2}
        >
          <GridText mr="1cm">
            {pdfTranslations.explanationsSecondPageTranslations.fossilText1}
          </GridText>
          <GridText>94 gCO2eq/MJ</GridText>
          <GridText mr="1cm">
            {pdfTranslations.explanationsSecondPageTranslations.fossilText2}
          </GridText>
          <GridText>183 gCO2eq/MJ</GridText>
          <GridText mr="1cm">
            {pdfTranslations.explanationsSecondPageTranslations.fossilText3}
          </GridText>
          <GridText>212 gCO2eq/MJ</GridText>
          <GridText mr="1cm">
            {pdfTranslations.explanationsSecondPageTranslations.fossilText4}
          </GridText>
          <GridText>80 gCO2eq/MJ</GridText>
          <GridText mr="1cm">
            {pdfTranslations.explanationsSecondPageTranslations.fossilText5}
          </GridText>
          <GridText>124 gCO2eq/MJ</GridText>
        </Box>
      </Stack>
    </PdfContainer>
  );
};
