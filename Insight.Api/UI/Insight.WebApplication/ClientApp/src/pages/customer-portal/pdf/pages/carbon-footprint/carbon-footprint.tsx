import { Box, Stack, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import {
  Emissionsstats,
  ProductSpecificationItem,
  Progress,
} from "../../../../../api/api";
import { theme } from "../../../../../theme/theme";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";
import { formatNumber } from "../../../../../util/formatters/formatters";
import { PdfContainer } from "../../pdf-container";
import { spaceBetweenSections } from "../summary/summary-page";
import { ProductSpecificationTable } from "./product-specification-table";

interface Props {
  pageNumber: number;
  emissionsStats: Emissionsstats;
  progress: Progress;
  productSpecifications: ProductSpecificationItem[];
}

export const CarbonFootprint = ({
  pageNumber,
  progress,
  emissionsStats,
  productSpecifications,
}: Props) => {
  const barOptions: ApexOptions = {
    series: [
      {
        name: pdfTranslations.carbonFootprintPageTranslations.legendEmissions,
        data: progress.emissions,
        color: theme.palette.common.black,
      },
      {
        name: pdfTranslations.carbonFootprintPageTranslations
          .legendAchivedEmissions,
        data: progress.emissions,
        color: theme.palette.primary.main,
      },
    ],

    plotOptions: {
      bar: {
        columnWidth: 15,
      },
    },
    dataLabels: {
      enabled: false,
    },

    chart: {
      stacked: true,
      toolbar: {
        show: false,
      },
    },
    legend: {
      fontFamily: "Open Sans",
    },
    xaxis: {
      type: "category",
      categories: progress.categories,
      labels: {
        style: {
          fontFamily: "Open Sans",
        },
      },
    },
    yaxis: {
      title: {
        text: "kg CO2e",
        style: { fontWeight: 500 },
      },
      labels: {
        formatter: function (value) {
          return formatNumber(value);
        },
        style: {
          fontFamily: "Open Sans",
        },
      },
    },
  };

  const optionsSpeedometer: ApexOptions = {
    chart: {
      height: 280,
      type: "radialBar",
    },
    series: [emissionsStats.emissionSavingsForCircle],
    colors: [theme.palette.primary.main],
    plotOptions: {
      radialBar: {
        startAngle: -90,
        endAngle: 90,
        track: {
          background: theme.palette.common.black,
          startAngle: -90,
          endAngle: 90,
        },
        dataLabels: {
          name: {
            show: false,
          },
          value: {
            fontSize: "30px",
            show: false,
          },
        },
      },
    },
    fill: {
      type: "gradient",
      gradient: {
        shade: "dark",
        type: "horizontal",
        gradientToColors: [theme.palette.primary.main],
        stops: [0, 100],
      },
    },
    stroke: {
      lineCap: "butt",
    },
  };

  return (
    <PdfContainer
      title={pdfTranslations.carbonFootprintPageTranslations.pageTitle}
      pageNumber={pageNumber}
    >
      <Stack position="relative" alignItems="center">
        <Box
          position="absolute"
          sx={{ top: "4.2cm", left: "5.1cm" }}
          width="7cm"
          height="1cm"
        >
          <Box
            position="relative"
            display="flex"
            width="100%"
            height="100%"
            alignItems="center"
            justifyContent="center"
            flexDirection="column"
          >
            <Typography
              mb="0.5cm"
              variant="pdf_h2"
              fontSize="50px"
              sx={{ color: (theme) => theme.palette.primary.main }}
            >
              {new Intl.NumberFormat("da-DK").format(
                emissionsStats.emissionSavingsForCircle,
              )}
              {"%"}
            </Typography>
          </Box>
          <div style={{ textAlign: "center", marginTop: "0.1cm" }}>
            <Typography variant="pdf_h6">
              {
                pdfTranslations.summaryPageTranslations.emissionsOverview
                  .CO2eLabel
              }
            </Typography>
          </div>
        </Box>
        <ReactApexChart
          options={optionsSpeedometer}
          series={optionsSpeedometer.series}
          type="radialBar"
          height={400}
        />
        <Box
          width="14cm"
          display="flex"
          flexDirection="row"
          justifyContent="space-between"
          mt="-1.5cm"
          ml="1cm"
        >
          <Box
            width="4cm"
            display="flex"
            flexDirection="column"
            justifyContent="center"
          >
            <Typography mb={"0.2cm"} variant="pdf_h6">
              {
                pdfTranslations.summaryPageTranslations.emissionsOverview
                  .emissionsReductionTitle
              }
            </Typography>
            <Typography variant="pdf_h2" color={theme.palette.primary.main}>
              {formatNumber(emissionsStats.achievedEmissionReductions)}
            </Typography>
            <Typography variant="pdf_subtitle1">
              {
                pdfTranslations.summaryPageTranslations.emissionsOverview
                  .kgCO2eLabel
              }
            </Typography>
          </Box>
          <Box
            width="4cm"
            display="flex"
            flexDirection="column"
            justifyContent="center"
          >
            <Typography mb={"0.2cm"} variant="pdf_h6">
              {
                pdfTranslations.summaryPageTranslations.emissionsOverview
                  .netEmissionTitle
              }
            </Typography>
            <Typography variant="pdf_h2" color={theme.palette.common.black}>
              {formatNumber(emissionsStats.netEmission)}
            </Typography>
            <Typography variant="pdf_subtitle1">
              {
                pdfTranslations.summaryPageTranslations.emissionsOverview
                  .kgCO2eLabel
              }
            </Typography>
          </Box>
        </Box>
      </Stack>
      <ProductSpecificationTable
        productSpecifications={productSpecifications}
      />
      <Stack mt={spaceBetweenSections}>
        <Typography mb="-0.25cm" variant="pdf_h5">
          {pdfTranslations.carbonFootprintPageTranslations.progressTitle}
        </Typography>
        <ReactApexChart
          options={barOptions}
          series={barOptions.series}
          type="bar"
          height={200}
        />
      </Stack>
    </PdfContainer>
  );
};
