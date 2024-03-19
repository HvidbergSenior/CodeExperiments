import { TableSortLabel, Typography } from "@mui/material";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import React from "react";
import { HeaderRowContentArgs, HeaderRowContentGroup } from "../../types";

export const getFixedHeaderAndRowContent = <T,>(
  headerRowContentArgs: HeaderRowContentArgs<T>,
): HeaderRowContentGroup<T> => {
  const fixedHeaderContent = () => {
    return (
      <TableRow sx={{ display: "flex", width: "100%" }}>
        {headerRowContentArgs.columns.map((column) => (
          <TableCell
            key={column.key}
            variant="head"
            style={{ minWidth: column.minWidth, width: column.width }}
            sx={{
              backgroundColor: "background.paper",
              alignItems: "center",
            }}
            {...column.headerProps}
          >
            {typeof column.header === "function" ? (
              column.header(
                headerRowContentArgs.selectAllRows ?? (() => {}),
                headerRowContentArgs.selectedRows ?? [],
                headerRowContentArgs.data.length ?? 0,
              )
            ) : column.isSortable && headerRowContentArgs.getLabelProps ? (
              <TableSortLabel
                {...headerRowContentArgs.getLabelProps(column.key as keyof T)}
              >
                <Typography variant="datatable">{column.header}</Typography>
              </TableSortLabel>
            ) : (
              <Typography variant="datatable">{column.header}</Typography>
            )}
          </TableCell>
        ))}
      </TableRow>
    );
  };

  const rowContent = (dataIndex: number, data: T) => {
    return (
      <React.Fragment>
        {headerRowContentArgs.columns.map((column) => (
          <TableCell
            key={column.key}
            sx={{
              background: (theme) => theme.palette.common.white,
              minWidth: column.minWidth,
              width: column.width,
              display: "flex",
              alignItems: "center",
              position: column.sticky ? "sticky" : undefined,
              right: column.sticky ? "0px" : undefined,
              ...column.cellProps?.sx,
              overflow: "hidden",
              textOverflow: "ellipsis",
            }}
            onClick={column.cellProps?.onClick}
            {...column.cellProps}
          >
            {typeof column.header === "function"
              ? column.cell(data, dataIndex, headerRowContentArgs.selectedRows)
              : column.cell(data, dataIndex)}
          </TableCell>
        ))}
      </React.Fragment>
    );
  };

  return { rowContent, fixedHeaderContent };
};
