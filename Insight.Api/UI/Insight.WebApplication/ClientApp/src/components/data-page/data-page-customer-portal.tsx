import { Stack } from "@mui/material";
import Box from "@mui/material/Box";
import Skeleton from "@mui/material/Skeleton";
import { Fragment } from "react";
import { QueryState } from "../../pages/types";
import { ErrorDisplay } from "../error/error-display";

interface Props<T> {
  queryState: QueryState<T>;
  children: ((data: T) => JSX.Element) | JSX.Element;
}

export function DataPageCustomerPortal<T>({ queryState, ...props }: Props<T>) {
  if (queryState.isLoading) {
    return (
      <Stack sx={{ width: { xs: "100%", sm: "60vw" } }}>
        <Box
          justifyContent="space-between"
          width="100%"
          display="flex"
          flexDirection="row"
        >
          <Skeleton width="30%" height="200px" />
          <Skeleton width="30%" height="200px" />
          <Skeleton width="30%" height="200px" />
        </Box>
        <Skeleton
          sx={{ padding: "0px", m: "0px" }}
          width="100%"
          height="500px"
        />
        <Skeleton width="100%" height="300px" />
        <Skeleton width="100%" height="300px" />
      </Stack>
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
