import ErrorIcon from "@mui/icons-material/Error";
import { Typography } from "@mui/material";
import { AllocationResponse } from "../../../../../api/api";
import { DeclarationStateChip } from "../../../../../components/status/declaration-upload/declaration-chip";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { theme } from "../../../../../theme/theme";
import { translations } from "../../../../../translations";
import {
  formatNumber,
  formatPercentage,
} from "../../../../../util/formatters/formatters";
import { AllocationRowMenu } from "./allocation-row-menu";

export type AllocationResponseKeys =
  | keyof AllocationResponse
  | "select"
  | "notification"
  | "contextMenu"
  | "state"
  | "customerNumber" //Missing in response
  | "batchId"; //Missing in response

const columnIdentifiers: Record<string, AllocationResponseKeys> = {
  select: "select",
  company: "company",
  country: "country",
  product: "product",
  customerNumber: "customerNumber",
  customerName: "customer",
  batchId: "batchId",
  storage: "storage",
  certificateSystem: "certificationSystem",
  rawMaterial: "rawMaterial",
  countryOfOrigin: "countryOfOrigin",
  ghgReduction: "ghgReduction",
  volume: "volume",
  state: "state",
  contextMenu: "contextMenu",
  notification: "notification",
};

export const virtualColumnsAllocations: Array<
  VirtualTableColumnDef<AllocationResponse>
> = [
  {
    key: columnIdentifiers.company,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
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
      translations.outgoingPageTranslations.allocationTabTranslations
        .countryColumnHeader,
    cell: ({ country }) => <Typography variant="body2">{country}</Typography>,
  },
  {
    key: columnIdentifiers.product,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .productColumnHeader,
    cell: ({ product }) => <Typography variant="body2">{product}</Typography>,
  },
  {
    key: columnIdentifiers.customerNumber,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
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
      translations.outgoingPageTranslations.allocationTabTranslations
        .customerNameColumnHeader,
    cell: ({ customer }) => <Typography variant="body2">{customer}</Typography>,
  },
  {
    key: columnIdentifiers.batchId,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .batchIDColumnHeader,
    cell: (_) => (
      <Typography variant="body2">
        {"-" /* TODO: BKN missing in data*/}
      </Typography>
    ),
  },
  {
    key: columnIdentifiers.storage,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .storageColumnHeader,
    headerProps: { align: "left" },
    cell: ({ storage }) => (
      <Typography variant="body2" align="left">
        {storage}
      </Typography>
    ),
  },
  {
    key: columnIdentifiers.certificateSystem,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .certificateSystemColumnHeader,
    headerProps: { align: "left" },
    cell: ({ certificationSystem }) => (
      <Typography variant="body2">{certificationSystem}</Typography>
    ),
  },
  {
    key: columnIdentifiers.rawMaterial,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .rawMaterialColumnHeader,
    cell: ({ rawMaterial }) => (
      <Typography variant="body2">{rawMaterial}</Typography>
    ),
  },
  {
    key: columnIdentifiers.countryOfOrigin,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .countryOfOriginColumnHeader,
    cell: ({ countryOfOrigin }) => (
      <Typography variant="body2">{countryOfOrigin}</Typography>
    ),
  },
  {
    key: columnIdentifiers.ghgReduction,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .ghgColumnHeader,
    cell: ({ ghgReduction }) => (
      <Typography variant="body2">{formatPercentage(ghgReduction)}</Typography>
    ),
  },
  {
    key: columnIdentifiers.volume,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .volumeColumnHeader,
    cell: ({ volume }) => (
      <Typography variant="body2">{formatNumber(volume)}</Typography>
    ),
  },
  {
    key: columnIdentifiers.state,
    isSortable: false,
    minWidth: "110px",
    width: "110px",
    header:
      translations.incomingTranslations.declarationUploadTranslations
        .stateColumnHeader,
    cell: (_) => <DeclarationStateChip state={"Allocated"} />,
  },
  {
    key: columnIdentifiers.notification,
    isSortable: false,
    minWidth: "60px",
    width: "60px",
    header: <NotificationCell isHeader={true} hasNotification={false} />,
    cell: ({ hasWarnings }) => (
      <NotificationCell isHeader={false} hasNotification={hasWarnings} />
    ),
  },

  {
    key: columnIdentifiers.contextMenu,
    isSortable: false,
    minWidth: "60px",
    width: "60px",
    header: "",
    headerProps: { padding: "none", sortDirection: false },
    cell: (allocation) => <AllocationRowMenu allocation={allocation} />,
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
