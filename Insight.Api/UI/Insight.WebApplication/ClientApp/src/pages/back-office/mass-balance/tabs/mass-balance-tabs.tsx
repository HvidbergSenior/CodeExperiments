import * as React from "react";
import Tabs from "@mui/material/Tabs";
import Tab from "@mui/material/Tab";
import Box from "@mui/material/Box";
import {
  BiofuelExpressCompanies,
  biofuelExpressCompanies,
  useMassBalanceContext,
} from "../context/mass-balance-context";
import useDeepCompare from "../../../../hooks/use-deep-compare/use-deep-compare";

export const MassBalanceTabs = () => {
  const { setSelectedCompany } = useMassBalanceContext();
  const [tabValue, setTabValue] =
    React.useState<BiofuelExpressCompanies>("Biofuel Express AB");

  useDeepCompare(() => {
    setSelectedCompany(tabValue);
  }, [tabValue]);

  const handleChange = (
    _: React.SyntheticEvent,
    newValue: BiofuelExpressCompanies,
  ) => {
    setTabValue(newValue);
  };

  return (
    <Box pl={4} sx={{ width: "100%" }}>
      <Tabs
        value={tabValue}
        onChange={handleChange}
        aria-label="basic tabs example"
      >
        {biofuelExpressCompanies.map((companyName) => (
          <Tab
            sx={{
              color: (theme) => theme.palette.primary.main,
              "&:hover": {
                color: (theme) => theme.palette.primary.main,
                backgroundColor: (theme) => theme.palette.primary.contrastText,
                opacity: 1,
              },
              "&.Mui-selected": {
                color: (theme) => theme.palette.primary.main,
              },
              "&.Mui-focusVisible": {
                backgroundColor: "#d1eaff",
              },
            }}
            label={companyName}
            value={companyName}
          />
        ))}
      </Tabs>
    </Box>
  );
};
