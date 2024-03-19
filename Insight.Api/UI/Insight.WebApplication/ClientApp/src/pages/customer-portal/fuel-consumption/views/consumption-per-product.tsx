import { Stack, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import { ConsumptionPerProduct } from "../../../../api/api";
import { customerPortalTranslations } from "../../../../translations/pages/customer-portal-translations";
import { formatNumber } from "../../../../util/formatters/formatters";
import { getFuelColor } from "../../pdf/pages/util";
import EmptyStateBox from "../../../../components/empty-state/empty-state-box";

const spaceBetweenTitleAndBody = "11px";

interface Props {
  consumptionPerProduct: ConsumptionPerProduct;
}

export const ConsumptionPerProductView = ({ consumptionPerProduct }: Props) => {
  const treeMapOptions: ApexOptions = {
    chart: {
      type: "treemap",
      toolbar: {
        show: false,
      },
    },
    dataLabels: {
      enabled: true,
      style: {
        fontSize: "12px",
        fontFamily: "Open Sans",
      },
      formatter: function (text: string, op) {
        return `${text}: ${formatNumber(op.value)} L`;
      },
      offsetY: -4,
    },
    plotOptions: {
      treemap: {
        distributed: true,
      },
    },
    tooltip: {
      y: {
        formatter: function (val) {
          return `${formatNumber(val)}`;
        },
        title: {
          formatter: function (seriesName) {
            return seriesName;
          },
        },
      },
    },
    series: [
      {
        data: consumptionPerProduct.data
          .sort((a, b) => (a.value > b.value ? -1 : 1))
          .map((item) => ({
            x: item.productNameEnumeration,
            y: item.value.toFixed(0),
            fillColor: getFuelColor.get(item.productNameEnumeration),
          })),
      },
    ],
  };

  return (
    <Stack>
      <Typography mb={spaceBetweenTitleAndBody} variant="h5">
        {
          customerPortalTranslations.fuelConsumption.consumptionPerProduct
            .treeMapTitle
        }
      </Typography>

      {consumptionPerProduct.data.length > 0 && (
        <ReactApexChart
          options={treeMapOptions}
          series={treeMapOptions.series}
          type="treemap"
          height={250}
          width="100%"
        />
      )}
      {consumptionPerProduct.data.length === 0 && (
        <EmptyStateBox height="250px" width="100%" />
      )}
    </Stack>
  );
};
