import { TableComponents } from "react-virtuoso";
import { OutgoingDeclarationResponse } from "../../../../../api/api";
import { DataPage } from "../../../../../components/data-page/data-page";
import { TableFrame } from "../../../../../components/table/table-frame";
import { getFixedHeaderAndRowContent } from "../../../../../components/table/virtual-table/header-row-content";
import VirtualTable from "../../../../../components/table/virtual-table/virtual-table";
import { genericVirtualTableComponents } from "../../../../../components/table/virtual-table/virtual-table-components";
import {
  TableContext,
  VirtuosoTableComponentsGroup,
} from "../../../../../components/types";
import { usePublishedContext } from "../context/published-tab-context";
import { virtualColumnsDeclarations } from "./virtual-columns";

export const publishedDeclarationsVirtualTableComponents: TableComponents<
  OutgoingDeclarationResponse,
  TableContext
> = genericVirtualTableComponents;

export const DeclarationsTable = () => {
  const incomingProps = usePublishedContext();
  const hasMore =
    incomingProps.queryStateDeclarations.data?.hasMoreDeclarations ?? false;

  // TODO: KAH - Implement sorting for virtual table

  const fixedHeaderAndRowContent = getFixedHeaderAndRowContent({
    columns: virtualColumnsDeclarations,
    data:
      incomingProps.queryStateDeclarations.data
        ?.outgoingDeclarationsByPageAndPageSizeResponse ?? [],
  });
  const virtualComponentsForTable: VirtuosoTableComponentsGroup<OutgoingDeclarationResponse> =
    {
      virtuosoTableComponents: publishedDeclarationsVirtualTableComponents,
      fixedHeaderContent: fixedHeaderAndRowContent.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContent.rowContent,
    };

  return (
    <DataPage queryState={incomingProps.queryStateDeclarations}>
      {({ outgoingDeclarationsByPageAndPageSizeResponse }) => (
        <TableFrame
          volume={incomingProps.volume}
          ghgWeightedAvg={incomingProps.ghgWeightedAvg}
          numberOfEntriesLoaded={
            outgoingDeclarationsByPageAndPageSizeResponse.length
          }
          totalNumberOfEntries={
            incomingProps.queryStateDeclarations.data
              ?.totalAmountOfDeclarations ?? 0
          }
        >
          <VirtualTable
            data={outgoingDeclarationsByPageAndPageSizeResponse}
            loadMore={incomingProps.loadMore}
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
