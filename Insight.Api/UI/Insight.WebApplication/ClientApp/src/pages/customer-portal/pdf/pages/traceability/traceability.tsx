import { Stack } from "@mui/material";
import { PdfContainer } from "../../pdf-container";
import { CountryOfOrigin } from "./country-of-origin";
import { FeedStocks } from "./feedstock";
import { Country, Feedstock } from "../../../../../api/api";
import { traceabilityPageTranslations } from "../../../../../translations/pdf/traceability-page-translations";
interface Props {
  pageNumber: number;
  feedStockData: Feedstock[];
  countries: Country[];
}
export const Traceability = ({
  pageNumber,
  feedStockData,
  countries,
}: Props) => {
  return (
    <PdfContainer
      title={traceabilityPageTranslations.title}
      pageNumber={pageNumber}
    >
      <Stack>
        <FeedStocks feedStockData={feedStockData} />
        <CountryOfOrigin countries={countries} />
      </Stack>
    </PdfContainer>
  );
};
