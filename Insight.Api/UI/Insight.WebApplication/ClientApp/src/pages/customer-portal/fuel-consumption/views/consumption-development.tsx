import { Stack, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import { ConsumptionDevelopment } from "../../../../api/api";
import { customerPortalTranslations } from "../../../../translations/pages/customer-portal-translations";
import { formatNumber } from "../../../../util/formatters/formatters";
import { getFuelColor } from "../../pdf/pages/util";
import EmptyStateBox from "../../../../components/empty-state/empty-state-box";
import useMediaQuery from "@mui/material/useMediaQuery";

const spaceBetweenTitleAndBody = "11px";

interface Props {
  consumptionDevelopment: ConsumptionDevelopment;
}

export const ConsumptionDevelopmentView = ({
  consumptionDevelopment,
}: Props) => {
  const matches = useMediaQuery("(min-width:600px)");
  const barOptions: ApexOptions = {
    series: consumptionDevelopment.series.map((item) => ({
      data: item.data,
      name: item.productNameEnumeration,
      color: getFuelColor.get(item.productNameEnumeration),
    })),
    plotOptions: {
      bar: {
        dataLabels: {
          total: {
            enabled: matches ? true : false,
          },
        },
        columnWidth: matches ? 20 : 10,
      },
    },
    dataLabels: {
      enabled: false,
      style: {
        fontFamily: "Open Sans",
      },
      formatter: function (_, opt) {
        return formatNumber(
          opt.globals.stackedSeriesTotals[opt.dataPointIndex],
        );
      },
    },
    legend: {
      fontFamily: "Open Sans",
    },
    chart: {
      type: "bar",
      height: "38px",
      stacked: true,
      toolbar: {
        show: false,
      },
      zoom: {
        enabled: false,
      },
    },
    xaxis: {
      type: "category",
      categories: consumptionDevelopment.categories,
      labels: {
        style: {
          fontFamily: "Open Sans",
        },
      },
    },
    yaxis: {
      title: {
        text: "Liters",
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

  return (
    <Stack>
      <Typography mb={spaceBetweenTitleAndBody} variant="h5">
        {
          customerPortalTranslations.fuelConsumption.consumptionDevelopment
            .columnChartTitle
        }
      </Typography>

      {consumptionDevelopment.series.length > 0 && (
        <ReactApexChart
          type="bar"
          options={barOptions}
          series={barOptions.series}
          height={300}
        />
      )}
      {consumptionDevelopment.series.length === 0 && (
        <EmptyStateBox height="250px" width="100%" />
      )}
    </Stack>
  );
};
