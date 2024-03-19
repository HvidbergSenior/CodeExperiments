import ErrorIcon from "@mui/icons-material/Error";
import { Checkbox, Tooltip, Typography } from "@mui/material";
import {
  IncomingDeclarationState,
  SuggestionResponse,
} from "../../../../../api/api";
import { DeclarationStateChip } from "../../../../../components/status/declaration-upload/declaration-chip";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { theme } from "../../../../../theme/theme";
import { translations } from "../../../../../translations";
import { palette } from "../../../../../theme/biofuel/palette";
import { DeclarationRowMenu } from "../../../incoming/declaration-upload-tab/table/declaration-row-menu";
import {
  formatNumber,
  formatPercentage,
} from "../../../../../util/formatters/formatters";

export type SuggestionResponseKeys =
  | keyof SuggestionResponse
  | "select"
  | "notification"
  | "contextMenu";

const columnIdentifiers: Record<string, SuggestionResponseKeys> = {
  select: "select",
  id: "id",
  company: "company",
  countryOfOrigin: "countryOfOrigin",
  storage: "storage",
  period: "period",
  product: "product",
  supplier: "supplier",
  rawMaterial: "rawMaterial",
  volume: "volume",
  volumeAvailable: "volumeAvailable",
  ghgReduction: "ghgReduction",
  incomingDeclarationState: "incomingDeclarationState",
  notification: "notification",
  contextMenu: "contextMenu",
};

export const virtualAllocationSuggestionDeclarationColumns: Array<
  VirtualTableColumnDef<SuggestionResponse>
> = [
  {
    key: columnIdentifiers.select,
    minWidth: "80px",
    width: "80px",
    header: (
      selectAllRows: () => void,
      selectedRows: string[],
      dataCount: number,
    ) => (
      <Checkbox
        sx={{ padding: "0px" }}
        checked={dataCount > 0 && selectedRows.length === dataCount}
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
          onChange={() => {
            selectRow ? selectRow(data.id) : null;
          }}
        />
      );
    },
    cellProps: {
      sx: { borderLeft: "1px solid " + theme.palette.Gray4.main },
    },
  },
  {
    key: columnIdentifiers.id,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .idColumnHeader,
    headerProps: { align: "left" },
    cell: ({ id }) => <Typography variant="body2">{id}</Typography>,
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
    key: columnIdentifiers.countryOfOrigin,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .countryColumnHeader,
    cell: ({ country }) => <Typography variant="body2">{country}</Typography>,
  },
  {
    key: columnIdentifiers.storage,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .storageColumnHeader,
    cell: ({ storage }) => <Typography variant="body2">{storage}</Typography>,
  },
  {
    key: columnIdentifiers.period,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .periodColumnHeader,
    cell: ({ period }) => <Typography variant="body2">{period}</Typography>,
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
    key: columnIdentifiers.volume,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .volumeColumnHeader,
    cell: ({ volume }) => (
      <Typography variant="body2">{formatNumber(volume)}</Typography>
    ),
  },
  {
    key: columnIdentifiers.volumeAvailable,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .volumeAvailableColumnHeader,
    cell: ({ volumeAvailable }) => (
      <Typography variant="body2">{formatNumber(volumeAvailable)}</Typography>
    ),
  },
  {
    key: columnIdentifiers.ghgReduction,
    minWidth: "50px",
    width: "100%",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .ghgColumnHeader,
    cell: ({ ghgReduction }) => (
      <Typography variant="body2">{formatPercentage(ghgReduction)}</Typography>
    ),
  },
  {
    key: columnIdentifiers.incomingDeclarationState,
    minWidth: "130px",
    width: "130px",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .stateColumnHeader,
    cell: ({ incomingDeclarationState }) => (
      <DeclarationStateChip
        state={incomingDeclarationState as IncomingDeclarationState}
      />
    ),
  },
  {
    key: columnIdentifiers.notification,
    minWidth: "50px",
    width: "50px",
    header: <NotificationCell isHeader={true} hasNotification={false} />,
    cell: ({ hasWarnings, warnings }) => (
      <Tooltip
        title={warnings.map((line) => (
          <Typography p="4px" variant="body2" color={palette?.Gray1?.main}>
            {line}
            <br />
          </Typography>
        ))}
      >
        <div>
          <NotificationCell isHeader={false} hasNotification={hasWarnings} />
        </div>
      </Tooltip>
    ),
  },
  {
    key: columnIdentifiers.contextMenu,
    minWidth: "60px",
    width: "60px",
    header: "",
    headerProps: { padding: "none", sortDirection: false },
    cell: (allocationSuggest) => (
      <DeclarationRowMenu
        incomingDeclarationId={allocationSuggest.id}
        readOnlyView={true}
      />
    ),
    cellProps: {
      padding: "none",
      onClick: (event) => event.stopPropagation(),
    },
  },
];

function NotificationCell({
  isHeader,
  hasNotification,
}: {
  isHeader: Boolean;
  hasNotification: Boolean;
}) {
  if (hasNotification || isHeader) {
    return (
      <ErrorIcon
        fontSize="small"
        htmlColor={
          isHeader ? theme.palette.common.black : theme.palette.Yellow1.main
        }
      ></ErrorIcon>
    );
  } else {
    return <div></div>;
  }
}
