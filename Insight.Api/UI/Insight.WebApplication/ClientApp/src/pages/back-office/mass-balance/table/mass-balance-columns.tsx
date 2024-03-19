import Typography from "@mui/material/Typography";
import { ColumnDef } from "../../../../components/table/grid-table/grid-table";

// TODO - KAH - Remove when real type is available
export type MassBalanceData = {
  fuelType: string;
  data: string;
};

export const massBalanceColumns: Array<ColumnDef<MassBalanceData>> = [
  {
    key: "fuelType",
    header: "Fuel type",
    minMax: "minmax(max-content, 4fr)",
    cell: ({ fuelType }) => <Typography variant="body2">{fuelType}</Typography>,
  },
  {
    key: "data",
    header: "Data",
    minMax: "minmax(max-content, 1fr)",
    cell: ({ data }) => <Typography variant="body2">{data}</Typography>,
  },
];
