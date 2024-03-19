import ErrorIcon from "@mui/icons-material/Error";
import { Tooltip, Typography } from "@mui/material";
import { IncomingDeclarationState } from "../../../../../api/api";
import { DeclarationStateChip } from "../../../../../components/status/declaration-upload/declaration-chip";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { theme } from "../../../../../theme/theme";
import { translations } from "../../../../../translations";
import { OutgoingFuelTransactionResponseWithWarnings } from "../../../../types";
import { palette } from "../../../../../theme/biofuel/palette";
import { formatNumber } from "../../../../../util/formatters/formatters";

export type OutgoingFuelTransactionResponseWithWarningsKeys =
  | keyof OutgoingFuelTransactionResponseWithWarnings
  | "state"
  | "notification";

const columnIdentifiers: Record<
  string,
  OutgoingFuelTransactionResponseWithWarningsKeys
> = {
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
  notification: "notification",
};

export const virtualColumsOutgoingDialog: Array<
  VirtualTableColumnDef<OutgoingFuelTransactionResponseWithWarnings>
> = [
  {
    key: columnIdentifiers.company,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .companyColumnHeader,
    cell: (_) => <Typography variant="body2">{"-"}</Typography>,
    cellProps: {
      sx: { borderLeft: "1px solid " + theme.palette.Gray4.main },
    },
  },
  {
    key: columnIdentifiers.country,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .countryColumnHeader,
    cell: ({ country }) => <Typography variant="body2">{country}</Typography>,
  },
  {
    key: columnIdentifiers.productName,
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
    key: columnIdentifiers.notification,
    minWidth: "80px",
    width: "80px",
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
    cellProps: {
      sx: { borderRight: "1px solid " + theme.palette.Gray4.main },
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
