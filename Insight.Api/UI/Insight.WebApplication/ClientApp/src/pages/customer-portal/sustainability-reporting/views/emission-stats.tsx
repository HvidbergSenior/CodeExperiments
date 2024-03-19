import { Box, Typography } from "@mui/material";
import { ApexOptions } from "apexcharts";
import ReactApexChart from "react-apexcharts";
import { Emissionsstats } from "../../../../api/api";
import { theme } from "../../../../theme/theme";
import { customerPortalTranslations } from "../../../../translations/pages/customer-portal-translations";
import { formatNumber } from "../../../../util/formatters/formatters";

interface Props {
  emissionsStats: Emissionsstats;
}
export const EmissionStatsView = ({ emissionsStats }: Props) => {
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
    <Box>
      <Box
        sx={{
          display: "flex",
          justifyContent: "center",
          position: "relative",
          height: "290px",
        }}
      >
        <ReactApexChart
          options={optionsSpeedometer}
          series={optionsSpeedometer.series}
          type="radialBar"
          height={400}
        />
        <Box sx={{ position: "absolute", top: "150px", textAlign: "center" }}>
          <Typography
            variant="h2"
            fontSize="50px"
            sx={{ color: (theme) => theme.palette.primary.main, mb: "10px" }}
          >
            {new Intl.NumberFormat("da-DK").format(
              emissionsStats.emissionSavingsForCircle,
            )}
            {"%"}
          </Typography>
          <Typography variant="h6">
            {customerPortalTranslations.sustainability.emissionStats.CO2eLabel}
          </Typography>
        </Box>
      </Box>

      <Box
        sx={{
          display: "flex",
          flexDirection: { xs: "column", sm: "row" },
          justifyContent: "center",
          alignItems: "center",
          textAlign: "center",
          gap: { xs: "20px", sm: "200px" },
          margin: { xs: "-60px 0 30px 0 ", sm: "-80px 0 0 0" },
        }}
      >
        <Box width="160px">
          <Typography mb={"8px"} variant="h6">
            {
              customerPortalTranslations.sustainability.emissionStats
                .emissionsReductionTitle
            }
          </Typography>
          <Typography variant="h2" color={theme.palette.primary.main}>
            {formatNumber(emissionsStats.achievedEmissionReductions)}
          </Typography>
          <Typography variant="subtitle1">
            {
              customerPortalTranslations.sustainability.emissionStats
                .kgCO2eLabel
            }
          </Typography>
        </Box>
        <Box width="160px">
          <Typography mb={"8px"} variant="h6">
            {
              customerPortalTranslations.sustainability.emissionStats
                .netEmissionTitle
            }
          </Typography>
          <Typography variant="h2" color={theme.palette.common.black}>
            {formatNumber(emissionsStats.netEmission)}
          </Typography>
          <Typography variant="subtitle1">
            {
              customerPortalTranslations.sustainability.emissionStats
                .kgCO2eLabel
            }
          </Typography>
        </Box>
      </Box>
    </Box>
  );
};
