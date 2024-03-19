import { Box, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import { Country } from "../../../../api/api";
import EmptyStateBox from "../../../../components/empty-state/empty-state-box";
import { customerPortalTranslations } from "../../../../translations/pages/customer-portal-translations";
import { getGreenPalette } from "../util";
import { formatPercentage } from "../../../../util/formatters/formatters";

const spaceBetweenTitleAndBody = "11px";

interface Props {
  countries: Country[];
}

export const TraceabilityCountryOfOriginView = ({ countries }: Props) => {
  const countryOfOriginOptions: ApexOptions = {
    series: countries.map((item) => item.percentage),
    labels: countries.map((item) => item.name),
    chart: {
      type: "donut",
    },
    colors: getGreenPalette,
    dataLabels: {
      enabled: false,
    },
    legend: {
      position: "right",
      fontFamily: "Open Sans",
      formatter: function (seriesName, opts) {
        return `${seriesName}: ${formatPercentage(
          opts.w.globals.series[opts.seriesIndex],
        )}`;
      },
    },
    responsive: [
      {
        breakpoint: 1500,
        options: {
          legend: {
            position: "bottom",
          },
        },
      },
    ],
    plotOptions: {
      pie: {
        customScale: 1,
      },
    },
  };
  return (
    <Box sx={{ width: { sm: "50%" } }}>
      <Typography mb={spaceBetweenTitleAndBody} variant="h5">
        {
          customerPortalTranslations.sustainability.traceability
            .countryOfOriginTitle
        }
      </Typography>
      {countries.length === 0 ? (
        <EmptyStateBox ml={2} height="250px" width="100%" />
      ) : (
        <ReactApexChart
          type="donut"
          options={countryOfOriginOptions}
          series={countryOfOriginOptions.series}
          height={300}
        />
      )}
    </Box>
  );
};
