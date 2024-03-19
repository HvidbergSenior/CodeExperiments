import ArticleOutlinedIcon from "@mui/icons-material/ArticleOutlined";
import Grid from "@mui/material/Grid";
import Stack from "@mui/material/Stack";
import MuiTable from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell, { TableCellProps } from "@mui/material/TableCell";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Typography from "@mui/material/Typography";
import { ReactNode } from "react";

export type ColumnDef<T> = {
  key: string;
  header?: ReactNode;
  headerProps?: TableCellProps;
  cell: (data: T, index: number) => ReactNode;
  cellProps?: TableCellProps;
  minMax: string;
  sticky?: boolean;
};

type Props<T> = {
  data: T[];
  columns: Array<ColumnDef<T>>;
  emptyText?: string;
};

export function GridTable<T>(props: Props<T>) {
  const hasHeader = props.columns.every(
    (column) => column.header !== undefined,
  );
  const tableRows = props.data.length - (hasHeader ? 0 : 1);
  const gridTemplateRows = `auto repeat(${tableRows}, minmax(78px, 78px))`;

  return (
    <MuiTable
      stickyHeader
      sx={{
        display: "grid",
        gridTemplateRows,
        gridTemplateColumns: props.columns
          .map((column) => column.minMax)
          .join(" "),
        overflowX: "auto",
        maxHeight: "100%",
        overflowY: "auto",
      }}
    >
      {hasHeader && (
        <TableHead sx={{ display: "contents" }}>
          <TableRow sx={{ display: "contents" }}>
            {props.columns.map((column) => (
              <TableCell
                variant="head"
                key={column.key}
                {...column.headerProps}
                sx={{
                  whiteSpace: "nowrap",
                  alignItems: "center",
                  ...column.headerProps?.sx,
                }}
              >
                <Typography variant="datatable">{column.header}</Typography>
              </TableCell>
            ))}
          </TableRow>
        </TableHead>
      )}
      <TableBody sx={{ display: "contents" }}>
        {props.data.map((data, dataIndex) => {
          return (
            <TableRow key={dataIndex} sx={{ display: "contents" }}>
              {props.columns.map((column, columnIndex) => (
                <TableCell
                  key={columnIndex}
                  {...column.cellProps}
                  sx={{
                    background: (theme) => theme.palette.background.paper,
                    display: "flex",
                    alignItems: "center",
                    ...column.cellProps?.sx,
                  }}
                >
                  {column.cell(data, dataIndex)}
                </TableCell>
              ))}
            </TableRow>
          );
        })}
        {props.data.length === 0 && props.emptyText && (
          <TableRow sx={{ display: "contents" }}>
            <TableCell
              sx={{
                gridColumn: `1/${props.columns.length + 1}`,
                backgroundColor: "white",
              }}
            >
              <Grid
                container
                sx={{ p: 8 }}
                direction="column"
                alignItems="center"
                justifyContent="center"
              >
                <Stack alignItems="center">
                  <ArticleOutlinedIcon sx={{ fontSize: "60px" }} />
                  <Typography mt={2} variant="body1">
                    {props.emptyText}
                  </Typography>
                </Stack>
              </Grid>
            </TableCell>
          </TableRow>
        )}
      </TableBody>
    </MuiTable>
  );
}
