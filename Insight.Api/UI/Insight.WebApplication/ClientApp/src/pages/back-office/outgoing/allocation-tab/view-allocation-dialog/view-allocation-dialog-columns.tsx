import { Typography } from "@mui/material";
import { AllocationByIdResponse } from "../../../../../api/api";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { theme } from "../../../../../theme/theme";
import { translations } from "../../../../../translations";

const columnIdentifiers: Record<string, keyof AllocationByIdResponse> = {
  customer: "customer",
  customerNumber: "customerNumber",
  country: "country",
  product: "product",
  volume: "volume",
};

export const virtualAllocatedDialogColumns: Array<
  VirtualTableColumnDef<AllocationByIdResponse>
> = [
  {
    key: columnIdentifiers.customer,
    minWidth: "80px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.allocationTableHeaders.customer,
    cell: ({ customer }) => <Typography variant="body2">{customer}</Typography>,
    cellProps: {
      sx: { borderLeft: "1px solid " + theme.palette.Gray4.main },
    },
  },
  {
    key: columnIdentifiers.customerNumber,
    minWidth: "80px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.allocationTableHeaders.customerNumber,
    cell: ({ customerNumber }) => (
      <Typography variant="body2">{customerNumber}</Typography>
    ),
  },
  {
    key: columnIdentifiers.country,
    minWidth: "80px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.allocationTableHeaders.country,
    cell: ({ country }) => <Typography variant="body2">{country}</Typography>,
  },
  {
    key: columnIdentifiers.product,
    minWidth: "80px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.allocationTableHeaders.product,
    cell: ({ product }) => <Typography variant="body2">{product}</Typography>,
  },
  {
    key: columnIdentifiers.volume,
    minWidth: "80px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.allocationTabTranslations
        .viewAllocationDialog.allocationTableHeaders.volume,
    cell: ({ volume }) => <Typography variant="body2">{volume}</Typography>,
    cellProps: {
      sx: { borderRight: "1px solid " + theme.palette.Gray4.main },
    },
  },
];
