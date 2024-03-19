import { TableComponents } from "react-virtuoso";
import { StockTransactionResponse } from "../../../../../api/api";
import { DataPage } from "../../../../../components/data-page/data-page";
import { TableFrame } from "../../../../../components/table/table-frame";
import { getFixedHeaderAndRowContent } from "../../../../../components/table/virtual-table/header-row-content";
import VirtualTable from "../../../../../components/table/virtual-table/virtual-table";
import { genericVirtualTableComponents } from "../../../../../components/table/virtual-table/virtual-table-components";
import {
  TableContext,
  VirtuosoTableComponentsGroup,
} from "../../../../../components/types";
import { useStockContext } from "../context/stock-tab-context";
import { virtualStockColumns } from "./virtual-stock-columns";

export const outgoingVirtualTableComponents: TableComponents<
  StockTransactionResponse,
  TableContext
> = genericVirtualTableComponents;

export const StockTable = () => {
  const props = useStockContext();

  const hasMore =
    props.queryStateStocks.data?.hasMoreStockTransactions ?? false;

  const fixedHeaderAndRowContent = getFixedHeaderAndRowContent({
    data: props.queryStateStocks.data?.stockTransactions ?? [],
    columns: virtualStockColumns,
    getLabelProps: props.getLabelProps,
  });

  const virtualComponentsForTable: VirtuosoTableComponentsGroup<StockTransactionResponse> =
    {
      virtuosoTableComponents: outgoingVirtualTableComponents,
      fixedHeaderContent: fixedHeaderAndRowContent.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContent.rowContent,
    };

  const volume = props.queryStateStocks.data?.stockTransactions
    .map((tx) => tx.quantity)
    .reduce((accumulated, value) => accumulated + value, 0);

  return (
    <DataPage queryState={props.queryStateStocks}>
      {({ stockTransactions }) => (
        <TableFrame
          volume={volume}
          numberOfEntriesLoaded={stockTransactions.length}
          totalNumberOfEntries={
            props.queryStateStocks.data?.totalAmountOfStockTransactions ?? 0
          }
        >
          <VirtualTable
            data={stockTransactions}
            loadMore={props.loadMore}
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
