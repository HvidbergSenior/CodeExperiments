import { Box, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import { ConsumptionStats } from "../../../../api/api";
import { theme } from "../../../../theme/theme";
import { customerPortalTranslations } from "../../../../translations/pages/customer-portal-translations";
import { formatNumber } from "../../../../util/formatters/formatters";
import { StatDisplay } from "./stat-display";

const spaceBetweenTitleAndBody = "11px";

const getGeneralFuelColor: Map<string, string> = new Map([
  ["Renewable fuel", theme.palette.primary.main],
  ["Fossil fuel", "#000000"],
]);

interface Props {
  consumptionStats: ConsumptionStats;
}

export const ConsumptionStatsView = ({ consumptionStats }: Props) => {
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
      type: "solid",
      colors: consumptionStats.generalFuelTypes.map((fuel) =>
        getGeneralFuelColor.get(fuel),
      ),
    },
    dataLabels: {
      enabled: false,
    },
    legend: {
      show: false,
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

    plotOptions: {
      pie: {
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
    <Box
      sx={{
        display: "flex",
        flexDirection: { xs: "column", sm: "row" },
        gap: { xs: "20px", sm: "80px" },
        flexWrap: "wrap",
        marginBottom: { xs: "20px", sm: "0px" },
      }}
    >
      <StatDisplay
        header={
          customerPortalTranslations.fuelConsumption.fuelConsumptionStats
            .consumptionTitle
        }
        middle={formatNumber(consumptionStats.totalConsumptionAllFuels)}
        unit={
          customerPortalTranslations.fuelConsumption.fuelConsumptionStats.unit
        }
        spaceBetweenTitleAndBody={spaceBetweenTitleAndBody}
      />
      <StatDisplay
        header={
          customerPortalTranslations.fuelConsumption.fuelConsumptionStats
            .renewablesTitle
        }
        middle={formatNumber(consumptionStats.totalConsumptionRenewableFuels)}
        unit={
          customerPortalTranslations.fuelConsumption.fuelConsumptionStats.unit
        }
        spaceBetweenTitleAndBody={spaceBetweenTitleAndBody}
      />
      <Box position="relative">
        <>
          <Typography mb={spaceBetweenTitleAndBody} variant="h5">
            {
              customerPortalTranslations.fuelConsumption.fuelConsumptionStats
                .renewableShareTitle
            }
          </Typography>
          <Box
            position="absolute"
            sx={{ top: "30px", left: "15px" }}
            width="114px"
            height="114px"
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
              <Typography variant="h3">{`${consumptionStats.consumptionTotalForCircle}%`}</Typography>
            </Box>
          </Box>
          <ReactApexChart
            options={optionsConsumption}
            series={optionsConsumption.series}
            type="donut"
            width={140}
            height={140}
          />
        </>
      </Box>
    </Box>
  );
};
