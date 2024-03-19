import { Tab, Tabs } from "@mui/material";
import { styled } from "@mui/material/styles";
import React from "react";
import { Link, matchPath, useLocation } from "react-router-dom";
import { theme } from "../../theme/theme";
import { customerPortalTranslations } from "../../translations/pages/customer-portal-translations";
import { StyledTabsProps } from "../types";
import {
  AvailableAccessRights,
  usePermissions,
} from "../../hooks/use-permissions/use-permissions";

export const CustomerPortalTabMenu = () => {
  const { hasAccessTo } = usePermissions();
  const fuelConsumptionValue = "/customer-portal/fuel-consumption";
  const sustainabilityReportingValue =
    "/customer-portal/sustainability-reporting";
  const deselectTabs = location.pathname.endsWith("settings");
  const findRouteMatch = (patterns: readonly string[]) => {
    const { pathname } = useLocation();

    for (let i = 0; i < patterns.length; i += 1) {
      const pattern = patterns[i];
      const possibleMatch = matchPath(pattern, pathname);
      if (possibleMatch !== null) {
        return possibleMatch;
      }
    }

    return null;
  };
const routeMatch = findRouteMatch([
  fuelConsumptionValue,
  sustainabilityReportingValue,
]);
const tabValue = routeMatch?.pattern?.path ?? fuelConsumptionValue;

  const handleTabChange = (_event: React.SyntheticEvent, _newValue: number) => {
    // Do nothing - tabValue is updated by checking the router
  };

  const StyledTabs = styled((props: StyledTabsProps) => (
    <Tabs
      {...props}
      value={deselectTabs ? false : props.value}
      variant="scrollable"
      scrollButtons={false}
      allowScrollButtonsMobile
      TabIndicatorProps={{
        children: <span className="MuiTabs-indicatorSpan" />,
      }}
    />
  ))({
    "& .MuiTabs-indicator": {
      display: "flex",
      justifyContent: "center",
      backgroundColor: "transparent",
    },
    "& .MuiTabs-indicatorSpan": {
      width: "82%",
      backgroundColor: theme.palette.primary.light,
    },
  });

  return (
    <StyledTabs
      value={tabValue}
      onChange={handleTabChange}
      sx={{
        height: "35px",
        minHeight: "0px",
      }}
    >
      {hasAccessTo(AvailableAccessRights.fuelConsumption) && (
        <Tab
          label={customerPortalTranslations.menuItemFuelConsumption}
          value={fuelConsumptionValue}
          to="fuel-consumption"
          component={Link}
        />
      )}
      {hasAccessTo(AvailableAccessRights.sustainabilityReport) && (
        <Tab
          label={customerPortalTranslations.menuItemSustainabilityReporting}
          value={sustainabilityReportingValue}
          to="sustainability-reporting"
          component={Link}
        />
      )}
    </StyledTabs>
  );
};
