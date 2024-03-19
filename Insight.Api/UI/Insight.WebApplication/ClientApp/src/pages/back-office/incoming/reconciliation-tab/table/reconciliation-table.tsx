import { TableComponents } from "react-virtuoso";
import { IncomingDeclarationResponse } from "../../../../../api/api";
import { DataPage } from "../../../../../components/data-page/data-page";
import { TableFrame } from "../../../../../components/table/table-frame";
import { getFixedHeaderAndRowContent } from "../../../../../components/table/virtual-table/header-row-content";
import VirtualTable from "../../../../../components/table/virtual-table/virtual-table";
import { genericVirtualTableComponents } from "../../../../../components/table/virtual-table/virtual-table-components";
import {
  TableContext,
  VirtuosoTableComponentsGroup,
} from "../../../../../components/types";
import { useReconciliationContext } from "../context/reconciliation-context";
import { virtualColumnsReconciled } from "./virtual-columns";

export const reconciliationVirtualTableComponents: TableComponents<
  IncomingDeclarationResponse,
  TableContext
> = genericVirtualTableComponents;

export const ReconciliationTable = () => {
  const reconciliationProps = useReconciliationContext();

  const hasMore =
    reconciliationProps.queryStateReconciliation.data?.hasMoreDeclarations ??
    false;

  const fixedHeaderAndRowContent = getFixedHeaderAndRowContent({
    data:
      reconciliationProps.queryStateReconciliation.data
        ?.incomingDeclarationsByPageAndPageSize ?? [],
    columns: virtualColumnsReconciled,
    getLabelProps: reconciliationProps.getLabelProps,
  });

  const virtualComponentsForTable: VirtuosoTableComponentsGroup<IncomingDeclarationResponse> =
    {
      virtuosoTableComponents: reconciliationVirtualTableComponents,
      fixedHeaderContent: fixedHeaderAndRowContent.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContent.rowContent,
    };

  return (
    <DataPage queryState={reconciliationProps.queryStateReconciliation}>
      {({ incomingDeclarationsByPageAndPageSize }) => (
        <TableFrame
          volume={reconciliationProps.volume}
          numberOfEntriesLoaded={incomingDeclarationsByPageAndPageSize.length}
          totalNumberOfEntries={
            reconciliationProps.queryStateReconciliation.data
              ?.totalAmountOfDeclarations ?? 0
          }
        >
          <VirtualTable
            data={incomingDeclarationsByPageAndPageSize}
            loadMore={reconciliationProps.loadMore}
            virtualTableComponents={virtualComponentsForTable}
            tableContext={{
              hasMore,
            }}
          />
        </TableFrame>
      )}
    </DataPage>
  );
};
