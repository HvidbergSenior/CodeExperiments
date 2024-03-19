import { Box, Stack, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import { theme } from "../../../../../theme/theme";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";
import "./../styles.css"; // adjust the path based on your project structure
import { Emissionsstats } from "../../../../../api/api";

interface Props {
  emissionsStats: Emissionsstats;
}

export const OverviewEmission = ({ emissionsStats }: Props) => {
  const optionsSpeedometer: ApexOptions = {
    chart: {
      height: 280,
      type: "radialBar",
    },
    series: [emissionsStats.emissionSavingsForCircle],
    colors: [theme.palette.primary.main],
    plotOptions: {
      radialBar: {
        startAngle: -90,
        endAngle: 90,
        track: {
          background: theme.palette.common.black,
          startAngle: -90,
          endAngle: 90,
        },
        dataLabels: {
          name: {
            show: false,
          },
          value: {
            fontSize: "30px",
            show: false,
          },
        },
      },
    },
    fill: {
      type: "gradient",
      gradient: {
        shade: "dark",
        type: "horizontal",
        gradientToColors: [theme.palette.primary.main],
        stops: [0, 100],
      },
    },
    stroke: {
      lineCap: "butt",
    },
  };

  return (
    <Stack position="relative" width="48%">
      <Typography variant="pdf_h5">
        {
          pdfTranslations.summaryPageTranslations.emissionsOverview
            .carbonFootprint
        }
      </Typography>
      <Box
        position="absolute"
        sx={{ top: "1.9cm", left: "2.4cm" }}
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
            {new Intl.NumberFormat("da-DK").format(
              emissionsStats.emissionSavingsForCircle,
            )}
            {"%"}
          </Typography>
          <div style={{ textAlign: "center" }}>
            <Typography variant="pdf_subtitle1">
              {
                pdfTranslations.summaryPageTranslations.emissionsOverview
                  .CO2eLabel
              }
            </Typography>
          </div>
        </Box>
      </Box>
      <ReactApexChart
        options={optionsSpeedometer}
        series={optionsSpeedometer.series}
        type="radialBar"
        height={280}
      />
      <Box
        display="flex"
        flexDirection="row"
        justifyContent="space-between"
        alignItems="center"
        mb="0.3cm"
        mt="-0.2cm"
      >
        <Box
          display="flex"
          flexDirection="column"
          justifyContent="start"
          pr="0.5cm"
        >
          <Typography variant="pdf_body1">
            {
              pdfTranslations.summaryPageTranslations.emissionsOverview
                .emissionsReductionTitle
            }
          </Typography>
        </Box>
        <Box
          display="flex"
          flexDirection="column"
          alignItems="end"
          color={(theme) => theme.palette.primary.main}
        >
          <Typography variant="h4">
            {new Intl.NumberFormat("da-DK").format(
              emissionsStats.achievedEmissionReductions,
            )}
          </Typography>
          <Typography variant="pdf_subtitle1">
            {
              pdfTranslations.summaryPageTranslations.emissionsOverview
                .kgCO2eLabel
            }
          </Typography>
        </Box>
      </Box>
      <Box
        display="flex"
        flexDirection="row"
        justifyContent="space-between"
        alignItems="center"
        mt="0.3cm"
      >
        <Box
          display="flex"
          flexDirection="column"
          justifyContent="start"
          alignItems="start"
        >
          <Typography variant="pdf_body1">
            {
              pdfTranslations.summaryPageTranslations.emissionsOverview
                .netEmissionTitle
            }
          </Typography>
        </Box>
        <Box display="flex" flexDirection="column" alignItems="end">
          <Typography variant="pdf_h4">
            {new Intl.NumberFormat("da-DK").format(emissionsStats.netEmission)}
          </Typography>
          <Typography variant="pdf_subtitle1">
            {
              pdfTranslations.summaryPageTranslations.emissionsOverview
                .kgCO2eLabel
            }
          </Typography>
        </Box>
      </Box>
      <Box mt="2.75cm" display="flex" justifyContent="center">
        <Typography variant="pdf_body3">
          {pdfTranslations.summaryPageTranslations.emissionsOverview.readMore}
        </Typography>
      </Box>
    </Stack>
  );
};
