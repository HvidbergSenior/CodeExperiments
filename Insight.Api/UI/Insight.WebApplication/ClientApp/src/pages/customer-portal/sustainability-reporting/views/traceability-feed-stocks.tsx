import { Box, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import { customerPortalTranslations } from "../../../../translations/pages/customer-portal-translations";
import { getGreenPalette } from "../util";
import { Feedstock } from "../../../../api/api";
import EmptyStateBox from "../../../../components/empty-state/empty-state-box";
import { formatPercentage } from "../../../../util/formatters/formatters";

const spaceBetweenTitleAndBody = "11px";

interface Props {
  feedStocks: Feedstock[];
}

export const TraceabilityFeedStocksView = ({ feedStocks }: Props) => {
  const feedstockOptions: ApexOptions = {
    series: feedStocks.map((item) => item.percentage),
    labels: feedStocks.map((item) => item.name),
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
  };

  return (
    <Box sx={{ width: { sm: "50%" } }}>
      <Typography mb={spaceBetweenTitleAndBody} variant="h5">
        {customerPortalTranslations.sustainability.traceability.feedstockTitle}
      </Typography>
      {feedStocks.length === 0 ? (
        <EmptyStateBox height="250px" width="100%" />
      ) : (
        <ReactApexChart
          type="donut"
          options={feedstockOptions}
          series={feedstockOptions.series}
          height={300}
        />
      )}
    </Box>
  );
};
