import { Stack } from "@mui/material";
import { useEffect, useState } from "react";
import { authorizedHttpClient } from "../../../api";
import { GetSustainabilityReportPdfResponse } from "../../../api/api";
import { pdfTranslations } from "../../../translations/pdf/pdf-translations";
import {
  decodeUrlParams,
  ensureCustomUrlFilterParams,
} from "../../../util/url/url-params";
import CircularProgress from "@mui/material/CircularProgress";
import { AcronymsAndGlossaryOne } from "./pages/acronyms-and-glossary/acronyms-and-glossary-one";
import { CarbonFootprint } from "./pages/carbon-footprint/carbon-footprint";
import { ExplanationsFirst } from "./pages/explanations-first/explanations-first";
import { ExplanationsSecond } from "./pages/explanations-second/explanations-second";
import { Frontpage } from "./pages/frontpage/frontpage";
import { FuelConsumption } from "./pages/fuel-consumption/fuel-consumption";
import { ProofOfSustainabilityPage } from "./pages/proof-of-sustainability/proof-of-sustainability-page";
import { ZeroDeclarations } from "./pages/proof-of-sustainability/zero-declarations-page";
import { SummaryPage } from "./pages/summary/summary-page";
import { Traceability } from "./pages/traceability/traceability";
import { ExplanationsThird } from "./pages/explanations-three/explanation-three";
import { AcronymsAndGlossaryTwo } from "./pages/acronyms-and-glossary/acronyms-and-glossary-two";

export const SustainabilityReportPdf = () => {
  const [sustainabilityReportData, setSustainabilityReportData] = useState<
    GetSustainabilityReportPdfResponse | undefined
  >(undefined);

  const [dateRangeForReport, setDataRangeForReport] = useState<{
    dateFrom: string;
    dateTo: string;
  }>({ dateFrom: "-", dateTo: "-" });
  const [username, setUsername] = useState<string>("-");

  useEffect(() => {
    try {
      //Decode url to get filter parameters (when you download the mock pdf version, you can see the date range has been set to the one chosen in the customer portal)
      const url = window.location.href;
      const decodedParams = decodeUrlParams(url);
      const parameters = ensureCustomUrlFilterParams(decodedParams);
      setDataRangeForReport({
        dateFrom: parameters.fromDate,
        dateTo: parameters.toDate,
      });
      setUsername(parameters.username.replace("%40", "@"));

      // Call endpoint to get pdf data from endpoint. Handles response in promise
      authorizedHttpClient.api
        .getSustainabilityReportPdf({
          dateTo: parameters.fromDate,
          dateFrom: parameters.toDate,
          customerIds: parameters.accountsIds,
          maxColumns: 12,
          productNames: parameters.fuels,
          customerNumbers: [],
        })
        .then((response) => setSustainabilityReportData(response.data));
    } catch (error) {}
  }, []);

  if (sustainabilityReportData === undefined) {
    return <CircularProgress />;
  }

  // The below html renders the pdf with real data.
  // Line 133 in proof-of-sustainability-page (ProofOfSustainabilityPage element) contains the selector that puppeteer is waiting for.
  return (
    <Stack>
      <Frontpage
        title1={pdfTranslations.frontpageTranslations.reportTitle1}
        title2={pdfTranslations.frontpageTranslations.reportTitle2}
        dateRangeForReport={dateRangeForReport}
      />
      <SummaryPage
        pageNumber={2}
        consumptionStats={sustainabilityReportData.consumptionStats}
        emissionsStats={sustainabilityReportData.emissionsStats}
        username={username}
      />
      <FuelConsumption
        pageNumber={3}
        fuelConsumptionData={sustainabilityReportData}
      />
      <CarbonFootprint
        emissionsStats={sustainabilityReportData.emissionsStats}
        progress={sustainabilityReportData.progress}
        productSpecifications={
          sustainabilityReportData.productSpecificationItems
        }
        pageNumber={4}
      />
      <Traceability
        pageNumber={5}
        countries={sustainabilityReportData.countries}
        feedStockData={sustainabilityReportData.feedstocks}
      />
      {sustainabilityReportData.pdfReportPosResponses.length > 0 ? (
        sustainabilityReportData.pdfReportPosResponses.map((pdfData, index) => (
          <ProofOfSustainabilityPage
            pageNumber={6 + index}
            proofOfSustainability={pdfData}
          />
        ))
      ) : (
        <ZeroDeclarations pageNumber={6} />
      )}
      <ExplanationsFirst
        pageNumber={
          6 + sustainabilityReportData.pdfReportPosResponses.length + 1
        }
      />
      <ExplanationsSecond
        pageNumber={
          6 + sustainabilityReportData.pdfReportPosResponses.length + 1
        }
      />
      <ExplanationsThird
        pageNumber={
          6 + sustainabilityReportData.pdfReportPosResponses.length + 1
        }
      />
      <AcronymsAndGlossaryOne
        pageNumber={
          6 + sustainabilityReportData.pdfReportPosResponses.length + 1
        }
      />
      <AcronymsAndGlossaryTwo
        pageNumber={
          6 + sustainabilityReportData.pdfReportPosResponses.length + 1
        }
      />
    </Stack>
  );
};
