import { Typography } from "@mui/material";
import {
  IncomingDeclarationState,
  OutgoingFuelTransactionResponse,
} from "../../../../../api/api";
import { DeclarationStateChip } from "../../../../../components/status/declaration-upload/declaration-chip";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { translations } from "../../../../../translations";
import { OutgoingRowMenu } from "./outgoing-row-menu";
import { formatNumber } from "../../../../../util/formatters/formatters";

export type OutgoingFuelTransactionResponseKeys =
  | keyof OutgoingFuelTransactionResponse
  | "state"
  | "contextMenu";

const columnIdentifiers: Record<string, OutgoingFuelTransactionResponseKeys> = {
  company: "customerId",
  country: "country",
  productName: "productName",
  customerNumber: "customerNumber",
  customerName: "customerName",
  customerType: "customerType",
  segment: "customerSegment",
  volume: "quantity",
  allocation: "allocatedQuantity",
  allocationPercent: "alreadyAllocatedPercentage",
  missingAllocationQuantity: "missingAllocationQuantity",
  state: "state",
};

export const virtualColumnsOutgoing: Array<
  VirtualTableColumnDef<OutgoingFuelTransactionResponse>
> = [
  {
    key: columnIdentifiers.company,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .companyColumnHeader,
    cell: (_) => <Typography variant="body2">{"-"}</Typography>,
  },
  {
    key: columnIdentifiers.country,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .countryColumnHeader,
    cell: ({ country }) => <Typography variant="body2">{country}</Typography>,
  },
  {
    key: columnIdentifiers.productName,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .productColumnHeader,
    cell: ({ productName }) => (
      <Typography variant="body2">{productName}</Typography>
    ),
  },
  {
    key: columnIdentifiers.customerNumber,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .customerNumberColumnHeader,
    cell: ({ customerNumber }) => (
      <Typography variant="body2">{customerNumber}</Typography>
    ),
  },
  {
    key: columnIdentifiers.customerName,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .customerNameColumnHeader,
    cell: ({ customerName }) => (
      <Typography variant="body2">{customerName}</Typography>
    ),
  },
  {
    key: columnIdentifiers.customerType,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .customerTypeColumnHeader,
    cell: ({ customerType }) => (
      <Typography variant="body2">{customerType}</Typography>
    ),
  },
  {
    key: columnIdentifiers.segment,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .segmentColumnHeader,
    cell: ({ customerSegment }) => (
      <Typography variant="body2">{customerSegment}</Typography>
    ),
  },
  {
    key: columnIdentifiers.volume,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .volumeColumnHeader,
    cell: ({ quantity }) => (
      <Typography variant="body2">{formatNumber(quantity)}</Typography>
    ),
  },
  {
    key: columnIdentifiers.allocation,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .allocationColumnHeader,
    cell: ({ allocatedQuantity }) => (
      <Typography variant="body2">{formatNumber(allocatedQuantity)}</Typography>
    ),
  },
  {
    key: columnIdentifiers.allocationPercent,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .allocationPercentColumnHeader,
    cell: ({ alreadyAllocatedPercentage }) => (
      <Typography variant="body2">
        {formatNumber(alreadyAllocatedPercentage)}%
      </Typography>
    ),
  },
  {
    key: columnIdentifiers.missingAllocationQuantity,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .missingAllocationColumnHeader,
    cell: ({ missingAllocationQuantity }) => (
      <Typography variant="body2">
        {formatNumber(missingAllocationQuantity)}
      </Typography>
    ),
  },
  {
    key: columnIdentifiers.state,
    isSortable: false,
    minWidth: "110px",
    width: "110px",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .stateColumnHeader,
    cell: (_) => (
      <DeclarationStateChip state={"Registered" as IncomingDeclarationState} />
    ),
  },
  {
    key: columnIdentifiers.contextMenu,
    isSortable: false,
    minWidth: "60px",
    width: "60px",
    header: "",
    headerProps: { padding: "none", sortDirection: false },
    cell: (fuelTransaction) => (
      <OutgoingRowMenu fuelTransaction={fuelTransaction} />
    ),
    cellProps: {
      padding: "none",
      onClick: (event) => event.stopPropagation(),
    },
  },
];
