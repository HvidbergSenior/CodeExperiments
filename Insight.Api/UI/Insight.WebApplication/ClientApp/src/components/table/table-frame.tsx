import React from "react";
import { theme } from "../../theme/theme";
import { TableSummation } from "./table-summation";

interface Props {
  children: React.ReactNode;
  offset?: number;
  addBordersForDialog?: boolean;
  volume?: number;
  ghgWeightedAvg?: number;
  totalNumberOfEntries: number;
  numberOfEntriesLoaded: number;
  customEntriesText?: string;
}
export const TableFrame = ({
  children,
  offset = 350,
  addBordersForDialog,
  volume,
  ghgWeightedAvg,
  totalNumberOfEntries,
  numberOfEntriesLoaded,
  customEntriesText,
}: Props) => {
  const summationProps = {
    numberOfEntriesLoaded,
    volume,
    ghgWeightedAvg,
    totalNumberOfEntries,
    customEntriesText,
  };
  return (
    <>
      <div
        style={{
          backgroundColor: "transparent",
          overflowX: "auto",
          height: `calc(100vh - ${offset}px)`,
          borderRadius: "10px",
          width: "100%",
          borderBottom: addBordersForDialog
            ? "1px solid " + theme.palette.Gray4.main
            : "",
        }}
      >
        {children}
      </div>
      <TableSummation {...summationProps} />
    </>
  );
};
