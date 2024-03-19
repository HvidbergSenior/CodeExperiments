import { Typography } from "@mui/material";
import {
  AllocationIncomingDeclarationDto,
  IncomingDeclarationState,
} from "../../../../../api/api";
import { DeclarationStateChip } from "../../../../../components/status/declaration-upload/declaration-chip";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { translations } from "../../../../../translations";
import {
  formatNumber,
  formatPercentage,
} from "../../../../../util/formatters/formatters";
import { theme } from "../../../../../theme/theme";

export type AllocationIncomingDeclarationResponseKeys =
  | keyof AllocationIncomingDeclarationDto
  | "select"
  | "contextMenu";

const columnIdentifiers: Record<
  string,
  AllocationIncomingDeclarationResponseKeys
> = {
  company: "company",
  country: "country",
  product: "product",
  supplier: "supplier",
  rawMaterial: "rawMaterial",
  posNumber: "posNumber",
  countryOfOrigin: "countryOfOrigin",
  placeOfDispatch: "placeOfDispatch",
  dateOfDispatch: "dateOfDispatch",
  quantity: "quantity",
  ghgEmissionSaving: "ghgEmissionSaving",
};

export const virtualDeclarationDialogColumns: Array<
  VirtualTableColumnDef<AllocationIncomingDeclarationDto>
> = [
  {
    key: columnIdentifiers.company,
    minWidth: "50px",
    width: "100%",
    isSortable: true,
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.declerationsTableHeaders.company,
    cell: ({ company }) => (
      <Typography variant="body2" align="left">
        {company}
      </Typography>
    ),
    cellProps: {
      sx: { borderLeft: "1px solid " + theme.palette.Gray4.main },
    },
  },
  {
    key: columnIdentifiers.country,
    minWidth: "50px",
    width: "100%",
    isSortable: true,
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.declerationsTableHeaders.country,
    cell: ({ country }) => <Typography variant="body2">{country}</Typography>,
  },
  {
    key: columnIdentifiers.placeOfDispatch,
    minWidth: "50px",
    width: "100%",
    isSortable: true,
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.declerationsTableHeaders.placeOfDispatch,
    cell: ({ placeOfDispatch }) => (
      <Typography variant="body2">{placeOfDispatch}</Typography>
    ),
  },
  {
    key: columnIdentifiers.dateOfDispatch,
    minWidth: "50px",
    width: "100%",
    isSortable: true,
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.declerationsTableHeaders.dateOfDispatch,
    cell: ({ dateOfDispatch }) => (
      <Typography variant="body2">{dateOfDispatch}</Typography>
    ),
  },
  {
    key: columnIdentifiers.product,
    minWidth: "50px",
    width: "100%",
    isSortable: true,
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.declerationsTableHeaders.product,
    cell: ({ product }) => <Typography variant="body2">{product}</Typography>,
  },
  {
    key: columnIdentifiers.supplier,
    minWidth: "50px",
    width: "100%",
    isSortable: true,
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.declerationsTableHeaders.supplier,
    cell: ({ supplier }) => <Typography variant="body2">{supplier}</Typography>,
  },
  {
    key: columnIdentifiers.rawMaterial,
    minWidth: "50px",
    width: "100%",
    isSortable: true,
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.declerationsTableHeaders.rawMaterial,
    headerProps: { align: "left" },
    cell: ({ rawMaterial }) => (
      <Typography variant="body2" align="left">
        {rawMaterial}
      </Typography>
    ),
  },
  {
    key: columnIdentifiers.countryOfOrigin,
    minWidth: "50px",
    width: "100%",
    isSortable: true,
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.declerationsTableHeaders.countryOfOrigin,
    headerProps: { align: "left" },
    cell: ({ countryOfOrigin }) => (
      <Typography variant="body2">{countryOfOrigin}</Typography>
    ),
  },
  {
    key: columnIdentifiers.quantity,
    minWidth: "50px",
    width: "100%",
    isSortable: true,
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.declerationsTableHeaders.quantity,
    cell: ({ quantity }) => (
      <Typography variant="body2">{formatNumber(quantity ?? 0)}</Typography>
    ),
  },
  {
    key: columnIdentifiers.ghgEmissionSaving,
    minWidth: "50px",
    width: "100%",
    isSortable: true,
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.declerationsTableHeaders.ghgEmmisionSaving,
    cell: ({ ghgEmissionSaving }) => (
      <Typography variant="body2">
        {formatPercentage(ghgEmissionSaving ?? 0)}
      </Typography>
    ),
  },
  {
    key: columnIdentifiers.incomingDeclarationState,
    minWidth: "120px",
    width: "120px",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.declerationsTableHeaders.state,
    cell: (_) => (
      <DeclarationStateChip state={"Allocated" as IncomingDeclarationState} />
    ),
    cellProps: {
      sx: { borderRight: "1px solid " + theme.palette.Gray4.main },
    },
  },
];
