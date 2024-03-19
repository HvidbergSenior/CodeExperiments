import { Box, Stack, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import { GetFuelConsumptionResponse } from "../../../../../api/api";
import { theme } from "../../../../../theme/theme";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";
import { formatNumber } from "../../../../../util/formatters/formatters";
import { PdfContainer } from "../../pdf-container";
import { StatDisplay } from "../../stat-display";
import "../styles.css"; // adjust the path based on your project structure
import { getGeneralFuelColor } from "../summary/overview-consumption";
import {
  spaceBetweenSections,
  spaceBetweenTitleAndBody,
} from "../summary/summary-page";
import { getFuelColor } from "../util";

interface Props {
  pageNumber: number;
  fuelConsumptionData: GetFuelConsumptionResponse;
}

export const FuelConsumption = ({ pageNumber, fuelConsumptionData }: Props) => {
  const { consumptionDevelopment, consumptionPerProduct, consumptionStats } =
    fuelConsumptionData;

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
      show: false,
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
    series: [
      {
        data: consumptionPerProduct.data
          .sort((a, b) => (a.value < b.value ? 1 : -1))
          .map((item) => ({
            x: item.productNameEnumeration,
            y: item.value,
            fillColor: getFuelColor.get(item.productNameEnumeration),
          })),
      },
    ],
  };

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
            enabled: true,
          },
        },
        columnWidth: 20,
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
      height: "1cm",
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
    <PdfContainer
      title={pdfTranslations.fuelConsumptionPageTranslations.pageTitle}
      pageNumber={pageNumber}
    >
      <Stack mt={spaceBetweenSections} display="flex" flexDirection="row">
        <StatDisplay
          header={
            pdfTranslations.fuelConsumptionPageTranslations
              .statDisplayConsumption.title
          }
          middle={formatNumber(consumptionStats.totalConsumptionAllFuels)}
          unit={
            pdfTranslations.fuelConsumptionPageTranslations
              .statDisplayConsumption.unit
          }
        />
        <Box width="3cm" />
        <StatDisplay
          header={
            pdfTranslations.fuelConsumptionPageTranslations
              .statDisplayRenewables.title
          }
          middle={formatNumber(consumptionStats.totalConsumptionRenewableFuels)}
          unit={
            pdfTranslations.fuelConsumptionPageTranslations
              .statDisplayRenewables.unit
          }
        />
        <Box
          position="relative"
          mr="-1.5cm"
          display="flex"
          alignItems="center"
          flexDirection="column"
          width="265px"
        >
          <Typography mb={spaceBetweenTitleAndBody} variant="pdf_h5">
            {pdfTranslations.fuelConsumptionPageTranslations.renewableShare}
          </Typography>

          <Box
            position="absolute"
            sx={{ top: "0.82cm", left: "1.38cm" }}
            width="3cm"
            height="3cm"
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
                textAlign={"center"}
                variant="pdf_h3"
              >{`${consumptionStats.consumptionTotalForCircle}%`}</Typography>
            </Box>
          </Box>
          <ReactApexChart
            options={optionsConsumption}
            series={optionsConsumption.series}
            type="donut"
            width="100%"
            height={140}
          />
        </Box>
      </Stack>
      <Stack mt="0.5cm">
        <Typography mb="-0.25cm" variant="pdf_h5">
          {pdfTranslations.fuelConsumptionPageTranslations.treeMapTitle}
        </Typography>

        <ReactApexChart
          options={treeMapOptions}
          series={treeMapOptions.series}
          type="treemap"
          height={250}
          width="100%"
        />
      </Stack>
      <Stack mt={spaceBetweenSections}>
        <Typography variant="pdf_h5">
          {pdfTranslations.fuelConsumptionPageTranslations.columnChartTitle}
        </Typography>
        <ReactApexChart
          type="bar"
          options={barOptions}
          series={barOptions.series}
          height={300}
        />
      </Stack>
    </PdfContainer>
  );
};
