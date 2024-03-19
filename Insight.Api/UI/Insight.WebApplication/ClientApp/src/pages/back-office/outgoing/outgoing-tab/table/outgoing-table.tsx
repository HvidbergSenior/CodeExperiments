import { TableComponents } from "react-virtuoso";
import { OutgoingFuelTransactionResponse } from "../../../../../api/api";
import { DataPage } from "../../../../../components/data-page/data-page";
import { TableFrame } from "../../../../../components/table/table-frame";
import { getFixedHeaderAndRowContent } from "../../../../../components/table/virtual-table/header-row-content";
import VirtualTable from "../../../../../components/table/virtual-table/virtual-table";
import { genericVirtualTableComponents } from "../../../../../components/table/virtual-table/virtual-table-components";
import {
  TableContext,
  VirtuosoTableComponentsGroup,
} from "../../../../../components/types";
import { useOutgoingContext } from "../context/outgoing-tab-context";
import { virtualColumnsOutgoing } from "./virtual-columns";
import { outgoingPageTranslations } from "../../../../../translations/pages/outgoing-page-translations";

export const outgoingVirtualTableComponents: TableComponents<
  OutgoingFuelTransactionResponse,
  TableContext
> = genericVirtualTableComponents;

export const OutgoingTable = () => {
  const props = useOutgoingContext();

  const hasMore =
    props.queryStateFuelTransactions.data?.hasMoreOutgoingFuelTransactions ??
    false;

  const fixedHeaderAndRowContent = getFixedHeaderAndRowContent({
    data: props.queryStateFuelTransactions.data?.outgoingFuelTransactions ?? [],
    columns: virtualColumnsOutgoing,
    getLabelProps: props.getLabelProps,
  });

  const virtualComponentsForTable: VirtuosoTableComponentsGroup<OutgoingFuelTransactionResponse> =
    {
      virtuosoTableComponents: outgoingVirtualTableComponents,
      fixedHeaderContent: fixedHeaderAndRowContent.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContent.rowContent,
    };

  return (
    <DataPage queryState={props.queryStateFuelTransactions}>
      {({ outgoingFuelTransactions }) => (
        <TableFrame
          volume={props.queryStateFuelTransactions.data?.totalQuantity ?? 0}
          numberOfEntriesLoaded={outgoingFuelTransactions.length}
          totalNumberOfEntries={
            props.queryStateFuelTransactions.data
              ?.totalAmountOfOutgoingFuelTransactions ?? 0
          }
        >
          <VirtualTable
            data={outgoingFuelTransactions}
            loadMore={props.loadMore}
            virtualTableComponents={virtualComponentsForTable}
            tableContext={{
              hasMore,
              emptyText:
                outgoingPageTranslations.outgoingTabTranslations
                  .emptyTextDeclarationTable,
            }}
          />
        </TableFrame>
      )}
    </DataPage>
  );
};
