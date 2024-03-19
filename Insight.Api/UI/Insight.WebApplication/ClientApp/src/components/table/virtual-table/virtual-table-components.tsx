import ArticleOutlinedIcon from "@mui/icons-material/ArticleOutlined";
import { Stack, Typography, Grid } from "@mui/material";
import CircularProgress from "@mui/material/CircularProgress";
import Paper from "@mui/material/Paper";
import MuiTable from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import React from "react";
import { declarationUploadTranslations } from "../../../translations/pages/declaration-upload-translations";

export const genericVirtualTableComponents = {
  Scroller: React.forwardRef<HTMLDivElement>((props, ref) => (
    <TableContainer component={Paper} {...props} ref={ref} />
  )),
  Table: (props: any) => (
    <MuiTable
      {...props}
      stickyHeader
      sx={{
        overflowX: "auto",
        overflowY: "auto",
      }}
    />
  ),
  TableHead,
  TableRow: ({ item: _item, ...props }: any) => (
    <TableRow
      sx={{
        display: "flex",
      }}
      {...(props.context?.setSelectedRows
        ? {
            selected: props.context?.selectedRows.includes(_item.id),
            onClick: () => props.context?.setSelectedRows(_item.id),
          }
        : {})}
      {...props}
    />
  ),
  TableBody: React.forwardRef<HTMLTableSectionElement>((props, ref) => (
    <TableBody {...props} ref={ref} />
  )),
  TableFoot: () => (
    <div
      style={{
        padding: "2rem",
        height: "400px",

        display: "flex",
        justifyContent: "center",
      }}
    >
      <CircularProgress />
    </div>
  ),
  EmptyPlaceholder: ({ ...props }: any) => (
    <TableBody>
      <TableRow sx={{ display: "flex" }}>
        <TableCell sx={{ width: "100%", border: "0px solid transparent" }}>
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
                {props.context?.emptyText ??
                  declarationUploadTranslations.emptyTextDeclarationTable}
              </Typography>
            </Stack>
          </Grid>
        </TableCell>
      </TableRow>
    </TableBody>
  ),
};
