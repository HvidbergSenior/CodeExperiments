import { Box, Divider, Stack, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import { theme } from "../../../../../theme/theme";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";
import "./../styles.css"; // adjust the path based on your project structure
import { formatNumber } from "../../../../../util/formatters/formatters";
import { ConsumptionStats } from "../../../../../api/api";

interface Props {
  consumptionStats: ConsumptionStats;
}

export type GeneralFuelTypes = "Renewable fuel" | "Fossil fuel";

export const getGeneralFuelColor: Map<string, string> = new Map([
  ["Renewable fuel", theme.palette.primary.main],
  ["Fossil fuel", "#000000"],
]);

export const OverviewConsumption = ({ consumptionStats }: Props) => {
  const optionsConsumption: ApexOptions = {
    series: consumptionStats.data,
    labels: consumptionStats.generalFuelTypes,
    chart: {
      type: "donut",
    },
    colors: consumptionStats.generalFuelTypes.map((fuel) =>
      getGeneralFuelColor.get(fuel),
    ),
    fill: {
      type: "gradient",
      gradient: {
        shade: "dark",
        type: "horizontal",
        gradientToColors: [theme.palette.primary.main],
        stops: [20, 100],
      },
    },
    dataLabels: {
      enabled: false,
    },
    legend: {
      fontFamily: "Open Sans",
      floating: true,
      offsetX: -40,
      offsetY: -20,
    },

    plotOptions: {
      pie: {
        offsetX: -20,
        donut: {
          labels: {
            show: false,
            total: {
              show: false,
            },
            name: {
              show: false,
            },
          },
          size: "80%",
        },
      },
    },
  };

  return (
    <Stack position="relative" width="48%">
      <Typography variant="pdf_h5">
        {pdfTranslations.summaryPageTranslations.consumptionOverview.title}
      </Typography>
      <Box
        position="absolute"
        sx={{ top: "1.4cm", left: "1.9cm" }}
        width="3.3cm"
        height="3.3cm"
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
          <Typography variant="pdf_h3">
            {formatNumber(consumptionStats.consumptionTotalForCircle)}
            {"%"}
          </Typography>
          <div style={{ width: "90px", textAlign: "center" }}>
            <Typography variant="pdf_subtitle1">
              {
                pdfTranslations.summaryPageTranslations.consumptionOverview
                  .renewableShareSubtitle
              }
            </Typography>
          </div>
        </Box>
      </Box>
      <div style={{ marginTop: "0.5cm" }}>
        <ReactApexChart
          options={optionsConsumption}
          series={optionsConsumption.series}
          type="donut"
          width="100%"
          height={160}
        />
      </div>
      <Box
        display="flex"
        flexDirection="row"
        justifyContent="space-between"
        alignItems="center"
        mb="0.3cm"
        mt="1cm"
      >
        <Box display="flex" flexDirection="column" justifyContent="start">
          <Typography variant="pdf_body1">
            {
              pdfTranslations.summaryPageTranslations.consumptionOverview
                .renewableFuelsTitle
            }
          </Typography>
        </Box>
        <Box
          display="flex"
          flexDirection="column"
          alignItems="end"
          color={(theme) => theme.palette.primary.main}
        >
          <Typography variant="pdf_h4">
            {new Intl.NumberFormat("da-DK").format(
              consumptionStats.totalConsumptionFossilFuels,
            )}
          </Typography>
          <Typography variant="pdf_subtitle1">
            {pdfTranslations.summaryPageTranslations.consumptionOverview.liters}
          </Typography>
        </Box>
      </Box>
      <Box
        display="flex"
        flexDirection="row"
        justifyContent="space-between"
        alignItems="center"
        mt="0.3cm"
        mb="0.2cm"
      >
        <Box
          display="flex"
          flexDirection="column"
          justifyContent="start"
          alignItems="start"
        >
          <Typography variant="pdf_body1">
            {
              pdfTranslations.summaryPageTranslations.consumptionOverview
                .fossilFuelsTitle
            }
          </Typography>
        </Box>

        <Box
          display="flex"
          flexDirection="column"
          alignItems="end"
          color={(theme) => theme.palette.common.black}
        >
          <Typography variant="pdf_h4">
            {new Intl.NumberFormat("da-DK").format(
              consumptionStats.totalConsumptionRenewableFuels,
            )}
          </Typography>
          <Typography variant="pdf_subtitle1">
            {pdfTranslations.summaryPageTranslations.consumptionOverview.liters}
          </Typography>
        </Box>
      </Box>
      <Divider />
      <Box
        display="flex"
        flexDirection="row"
        justifyContent="space-between"
        alignItems="center"
        mt="0.3cm"
        mb="0.2cm"
      >
        <Box
          display="flex"
          flexDirection="column"
          justifyContent="start"
          alignItems="start"
        >
          <Typography variant="pdf_body1">
            {
              pdfTranslations.summaryPageTranslations.consumptionOverview
                .totalFuelConsumptionTitle
            }
          </Typography>
        </Box>

        <Box display="flex" flexDirection="column" alignItems="end">
          <Typography variant="pdf_h4">
            {new Intl.NumberFormat("da-DK").format(
              consumptionStats.totalConsumptionAllFuels,
            )}
          </Typography>
          <Typography variant="pdf_subtitle1">
            {pdfTranslations.summaryPageTranslations.consumptionOverview.liters}
          </Typography>
        </Box>
      </Box>
      <Box mt="1cm" display="flex" justifyContent="center">
        <Typography variant="pdf_body3">
          {pdfTranslations.summaryPageTranslations.consumptionOverview.readMore}
        </Typography>
      </Box>
    </Stack>
  );
};
