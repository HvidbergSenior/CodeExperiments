import { Box } from "@mui/system";
import { TableComponents } from "react-virtuoso";
import {
  OutgoingFuelTransactionResponse,
  SuggestionResponse,
} from "../../../../../api/api";
import { DataPage } from "../../../../../components/data-page/data-page";
import { OperationDialog } from "../../../../../components/dialogs/operation-dialog";
import { PeriodButton } from "../../../../../components/filter/period-button";
import { TableFrame } from "../../../../../components/table/table-frame";
import { getFixedHeaderAndRowContent } from "../../../../../components/table/virtual-table/header-row-content";
import VirtualTable from "../../../../../components/table/virtual-table/virtual-table";
import { genericVirtualTableComponents } from "../../../../../components/table/virtual-table/virtual-table-components";
import {
  TableContext,
  VirtuosoTableComponentsGroup,
} from "../../../../../components/types";
import { DialogBaseProps } from "../../../../../shared/types";
import { commonTranslations } from "../../../../../translations/common";
import { outgoingTabTranslations } from "../../../../../translations/pages/outgoing-tab-translations";
import { OutgoingFuelTransactionResponseWithWarnings } from "../../../../types";
import { virtualColumsOutgoingDialog } from "../table/virtual-dialog-columns";
import { virtualAllocationSuggestionDeclarationColumns } from "./allocation-suggestion-columns";
import { useManualAllocation } from "./use-manual-allocation";
import { formatDate } from "../../../../../util/formatters/formatters";

interface Props extends DialogBaseProps {
  fuelTransaction: OutgoingFuelTransactionResponse;
  onSubmit: () => Promise<void>;
  filterDates: { fromDate: string; toDate: string };
}

export const outgoingVirtualTableComponents: TableComponents<
  OutgoingFuelTransactionResponseWithWarnings,
  TableContext
> = genericVirtualTableComponents;

const allocationSuggestionVirtualTableComponents: TableComponents<
  SuggestionResponse,
  TableContext
> = genericVirtualTableComponents;

export const ManualAllocationDialog = ({
  fuelTransaction,
  onSubmit,
  filterDates,
  ...props
}: Props) => {
  const useManualAllocationProps = useManualAllocation({
    initialFuelTransaction: fuelTransaction,
    filterDates,
  });

  const hasMore =
    useManualAllocationProps.queryStateAllocationSuggestionDeclarations.data
      ?.hasMoreSuggestions ?? false;

  const handleOnSubmit = async () => {
    const result = await useManualAllocationProps.submitManualAllocation();
    if (result) {
      onSubmit();
    }
  };

  const fixedHeaderAndRowContentFuelTransaction = getFixedHeaderAndRowContent({
    data: [useManualAllocationProps.currentFuelTransaction],
    columns: virtualColumsOutgoingDialog,
  });

  const virtualComponentsForTableFuelTransaction: VirtuosoTableComponentsGroup<OutgoingFuelTransactionResponseWithWarnings> =
    {
      virtuosoTableComponents: outgoingVirtualTableComponents,
      fixedHeaderContent:
        fixedHeaderAndRowContentFuelTransaction.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContentFuelTransaction.rowContent,
    };

  const fixedHeaderAndRowContent = getFixedHeaderAndRowContent({
    data:
      useManualAllocationProps.queryStateAllocationSuggestionDeclarations.data
        ?.suggestions ?? [],
    columns: virtualAllocationSuggestionDeclarationColumns,
    getLabelProps: useManualAllocationProps.getLabelProps,
    selectAllRows: useManualAllocationProps.selectAllRows,
    selectedRows: useManualAllocationProps.selectedRows,
  });

  const virtualComponentsForTable: VirtuosoTableComponentsGroup<SuggestionResponse> =
    {
      virtuosoTableComponents: allocationSuggestionVirtualTableComponents,
      fixedHeaderContent: fixedHeaderAndRowContent.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContent.rowContent,
    };

  return (
    <OperationDialog
      title={outgoingTabTranslations.manualAllocationDialog.title}
      isOpen={props.isOpen}
      onConfirm={() => handleOnSubmit()}
      onClose={props.onClose}
      submitTitle={outgoingTabTranslations.manualAllocationDialog.submitButton}
      cancelTitle={commonTranslations.cancel}
      disableSubmit={useManualAllocationProps.isSubmitDisabled}
      isLoading={useManualAllocationProps.isLoading}
      maxWidth={false}
      scroll="body"
    >
      <Box height="160px">
        <VirtualTable
          data={[useManualAllocationProps.currentFuelTransaction]}
          loadMore={() => {}}
          virtualTableComponents={virtualComponentsForTableFuelTransaction}
          tableContext={{
            hasMore: false,
          }}
        />
      </Box>

      <Box sx={{ m: 8 }} />
      <Box mb={3}>
        <PeriodButton
          dateFrom={useManualAllocationProps.pageArgs.startDate}
          dateTo={useManualAllocationProps.pageArgs.endDate}
          updateFilter={(fromDate, toDate) =>
            useManualAllocationProps.setPageArgs((prevFilter) => ({
              ...prevFilter,
              startDate: formatDate(fromDate),
              endDate: formatDate(toDate),
            }))
          }
        />
      </Box>

      <DataPage
        queryState={
          useManualAllocationProps.queryStateAllocationSuggestionDeclarations
        }
      >
        {({ suggestions }) => (
          <TableFrame
            offset={600}
            addBordersForDialog={true}
            numberOfEntriesLoaded={suggestions.length}
            totalNumberOfEntries={
              useManualAllocationProps
                .queryStateAllocationSuggestionDeclarations.data
                ?.totalAmountOfSuggestions ?? 0
            }
            volume={useManualAllocationProps.volume}
          >
            <VirtualTable
              data={suggestions}
              loadMore={useManualAllocationProps.loadMore}
              virtualTableComponents={virtualComponentsForTable}
              tableContext={{
                hasMore,
                selectedRows: useManualAllocationProps.selectedRows,
                setSelectedRows: useManualAllocationProps.selectRow,
              }}
            />
          </TableFrame>
        )}
      </DataPage>
    </OperationDialog>
  );
};
