import { Box, BoxProps, Typography } from "@mui/material";
import { customerPortalTranslations } from "../../translations/pages/customer-portal-translations";

interface Props extends BoxProps {}

const EmptyStateBox = (props: Props) => {
  return (
    <Box
      display="flex"
      alignItems="center"
      justifyContent="center"
      border="1px dashed grey "
      borderRadius="10px"
      {...props}
    >
      <Typography variant="body1">
        {customerPortalTranslations.fuelConsumption.fuelConsumptionStats.noData}
      </Typography>
    </Box>
  );
};

export default EmptyStateBox;
