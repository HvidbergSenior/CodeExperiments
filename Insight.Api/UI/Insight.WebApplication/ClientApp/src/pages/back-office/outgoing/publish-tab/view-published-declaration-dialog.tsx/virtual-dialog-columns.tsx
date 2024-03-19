import { Typography } from "@mui/material";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { translations } from "../../../../../translations";
import {
  formatNumber,
  formatPercentage,
} from "../../../../../util/formatters/formatters";
import { GetOutgoingDeclarationIncomingDeclarationResponse } from "../../../../../api/api";
import { theme } from "../../../../../theme/theme";

// below are  keyof GetIncomingDeclarationDto
const columnIdentifiers: Record<
  keyof GetOutgoingDeclarationIncomingDeclarationResponse,
  string
> = {
  company: "company",
  country: "country",
  countryOfOrigin: "countryOfOrigin",
  dateOfDispatch: "dateOfDispatch",
  ghgEmissionSaving: "ghgEmissionSaving",
  placeOfDispatch: "placeOfDispatch",
  posNumber: "posNumber",
  product: "product",
  quantity: "quantity",
  rawMaterial: "rawMaterial",
  supplier: "supplier",
  batchId: "batchId",
  incomingDeclarationId: "incomingDeclarationId",
};

export const virtualPublishedDialogIncomingDeclarationColumns: Array<
  VirtualTableColumnDef<GetOutgoingDeclarationIncomingDeclarationResponse>
> = [
  {
    key: columnIdentifiers.posNumber,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .idColumnHeader,
    headerProps: { align: "left" },
    cell: ({ posNumber }) => (
      <Typography variant="body2">{posNumber}</Typography>
    ),
    cellProps: {
      sx: { borderLeft: "1px solid " + theme.palette.Gray4.main },
    },
  },

  {
    key: columnIdentifiers.company,
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
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .countryColumnHeader,
    cell: ({ country }) => <Typography variant="body2">{country}</Typography>,
  },

  {
    key: columnIdentifiers.placeOfDispatch,
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
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .productColumnHeader,
    cell: ({ product }) => <Typography variant="body2">{product}</Typography>,
  },
  {
    key: columnIdentifiers.supplier,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .supplierColumnHeader,
    cell: ({ supplier }) => <Typography variant="body2">{supplier}</Typography>,
  },
  {
    key: columnIdentifiers.rawMaterial,
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
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .volumeColumnHeader,
    cell: ({ quantity }) => (
      <Typography variant="body2">{formatNumber(quantity ?? 0)}</Typography>
    ),
  },

  {
    key: columnIdentifiers.ghgEmissionSaving,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .ghgColumnHeader,
    cell: ({ ghgEmissionSaving }) => (
      <Typography variant="body2">
        {formatPercentage(ghgEmissionSaving ?? 0)}
      </Typography>
    ),
  },
  {
    key: columnIdentifiers.batchId,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .batchIdColumnHeader,
    headerProps: { align: "left" },
    cell: ({ batchId }) => <Typography variant="body2">{batchId}</Typography>,
    cellProps: {
      sx: { borderRight: "1px solid " + theme.palette.Gray4.main },
    },
  },
];
