import { Box, Stack } from "@mui/material";
import { DataPageCustomerPortal } from "../../../components/data-page/data-page-customer-portal";
import { CustomerPortalFilterBar } from "../customer-portal-filters";
import {
  FuelConsumptionContextProvider,
  useFuelConsumptionContext,
} from "./context/fuel-consumption-context";
import { ConsumptionDevelopmentView } from "./views/consumption-development";
import { ConsumptionPerProductView } from "./views/consumption-per-product";
import { ConsumptionStatsView } from "./views/consumption-stats";
import { ConsumptionTransactionsView } from "./views/transactions/consumption-transactions";

export const FuelConsumptionPage = () => {
  return (
    <FuelConsumptionContextProvider>
      <FuelConsumptionContent />
    </FuelConsumptionContextProvider>
  );
};

const FuelConsumptionContent = () => {
  const fuelConsumptionProps = useFuelConsumptionContext();

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        alignSelf: "center",
        width: { xs: "100%", sm: "60vw" },
        gap: { xs: "20px", sm: "80px" },
      }}
    >
      <Box
        sx={{
          display: "flex",
          flexDirection: { xs: "column", sm: "row" },
          gap: "10px",
          alignItems: "center",
        }}
      >
        <CustomerPortalFilterBar />
      </Box>
      <Stack alignItems="center" mt="40px" mb="160px">
        <DataPageCustomerPortal
          queryState={fuelConsumptionProps.queryStateFuelConsumption}
        >
          {({
            consumptionStats,
            consumptionPerProduct,
            consumptionDevelopment,
          }) => (
            <Box
              sx={{
                display: "flex",
                width: "100%",
                flexDirection: "column",
                gap: { xs: "20px", sm: "120px" },
              }}
            >
              <ConsumptionStatsView consumptionStats={consumptionStats} />

              <ConsumptionPerProductView
                consumptionPerProduct={consumptionPerProduct}
              />

              <ConsumptionDevelopmentView
                consumptionDevelopment={consumptionDevelopment}
              />

              <ConsumptionTransactionsView />
            </Box>
          )}
        </DataPageCustomerPortal>
      </Stack>
    </Box>
  );
};
