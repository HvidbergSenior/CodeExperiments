import { Stack, Typography } from "@mui/material";
import { ProductSpecificationItem } from "../../../../../api/api";
import { theme } from "../../../../../theme/theme";
import { pdfTranslations } from "../../../../../translations/pdf/pdf-translations";
import { carbonFootprintPageTranslations } from "../../../../../translations/pdf/carbon-footprint-page-translations";
import { formatNumber } from "../../../../../util/formatters/formatters";
import { spaceBetweenTitleAndBody } from "../summary/summary-page";

interface Props {
  productSpecifications: ProductSpecificationItem[];
}

export const ProductSpecificationTable = ({ productSpecifications }: Props) => {
  const headers: string[] = [
    carbonFootprintPageTranslations.headers.fuelType,
    carbonFootprintPageTranslations.headers.volume,
    carbonFootprintPageTranslations.headers.ghgBaseline,
    carbonFootprintPageTranslations.headers.ghgEmissionSaving,
    carbonFootprintPageTranslations.headers.achievedEmissionReduction,
    carbonFootprintPageTranslations.headers.netEmission,
  ];

  const TableCellStyled = (data: number) => (
    <td
      style={{
        textAlign: "right",
        borderBottom: `1px solid ${theme.palette.primary.main}`,
        padding: "0.2cm",
        fontFamily: "Open Sans",
      }}
    >
      {formatNumber(data, 0)}
    </td>
  );

  const TableCellStyledTotal = (data: number) => (
    <td
      style={{
        textAlign: "right",
        padding: "0.2cm",
        fontWeight: "600",
        fontFamily: "Open Sans",
      }}
    >
      {formatNumber(data, 0)}
    </td>
  );

  return (
    <Stack mt="1cm">
      <Typography mb={spaceBetweenTitleAndBody} variant="pdf_h5">
        {
          pdfTranslations.carbonFootprintPageTranslations
            .productSpecificationTitle
        }
      </Typography>
      <table
        style={{
          border: `1px solid ${theme.palette.primary.main}`,
          borderSpacing: "0px",
        }}
      >
        <tbody>
          <tr>
            {headers.map((item, index) => (
              <th
                key={index}
                style={{
                  textAlign: item === "Fuel type" ? "left" : "right",
                  paddingTop: "0.2cm",
                  paddingBottom: "0.2cm",
                  paddingLeft: "0.2cm",
                  width: "2cm",
                  fontWeight: 600,
                  backgroundColor: theme.palette.primary.main,
                  color: theme.palette.common.white,
                  fontFamily: "Open Sans",
                }}
              >
                {item}
              </th>
            ))}
          </tr>

          {productSpecifications.map((item, index) => {
            if (index === productSpecifications.length - 1) {
              return (
                <tr
                  key={index}
                  style={{
                    height: "0.6cm",
                    backgroundColor: theme.palette.common.white,
                    fontWeight: "600",
                  }}
                >
                  <td
                    style={{
                      padding: "0.2cm",
                    }}
                  >
                    Total
                  </td>
                  {TableCellStyledTotal(item.volume)}
                  {TableCellStyledTotal(item.ghgBaseline)}
                  {TableCellStyledTotal(item.ghgEmissionSaving)}
                  {TableCellStyledTotal(item.achievedEmissionReduction)}
                  {TableCellStyledTotal(item.netEmission)}
                </tr>
              );
            }
            return (
              <tr
                style={{
                  height: "0.6cm",
                  backgroundColor: theme.palette.common.white,
                }}
              >
                <td
                  style={{
                    borderBottom: `1px solid ${theme.palette.primary.main}`,
                    padding: "0.2cm",
                  }}
                >
                  {item.fuelType}
                </td>
                {TableCellStyled(item.volume)}
                {TableCellStyled(item.ghgBaseline)}
                {TableCellStyled(item.ghgEmissionSaving)}
                {TableCellStyled(item.achievedEmissionReduction)}
                {TableCellStyled(item.netEmission)}
              </tr>
            );
          })}
        </tbody>
      </table>
    </Stack>
  );
};
