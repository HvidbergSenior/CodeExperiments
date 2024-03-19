import type { TableCellProps } from "@mui/material/TableCell";
import { type ReactNode } from "react";
import { TableComponents } from "react-virtuoso";

type SelectableHeader = (
  selectAllRows: () => void,
  selectedRows: string[],
  dataCount: number,
) => JSX.Element;

export type VirtualTableColumnDef<T> = {
  key: string;
  header?: ReactNode | SelectableHeader;
  headerProps?: TableCellProps;
  cell: (
    data: T,
    index: number,
    selectedRows?: string[],
    selectRow?: (id: string) => void,
  ) => ReactNode;
  cellProps?: TableCellProps;
  minWidth: string;
  width: string;
  sticky?: boolean;
  isSortable?: boolean;
};

export type TableConfig<T> = {
  getRowId: (data: T, index: number) => string;
  onRowClicked?: (data: T) => void;
};

// Virtual table types

export type HeaderRowContentGroup<T> = {
  rowContent: (_index: number, row: T) => JSX.Element;
  fixedHeaderContent: () => JSX.Element;
};

export type HeaderRowContentArgs<T> = {
  data: T[];
  getLabelProps?: (key: keyof T) => {
    active: boolean;
    direction: "desc" | "asc";
    onClick: () => void;
  };
  selectAllRows?: () => void;
  selectedRows?: string[];
  columns: VirtualTableColumnDef<T>[];
};

export type VirtuosoTableComponentsGroup<T> = {
  virtuosoTableComponents: TableComponents<T, TableContext>;
  rowContent: (_index: number, row: T) => JSX.Element;
  fixedHeaderContent: () => JSX.Element;
};

export interface TableContext {
  selectedRows?: string[];
  setSelectedRows?: (id: string) => void;
  hasMore: boolean;
  emptyText?: string;
}
