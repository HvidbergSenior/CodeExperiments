import { Checkbox, Typography } from "@mui/material";
import { DeclarationStateChip } from "../../../../../components/status/declaration-upload/declaration-chip";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { translations } from "../../../../../translations";
import { IncomingDeclarationResponse } from "../../../../../api/api";
import { DeclarationRowMenu } from "./declaration-row-menu";
import {
  formatNumber,
  formatPercentage,
} from "../../../../../util/formatters/formatters";

export type IncomingDeclarationResponseKeys =
  | keyof IncomingDeclarationResponse
  | "select"
  | "contextMenu";

const columnIdentifiers: Record<string, IncomingDeclarationResponseKeys> = {
  select: "select",
  posNumber: "posNumber",
  company: "company",
  country: "country",
  placeOfDispatch: "placeOfDispatch",
  dateOfDispatch: "dateOfDispatch",
  product: "product",
  supplier: "supplier",
  rawMaterial: "rawMaterial",
  countryOfOrigin: "countryOfOrigin",
  quantity: "quantity",
  ghgEmissionSaving: "ghgEmissionSaving",
  incomingDeclarationState: "incomingDeclarationState",
  contextMenu: "contextMenu",
  id: "id",
};

export const virtualColumnsDeclarations: Array<
  VirtualTableColumnDef<IncomingDeclarationResponse>
> = [
  {
    key: columnIdentifiers.select,
    minWidth: "80px",
    width: "80px",
    isSortable: false,
    header: (
      selectAllRows: () => void,
      selectedRows: string[],
      dataCount: number,
    ) => (
      <Checkbox
        sx={{ padding: "0px" }}
        checked={selectedRows.length > 0 && selectedRows.length === dataCount}
        indeterminate={
          selectedRows.length > 0 && selectedRows.length !== dataCount
        }
        onChange={() => {
          selectAllRows();
        }}
      />
    ),
    headerProps: {
      onClick: (event) => event.stopPropagation(),
    },
    cell: (data, _, selectedRows, selectRow) => {
      return (
        <Checkbox
          sx={{ padding: "0px" }}
          checked={selectedRows?.includes(data.id) ?? false}
          onChange={() => selectRow?.(data.id)}
        />
      );
    },
  },
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
    key: columnIdentifiers.incomingDeclarationState,
    isSortable: true,
    minWidth: "80px",
    width: "80px",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .stateColumnHeader,
    cell: ({ incomingDeclarationState }) => (
      <DeclarationStateChip state={incomingDeclarationState} />
    ),
  },
  {
    key: columnIdentifiers.contextMenu,
    minWidth: "60px",
    width: "60px",
    isSortable: false,
    header: "",
    headerProps: { padding: "none", sortDirection: false },
    cell: (reconciledDeclaration) => (
      <DeclarationRowMenu incomingDeclarationId={reconciledDeclaration.id} />
    ),
    cellProps: {
      padding: "none",
      onClick: (event) => event.stopPropagation(),
    },
  },
];
