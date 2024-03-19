import { Box, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import { Progress } from "../../../../api/api";
import EmptyStateBox from "../../../../components/empty-state/empty-state-box";
import { theme } from "../../../../theme/theme";
import { customerPortalTranslations } from "../../../../translations/pages/customer-portal-translations";
import { formatNumber } from "../../../../util/formatters/formatters";

const spaceBetweenTitleAndBody = "11px";

interface Props {
  progress: Progress;
}

export const EmissionDevelopmentView = ({ progress }: Props) => {
  const barOptions: ApexOptions = {
    series: [
      {
        name: customerPortalTranslations.sustainability.emissionProgress
          .legendEmissions,
        data: progress.emissions,
        color: theme.palette.common.black,
      },
      {
        name: customerPortalTranslations.sustainability.emissionProgress
          .legendAchivedEmissions,
        data: progress.emissionReduction,
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
    },
    yaxis: {
      title: {
        text: customerPortalTranslations.sustainability.emissionProgress.unit,
      },
      labels: {
        formatter: function (value) {
          return formatNumber(value);
        },
      },
    },
  };
  return (
    <Box>
      <Typography mb={spaceBetweenTitleAndBody} variant="h5">
        {customerPortalTranslations.sustainability.emissionProgress.title}
      </Typography>
      {progress.emissionReduction.length === 0 ? (
        <EmptyStateBox height="250px" width="100%" />
      ) : (
        <ReactApexChart
          options={barOptions}
          series={barOptions.series}
          type="bar"
          height={200}
        />
      )}
    </Box>
  );
};
