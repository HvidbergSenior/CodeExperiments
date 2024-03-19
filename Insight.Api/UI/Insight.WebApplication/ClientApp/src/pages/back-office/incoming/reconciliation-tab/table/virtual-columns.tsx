import { Typography } from "@mui/material";
import { IncomingDeclarationResponse } from "../../../../../api/api";
import { DeclarationStateChip } from "../../../../../components/status/declaration-upload/declaration-chip";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { translations } from "../../../../../translations";
import { ReconciliationRowMenu } from "./row-menu-reconciliation";
import {
  formatNumber,
  formatPercentage,
} from "../../../../../util/formatters/formatters";

export type IncomingDeclarationResponseKeys =
  | keyof IncomingDeclarationResponse
  | "select"
  | "state"
  | "contextMenu";

const columnIdentifiers: Record<IncomingDeclarationResponseKeys, string> = {
  select: "select",
  id: "id",
  company: "company",
  country: "country",
  placeOfDispatch: "placeOfDispatch",
  dateOfDispatch: "dateOfDispatch",
  product: "product",
  supplier: "supplier",
  rawMaterial: "rawMaterial",
  countryOfOrigin: "countryOfOrigin",
  quantity: "quantity",
  remainingVolume: "remainingVolume",
  ghgEmissionSaving: "ghgEmissionSaving",
  state: "incomingDeclarationState",
  contextMenu: "contextMenu",
  incomingDeclarationState: "incomingDeclarationState",
  posNumber: "posNumber",
};

export const virtualColumnsReconciled: Array<
  VirtualTableColumnDef<IncomingDeclarationResponse>
> = [
  {
    key: columnIdentifiers.posNumber,
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .idColumnHeader,
    headerProps: { align: "left" },
    cell: ({ posNumber }) => (
      <Typography variant="body2">{posNumber}</Typography>
    ),
    isSortable: true,
    minWidth: "50px",
    width: "100%",
  },
  {
    key: columnIdentifiers.company,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .companyColumnHeader,
    cell: ({ company }) => (
      <Typography variant="body2" align="left">
        {company}
      </Typography>
    ),
  },
  {
    key: columnIdentifiers.country,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .countryColumnHeader,
    cell: ({ country }) => <Typography variant="body2">{country}</Typography>,
  },
  {
    key: columnIdentifiers.placeOfDispatch,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .storageColumnHeader,
    cell: ({ placeOfDispatch }) => (
      <Typography variant="body2">{placeOfDispatch}</Typography>
    ),
  },
  {
    key: columnIdentifiers.dateOfDispatch,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .periodColumnHeader,
    cell: ({ dateOfDispatch }) => (
      <Typography variant="body2">{dateOfDispatch}</Typography>
    ),
  },
  {
    key: columnIdentifiers.product,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .productColumnHeader,
    cell: ({ product }) => <Typography variant="body2">{product}</Typography>,
  },
  {
    key: columnIdentifiers.supplier,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .supplierColumnHeader,
    cell: ({ supplier }) => <Typography variant="body2">{supplier}</Typography>,
  },
  {
    key: columnIdentifiers.rawMaterial,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .rawMaterialColumnHeader,
    headerProps: { align: "left" },
    cell: ({ rawMaterial }) => (
      <Typography variant="body2" align="left">
        {rawMaterial}
      </Typography>
    ),
  },
  {
    key: columnIdentifiers.countryOfOrigin,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .countryOfOriginColumnHeader,
    headerProps: { align: "left" },
    cell: ({ countryOfOrigin }) => (
      <Typography variant="body2">{countryOfOrigin}</Typography>
    ),
  },
  {
    key: columnIdentifiers.quantity,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .volumeColumnHeader,
    cell: ({ quantity }) => (
      <Typography variant="body2">{formatNumber(quantity)}</Typography>
    ),
  },
  {
    key: columnIdentifiers.remainingVolume,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .remainingVolume,
    cell: ({ remainingVolume }) => (
      <Typography variant="body2">{formatNumber(remainingVolume)}</Typography>
    ),
  },
  {
    key: columnIdentifiers.ghgEmissionSaving,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .ghgColumnHeader,
    cell: ({ ghgEmissionSaving }) => (
      <Typography variant="body2">
        {formatPercentage(ghgEmissionSaving)}
      </Typography>
    ),
  },
  {
    key: columnIdentifiers.state,
    isSortable: true,
    minWidth: "110px",
    width: "110px",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .stateColumnHeader,
    cell: ({ incomingDeclarationState }) => (
      <DeclarationStateChip state={incomingDeclarationState} />
    ),
  },
  {
    key: columnIdentifiers.contextMenu,
    isSortable: false,
    minWidth: "60px",
    width: "60px",
    header: "",
    headerProps: { padding: "none", sortDirection: false },
    cell: (data) => <ReconciliationRowMenu reconciledDeclaration={data} />,
    cellProps: {
      padding: "none",
      onClick: (event) => event.stopPropagation(),
    },
  },
];
