import { Box, Stack, Typography } from "@mui/material";
import { useState } from "react";
import { TableComponents } from "react-virtuoso";
import { FuelConsumptionTransaction } from "../../../../../api/api";
import { DataPage } from "../../../../../components/data-page/data-page";
import { LoadingButton } from "../../../../../components/loading-button.tsx/loading-button";
import { getFixedHeaderAndRowContent } from "../../../../../components/table/virtual-table/header-row-content";
import VirtualTable from "../../../../../components/table/virtual-table/virtual-table";
import { genericVirtualTableComponents } from "../../../../../components/table/virtual-table/virtual-table-components";
import {
  TableContext,
  VirtuosoTableComponentsGroup,
} from "../../../../../components/types";
import { useExcelDownload } from "../../../../../hooks/excel-download/use-excel-download";
import { commonTranslations } from "../../../../../translations/common";
import { customerPortalTranslations } from "../../../../../translations/pages/customer-portal-translations";
import { outgoingTabTranslations } from "../../../../../translations/pages/outgoing-tab-translations";
import { useFuelConsumptionContext } from "../../context/fuel-consumption-context";
import { virtualConsumptionTransactionsColumns } from "./table/table-columns";
import { useCustomerPortalContext } from "../../../customer-portal-context";

const spaceBetweenTitleAndBody = "11px";

const transactionsVirtualTableComponents: TableComponents<
  FuelConsumptionTransaction,
  TableContext
> = genericVirtualTableComponents;

export const ConsumptionTransactionsView = () => {
  const { queryStateFuelConsumptionTransactions } = useFuelConsumptionContext();
  const fixedHeaderAndRowContentFuelTransactions = getFixedHeaderAndRowContent({
    data: queryStateFuelConsumptionTransactions.data?.data ?? [],
    columns: virtualConsumptionTransactionsColumns,
  });

  const virtualComponentsForTableFuelTransaction: VirtuosoTableComponentsGroup<FuelConsumptionTransaction> =
    {
      virtuosoTableComponents: transactionsVirtualTableComponents,
      fixedHeaderContent:
        fixedHeaderAndRowContentFuelTransactions.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContentFuelTransactions.rowContent,
    };
  const { filter } = useCustomerPortalContext();

  const [loading, setLoading] = useState(false);
  const { handleDownload } = useExcelDownload({
    fileName: "Biofuel-Express-Latest-Transactions.xlsx",
    setLoading: setLoading,
    filter,
  });

  return (
    <Stack>
      <DataPage queryState={queryStateFuelConsumptionTransactions}>
        {({ data, hasMoreTransactions }) => (
          <>
            <Stack
              direction="row"
              alignItems="center"
              justifyContent="space-between"
              mb={spaceBetweenTitleAndBody}
            >
              <Typography variant="h5">
                {
                  customerPortalTranslations.fuelConsumption
                    .consumptionTransactions.title
                }
              </Typography>
              <LoadingButton
                disabled={
                  queryStateFuelConsumptionTransactions.data?.data.length === 0
                }
                sx={{ minWidth: "150px", height: "36px", textWrap: "nowrap" }}
                loading={loading}
                variant="outlined"
                color="secondary"
                onClick={handleDownload}
              >
                {commonTranslations.exportToExcel}
              </LoadingButton>
            </Stack>
            <Box height="537px">
              <VirtualTable
                data={data}
                virtualTableComponents={
                  virtualComponentsForTableFuelTransaction
                }
                tableContext={{
                  hasMore: hasMoreTransactions,
                  emptyText: outgoingTabTranslations.emptyTextDeclarationTable,
                }}
                loadMore={() => {}}
              />
            </Box>
          </>
        )}
      </DataPage>
    </Stack>
  );
};
