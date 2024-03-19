import { Typography } from "@mui/material";
import { OutgoingDeclarationResponse } from "../../../../../api/api";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { theme } from "../../../../../theme/theme";
import { translations } from "../../../../../translations";
import {
  formatNumber,
  formatPercentage,
} from "../../../../../util/formatters/formatters";

const columnIdentifiers: Record<keyof OutgoingDeclarationResponse, string> = {
  country: "country",
  product: "product",
  customerNumber: "customerNumber",
  customerName: "customerName",
  ghgReduction: "ghgReduction",
  volumeTotal: "volumeTotal",
  allocationTotal: "allocationTotal",
  fossilFuelComparatorgCO2EqPerMJ: "fossilFuelComparatorgCO2EqPerMJ",
  incomingDeclarationIds: "incomingDeclarationIds",
  outgoingDeclarationId: "outgoingDeclarationId",
};

export const virtualPublishedDialogColumns: Array<
  VirtualTableColumnDef<OutgoingDeclarationResponse>
> = [
  {
    key: columnIdentifiers.customerName,
    minWidth: "80px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .customerNameColumnHeader,
    cell: ({ customerName }) => (
      <Typography variant="body2">{customerName}</Typography>
    ),
    cellProps: {
      sx: { borderLeft: "1px solid " + theme.palette.Gray4.main },
    },
  },
  {
    key: columnIdentifiers.customerNumber,
    minWidth: "80px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .customerNumberColumnHeader,
    cell: ({ customerNumber }) => (
      <Typography variant="body2">{customerNumber}</Typography>
    ),
  },
  {
    key: columnIdentifiers.country,
    minWidth: "80px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .countryColumnHeader,
    cell: ({ country }) => <Typography variant="body2">{country}</Typography>,
  },
  {
    key: columnIdentifiers.product,
    minWidth: "80px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .productColumnHeader,
    cell: ({ product }) => <Typography variant="body2">{product}</Typography>,
  },

  {
    key: columnIdentifiers.volumeTotal,
    minWidth: "80px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .volumeColumnHeader,
    cell: ({ volumeTotal }) => (
      <Typography variant="body2">{formatNumber(volumeTotal)}</Typography>
    ),
  },
  {
    key: columnIdentifiers.ghgReduction,
    minWidth: "80px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.publishedTabTranslations
        .ghgColumnHeader,
    cell: ({ ghgReduction }) => (
      <Typography variant="body2">{formatPercentage(ghgReduction)}</Typography>
    ),
  },
];
