import ErrorIcon from "@mui/icons-material/Error";
import { Box, Stack, Tooltip, Typography } from "@mui/material";
import { UploadStateChip } from "../../../../../components/status/declaration-upload/upload-state-chip";
import { VirtualTableColumnDef } from "../../../../../components/types";
import { theme } from "../../../../../theme/theme";
import { translations } from "../../../../../translations";
import { UploadResult } from "./use-upload";

export type UploadResultKeys = keyof UploadResult;

const columnIdentifiers: Record<string, UploadResultKeys> = {
  fileName: "fileName",
  errors: "errors",
  state: "state",
};

export const getUploadDialogVirtualColumns = (errorCount: number) => {
  const columns: Array<VirtualTableColumnDef<UploadResult>> = [
    {
      key: columnIdentifiers.fileName,
      minWidth: "200px",
      width: "30%",
      header:
        translations.incomingTranslations.declarationUploadTranslations
          .uploadDeclarations.declarationIdColumnHeader,
      headerProps: { sortDirection: false },
      cell: ({ fileName }) => (
        <Typography variant="body2">{fileName}</Typography>
      ),
      cellProps: {
        sx: { borderLeft: "1px solid " + theme.palette.Gray4.main },
      },
    },
    {
      key: columnIdentifiers.errors,
      minWidth: "50px",
      width: "100%",
      header: "",
      headerProps: { sortDirection: false },
      cell: ({ errors }) => (
        <Tooltip
          componentsProps={{
            tooltip: {
              sx: {
                backgroundColor: "transparent",
              },
            },
          }}
          title={
            <Box
              sx={{
                width: "max-content",
                backgroundColor: (theme) => theme.palette.common.white,
                padding: "20px",
                borderRadius: "10px",
                border: "1px solid black",
              }}
            >
              <Typography
                p="4px"
                color={theme.palette.primary.main}
                variant="h5"
              >
                Errors in uploaded file
                <br />
              </Typography>
              {errors.map((line) => (
                <Typography p="4px" variant="body2">
                  {line}
                  <br />
                </Typography>
              ))}
            </Box>
          }
        >
          <Box
            sx={{
              display: "inline-block",
              width: "300px",
              whiteSpace: "nowrap",
              overflow: "hidden !important",
              textOverflow: "ellipsis",
            }}
          >
            {errors.length > 0 ? errors[0] : ""}
          </Box>
        </Tooltip>
      ),
    },
    {
      key: columnIdentifiers.state,
      minWidth: "200px",
      width: "20%",
      header: (
        <Stack direction="row" spacing={4}>
          <Typography variant="datatable">
            {
              translations.incomingTranslations.declarationUploadTranslations
                .uploadDeclarations.uploadStatusColumnHeader
            }
          </Typography>
          {errorCount > 0 && (
            <Stack direction="row" alignItems="center" spacing={1}>
              <ErrorIcon
                htmlColor={theme.palette.Red1.main}
                sx={{ fontSize: "15px" }}
              />
              <Typography variant="body2" textAlign="center">
                {errorCount}
              </Typography>
            </Stack>
          )}
        </Stack>
      ),
      headerProps: { sortDirection: false },
      cell: ({ state }) => <UploadStateChip state={state} />,
      cellProps: {
        sx: { borderRight: "1px solid " + theme.palette.Gray4.main },
      },
    },
  ];
  return columns;
};
