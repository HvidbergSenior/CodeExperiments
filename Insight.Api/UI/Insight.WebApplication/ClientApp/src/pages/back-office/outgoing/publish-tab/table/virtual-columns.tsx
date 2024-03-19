import { Typography } from "@mui/material";
import { OutgoingDeclarationResponse } from "../../../../../api/api";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { publishedTabTranslations } from "../../../../../translations/pages/published-tab-translations";
import {
  formatNumber,
  formatPercentage,
} from "../../../../../util/formatters/formatters";
import { PublishedDeclarationRowMenu } from "./published-row-menu";

export type OutgoingDeclarationResponseKeys =
  | keyof OutgoingDeclarationResponse
  | "notification"
  | "contextMenu";

const columnIdentifiers: Record<string, OutgoingDeclarationResponseKeys> = {
  country: "country",
  product: "product",
  customerNumber: "customerNumber",
  customerName: "customerName",
  ghgReduction: "ghgReduction",
  volume: "volumeTotal",
  notification: "notification",
  contextMenu: "contextMenu",
};

export const virtualColumnsDeclarations: Array<
  VirtualTableColumnDef<OutgoingDeclarationResponse>
> = [
  {
    key: columnIdentifiers.customerName,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header: publishedTabTranslations.customerNameColumnHeader,
    cell: ({ customerName }) => (
      <Typography variant="body2">{customerName}</Typography>
    ),
  },
  {
    key: columnIdentifiers.customerNumber,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header: publishedTabTranslations.customerNumberColumnHeader,
    cell: ({ customerNumber }) => (
      <Typography variant="body2">{customerNumber}</Typography>
    ),
  },

  {
    key: columnIdentifiers.country,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header: publishedTabTranslations.countryColumnHeader,
    cell: ({ country }) => <Typography variant="body2">{country}</Typography>,
  },
  {
    key: columnIdentifiers.product,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header: publishedTabTranslations.productColumnHeader,
    cell: ({ product }) => <Typography variant="body2">{product}</Typography>,
  },

  {
    key: columnIdentifiers.volume,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header: publishedTabTranslations.volumeColumnHeader,
    cell: ({ volumeTotal }) => (
      <Typography variant="body2">{formatNumber(volumeTotal)}</Typography>
    ),
  },
  {
    key: columnIdentifiers.ghgReduction,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header: publishedTabTranslations.ghgColumnHeader,
    cell: ({ ghgReduction }) => (
      <Typography variant="body2">{formatPercentage(ghgReduction)}</Typography>
    ),
  },

  {
    key: columnIdentifiers.contextMenu,
    isSortable: false,
    minWidth: "60px",
    width: "60px",
    header: "",
    headerProps: { padding: "none", sortDirection: false },
    cell: (declaration) => (
      <PublishedDeclarationRowMenu declaration={declaration} />
    ),
    cellProps: {
      padding: "none",
      onClick: (event) => event.stopPropagation(),
    },
  },
];
