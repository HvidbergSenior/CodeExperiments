import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import { forwardRef, useMemo } from "react";
import type { LinkProps as RouterLinkProps } from "react-router-dom";
import {
  Link as RouterLink,
  useMatch,
  useResolvedPath,
} from "react-router-dom";

interface Props {
  primary: string;
  to: string;
}

export function ListItemLink({ primary, to }: Props) {
  const resolved = useResolvedPath(to);
  const match = useMatch({
    path: resolved.pathname,
    end: resolved.pathname === "/",
  });
  const selected = Boolean(match);

  const renderLink = useMemo(
    () =>
      forwardRef<HTMLAnchorElement, Omit<RouterLinkProps, "to">>(
        function Link(itemProps, ref) {
          return (
            <RouterLink to={to} ref={ref} {...itemProps} role={undefined} />
          );
        },
      ),
    [to],
  );

  return (
    <li>
      <ListItemButton selected={selected} component={renderLink}>
        <ListItemText
          primary={primary}
          sx={{
            textWrap: "nowrap",
            paddingLeft: (theme) => theme.spacing(2),
            ...(selected && {
              "& .MuiTypography-root": {
                fontWeight: 600,
                color: (theme) => theme.palette.primary.main,
              },
            }),
          }}
        />
      </ListItemButton>
    </li>
  );
}
