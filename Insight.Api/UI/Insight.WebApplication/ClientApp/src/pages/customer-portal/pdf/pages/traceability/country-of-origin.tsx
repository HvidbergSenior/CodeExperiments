import { Stack, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";
import { spaceBetweenSections } from "../summary/summary-page";
import { getGreenPalette } from "../util";
import { Country } from "../../../../../api/api";

interface Props {
  countries: Country[];
}

export const CountryOfOrigin = ({ countries }: Props) => {
  const feedstockOptions: ApexOptions = {
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
      fontFamily: "Open Sans",
      formatter: function (seriesName, opts) {
        return `${seriesName}: ${opts.w.globals.series[opts.seriesIndex]}%`;
      },
      floating: true,
      offsetX: 10,
      width: 200,
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
        {pdfTranslations.traceabilityPageTranslations.countryOfOriginTitle}
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
