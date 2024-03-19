import { Typography } from "@mui/material";
import { FuelConsumptionTransaction } from "../../../../../../api/api";
import { VirtualTableColumnDef } from "../../../../../../components/types";
import { translations } from "../../../../../../translations";
import {
  formatDate,
  formatNumber,
} from "../../../../../../util/formatters/formatters";
import { TransactionsResponse } from "../mock-data";

export type TransactionsResponseKeys = keyof TransactionsResponse;

const columnIdentifiers: Record<string, string> = {
  date: "date",
  time: "time",
  accountNumber: "customerNumber",
  accountName: "customerName",
  location: "location",
  productNumber: "productNumber",
  productName: "productName",
  liter: "liter",
};

const tableTranslations =
  translations.customerPortalTranslations.fuelConsumption
    .consumptionTransactions.table;

export const virtualConsumptionTransactionsColumns: Array<
  VirtualTableColumnDef<FuelConsumptionTransaction>
> = [
  {
    key: columnIdentifiers.date,
    minWidth: "80px",
    width: "100%",
    header: tableTranslations.date,
    cell: ({ date }) => (
      <Typography variant="body2">{formatDate(new Date(date))}</Typography>
    ),
  },
  {
    key: columnIdentifiers.time,
    minWidth: "80px",
    width: "100%",
    header: tableTranslations.time,
    cell: ({ time }) => <Typography variant="body2">{time}</Typography>,
  },
  {
    key: columnIdentifiers.accountNumber,
    minWidth: "80px",
    width: "100%",
    header: tableTranslations.accountNumber,
    cell: ({ customerNumber }) => (
      <Typography variant="body2">{customerNumber}</Typography>
    ),
  },
  {
    key: columnIdentifiers.accountName,
    minWidth: "80px",
    width: "100%",
    header: tableTranslations.accountName,
    cell: ({ customerName }) => (
      <Typography variant="body2">{customerName}</Typography>
    ),
  },
  {
    key: columnIdentifiers.productNumber,
    minWidth: "80px",
    width: "100%",
    header: tableTranslations.productNumber,
    cell: ({ productNumber }) => (
      <Typography variant="body2">{productNumber}</Typography>
    ),
  },
  {
    key: columnIdentifiers.productName,
    minWidth: "80px",
    width: "100%",
    header: tableTranslations.productName,
    cell: ({ productName }) => (
      <Typography variant="body2">{productName}</Typography>
    ),
  },
  {
    key: columnIdentifiers.location,
    minWidth: "80px",
    width: "100%",
    header: tableTranslations.location,
    cell: ({ location }) => <Typography variant="body2">{location}</Typography>,
  },
  {
    key: columnIdentifiers.liter,
    minWidth: "80px",
    width: "100%",
    header: tableTranslations.liter,
    headerProps: { align: "right" },
    cell: ({ quantity }) => (
      <Typography textAlign="right" variant="body2">
        {formatNumber(quantity, 0)}
      </Typography>
    ),
    cellProps: { align: "right" },
  },
];
