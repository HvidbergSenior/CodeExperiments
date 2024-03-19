import { Stack } from "@mui/material";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { Fragment } from "react";
import { api } from "../../api";
import { errorTranslations } from "../../translations/errors";
import ErrorIcon from "@mui/icons-material/Error";
import { AxiosError } from "axios";
import { isApiError } from "../../util/errors/predicates";
interface Props {
  error: AxiosError | undefined;
  traceId?: string;
}
export const ErrorDisplay = ({ error }: Props) => {
  return (
    <Fragment>
      <Box display="flex" justifyContent="center" width="100%">
        <Box
          display="flex"
          flexDirection="row"
          alignContent="center"
          alignItems="center"
        >
          <Box mr="16px">
            <ErrorIcon fontSize="large" />
          </Box>
          <Box>
            {isApiError(error?.response?.data) && (
              <Stack alignItems="left">
                <Typography mb={2} variant="h3">
                  {`${errorTranslations.anErrorOccured}: ${
                    (error?.response?.data as api.Error).title
                  }`}
                </Typography>
                <Typography variant="h5">
                  {(error?.response?.data as api.Error).detail}
                </Typography>
                <Typography variant="body1">{`TraceId: ${error?.response?.headers["x-trace-id"]}`}</Typography>
              </Stack>
            )}
            {error === undefined && (
              <Typography variant="h3">
                {errorTranslations.anErrorOccured}
              </Typography>
            )}
          </Box>
        </Box>
      </Box>
      <Box sx={{ margin: 2 }} />
    </Fragment>
  );
};
