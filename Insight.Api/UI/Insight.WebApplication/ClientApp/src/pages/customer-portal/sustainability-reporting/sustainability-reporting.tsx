import { Box } from "@mui/material";
import { useState } from "react";
import { DataPageCustomerPortal } from "../../../components/data-page/data-page-customer-portal";
import { LoadingButton } from "../../../components/loading-button.tsx/loading-button";
import { usePdfDownload } from "../../../hooks/pdf-download/use-pdf-download";
import { customerPortalTranslations } from "../../../translations/pages/customer-portal-translations";
import { CustomerPortalFilterBar } from "../customer-portal-filters";
import {
  SustainabilityReportingContextProvider,
  useSustainabilityReportingContext,
} from "./context/sustainability-reporting-context";
import { EmissionDevelopmentView } from "./views/emission-development";
import { EmissionStatsView } from "./views/emission-stats";
import { TraceabilityCountryOfOriginView } from "./views/traceability-country-of-origin";
import { TraceabilityFeedStocksView } from "./views/traceability-feed-stocks";
import { ProductSpecificationTable } from "../pdf/pages/carbon-footprint/product-specification-table";
import { useCustomerPortalContext } from "../customer-portal-context";

export const SustainabilityReportingPage = () => {
  const { filter } = useCustomerPortalContext();
  const [loading, setLoading] = useState(false);
  const { handleDownload } = usePdfDownload({
    fileName: "Biofuel-Express-Sustainability-Report.pdf",
    route: "pdf",
    setLoading: setLoading,
    filter,
  });

  return (
    <SustainabilityReportingContextProvider>
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          alignSelf: "center",
          width: { xs: "100%", sm: "60vw" },
          gap: { xs: "20px", sm: "80px" },
        }}
      >
        <Box
          sx={{
            display: "flex",
            flexDirection: { xs: "column", sm: "row" },
            gap: "10px",
            alignItems: "center",
          }}
        >
          <CustomerPortalFilterBar />
          <LoadingButton
            sx={{ textWrap: "nowrap" }}
            loading={loading}
            variant="contained"
            color="primary"
            onClick={handleDownload}
          >
            {customerPortalTranslations.sustainability.download}
          </LoadingButton>
        </Box>
        <SustainabilityReportingContent />
      </Box>
    </SustainabilityReportingContextProvider>
  );
};

const SustainabilityReportingContent = () => {
  const props = useSustainabilityReportingContext();

  return (
    <DataPageCustomerPortal queryState={props.queryStateSustainabilityReport}>
      {({
        countries,
        feedstocks,
        productSpecificationItems,
        progress,
        emissionsStats,
      }) => (
        <Box
          sx={{
            display: "flex",
            width: "100%",
            flexDirection: "column",
            gap: { xs: "20px", sm: "120px" },
            mb: "160px",
          }}
        >
          <EmissionStatsView emissionsStats={emissionsStats} />
          <ProductSpecificationTable
            productSpecifications={productSpecificationItems}
          />
          <EmissionDevelopmentView progress={progress} />
          <Box
            sx={{
              display: "flex",
              flexDirection: { xs: "column", sm: "row" },
              gap: { xs: 5, sm: 0 },
            }}
          >
            <TraceabilityFeedStocksView feedStocks={feedstocks} />
            <TraceabilityCountryOfOriginView countries={countries} />
          </Box>
        </Box>
      )}
    </DataPageCustomerPortal>
  );
};
