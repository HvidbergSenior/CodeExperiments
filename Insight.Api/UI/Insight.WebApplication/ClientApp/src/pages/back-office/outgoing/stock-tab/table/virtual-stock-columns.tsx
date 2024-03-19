import { Typography } from "@mui/material";
import { StockTransactionResponse } from "../../../../../api/api";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { translations } from "../../../../../translations";
import { formatNumber } from "../../../../../util/formatters/formatters";

export type StockTransactionResponseType = keyof StockTransactionResponse;

const columnIdentifiers: Record<string, StockTransactionResponseType> = {
  country: "country",
  productName: "productName",
  quantity: "quantity",
  allocatedQuantity: "allocatedQuantity",
  alreadyAllocatedPercentage: "alreadyAllocatedPercentage",
  missingAllocationQuantity: "missingAllocationQuantity",
  companyName: "companyName",
  location: "location",
  productNumber: "productNumber",
  companyId: "companyId",
  locationId: "locationId",
  id: "id",
};

export const virtualStockColumns: Array<
  VirtualTableColumnDef<StockTransactionResponse>
> = [
  {
    key: columnIdentifiers.companyName,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .companyColumnHeader,
    cell: ({ companyName }) => (
      <Typography variant="body2">{companyName}</Typography>
    ),
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
    key: columnIdentifiers.location,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header:
      translations.outgoingPageTranslations.outgoingTabTranslations
        .customerNumberColumnHeader,
    cell: ({ location }) => <Typography variant="body2">{location}</Typography>,
  },
  {
    key: columnIdentifiers.quantity,
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
    key: columnIdentifiers.allocatedQuantity,
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
    key: columnIdentifiers.alreadyAllocatedPercentage,
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
];
