import { TableComponents } from "react-virtuoso";
import { IncomingDeclarationResponse } from "../../../../../api/api";
import { DataPage } from "../../../../../components/data-page/data-page";
import { TableFrame } from "../../../../../components/table/table-frame";
import { getFixedHeaderAndRowContent } from "../../../../../components/table/virtual-table/header-row-content";
import VirtualTable from "../../../../../components/table/virtual-table/virtual-table";
import {
  TableContext,
  VirtuosoTableComponentsGroup,
} from "../../../../../components/types";
import { useDeclarationUploadContext } from "../context/declaration-upload-context";
import { virtualColumnsDeclarations } from "./virtual-columns";
import { genericVirtualTableComponents } from "../../../../../components/table/virtual-table/virtual-table-components";

export const declarationVirtualTableComponents: TableComponents<
  IncomingDeclarationResponse,
  TableContext
> = genericVirtualTableComponents;

export const DeclarationsTable = () => {
  const incomingProps = useDeclarationUploadContext();
  const hasMore =
    incomingProps.queryStateDeclarations.data?.hasMoreDeclarations ?? false;

  const fixedHeaderAndRowContent = getFixedHeaderAndRowContent({
    data:
      incomingProps.queryStateDeclarations.data
        ?.incomingDeclarationsByPageAndPageSize ?? [],
    columns: virtualColumnsDeclarations,
    getLabelProps: incomingProps.getLabelProps,
    selectAllRows: incomingProps.selectAllRows,
    selectedRows: incomingProps.selectedRows,
  });

  const virtualComponentsForTable: VirtuosoTableComponentsGroup<IncomingDeclarationResponse> =
    {
      virtuosoTableComponents: declarationVirtualTableComponents,
      fixedHeaderContent: fixedHeaderAndRowContent.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContent.rowContent,
    };

  return (
    <DataPage queryState={incomingProps.queryStateDeclarations}>
      {({ incomingDeclarationsByPageAndPageSize }) => (
        <TableFrame
          volume={incomingProps.volume}
          numberOfEntriesLoaded={incomingDeclarationsByPageAndPageSize.length}
          totalNumberOfEntries={
            incomingProps.queryStateDeclarations.data
              ?.totalAmountOfDeclarations ?? 0
          }
        >
          <VirtualTable
            data={incomingDeclarationsByPageAndPageSize}
            loadMore={incomingProps.loadMore}
            virtualTableComponents={virtualComponentsForTable}
            tableContext={{
              hasMore,
              selectedRows: incomingProps.selectedRows,
              setSelectedRows: incomingProps.selectRow,
            }}
          />
        </TableFrame>
      )}
    </DataPage>
  );
};
