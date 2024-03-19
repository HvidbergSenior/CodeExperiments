import { Stack, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";
import { spaceBetweenSections } from "../summary/summary-page";
import { getGreenPalette } from "../util";
import { Feedstock } from "../../../../../api/api";

interface Props {
  feedStockData: Feedstock[];
}
export const FeedStocks = ({ feedStockData }: Props) => {
  const feedstockOptions: ApexOptions = {
    series: feedStockData.map((item) => item.percentage),
    labels: feedStockData.map((item) => item.name),
    chart: {
      type: "donut",
    },
    colors: getGreenPalette,
    dataLabels: {
      enabled: false,
    },
    legend: {
      fontFamily: "Open Sans",
      floating: true,
      offsetX: 10,
      width: 200,
      formatter: function (seriesName, opts) {
        return `${seriesName}: ${opts.w.globals.series[opts.seriesIndex]}%`;
      },
    },
    plotOptions: {
      pie: {
        offsetX: -150,
      },
    },
  };
  return (
    <Stack mt={spaceBetweenSections}>
      <Typography mb="0.1cm" variant="pdf_h5">
        {pdfTranslations.traceabilityPageTranslations.feedstocksTitle}
      </Typography>
      <ReactApexChart
        type="donut"
        options={feedstockOptions}
        series={feedstockOptions.series}
        height={340}
      />
    </Stack>
  );
};
