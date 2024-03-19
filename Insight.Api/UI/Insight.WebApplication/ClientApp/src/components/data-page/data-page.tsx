import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Skeleton from "@mui/material/Skeleton";
import { Fragment } from "react";
import { QueryState } from "../../pages/types";
import { ErrorDisplay } from "../error/error-display";

interface Props<T> {
  queryState: QueryState<T>;
  children: ((data: T) => JSX.Element) | JSX.Element;
}

const rows = ["1", "2", "3", "4", "5"];

export function DataPage<T>({ queryState, ...props }: Props<T>) {
  if (queryState.isLoading) {
    return (
      <Fragment>
        <Grid mt={-6} width="100%" container alignItems="center">
          <Grid item xs>
            <Skeleton height="100px" />
            {rows.map((key) => (
              <Box key={key} display="flex" flexDirection="row">
                <Skeleton height="60px" width="40px" />
                <Box width="10px" />
                <Skeleton height="60px" width="100%" />
              </Box>
            ))}
          </Grid>
        </Grid>
      </Fragment>
    );
  }

  if (queryState.hasError || !queryState.data) {
    return <ErrorDisplay error={queryState.error} />;
  }

  return (
    <Fragment>
      {typeof props.children === "function"
        ? props.children(queryState.data)
        : props.children}
    </Fragment>
  );
}
