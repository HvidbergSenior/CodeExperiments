import { TableComponents } from "react-virtuoso";
import { AllocationResponse } from "../../../../../api/api";
import { DataPage } from "../../../../../components/data-page/data-page";
import { TableFrame } from "../../../../../components/table/table-frame";
import { getFixedHeaderAndRowContent } from "../../../../../components/table/virtual-table/header-row-content";
import VirtualTable from "../../../../../components/table/virtual-table/virtual-table";
import { genericVirtualTableComponents } from "../../../../../components/table/virtual-table/virtual-table-components";
import {
  TableContext,
  VirtuosoTableComponentsGroup,
} from "../../../../../components/types";
import { useAllocationContext } from "../context/allocation-tab-context";
import { virtualColumnsAllocations } from "./virtual-columns";

export const allocationVirtualTableComponents: TableComponents<
  AllocationResponse,
  TableContext
> = genericVirtualTableComponents;

export const AllocationTable = () => {
  const props = useAllocationContext();

  const hasMore = props.queryStateAllocations.data?.hasMoreAllocations ?? false;

  const fixedHeaderAndRowContent = getFixedHeaderAndRowContent({
    data: props.queryStateAllocations.data?.allocations ?? [],
    columns: virtualColumnsAllocations,
    getLabelProps: props.getLabelProps,
  });

  const virtualComponentsForTable: VirtuosoTableComponentsGroup<AllocationResponse> =
    {
      virtuosoTableComponents: allocationVirtualTableComponents,
      fixedHeaderContent: fixedHeaderAndRowContent.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContent.rowContent,
    };

  return (
    <DataPage queryState={props.queryStateAllocations}>
      {({ allocations }) => (
        <TableFrame
          volume={props.volume}
          ghgWeightedAvg={props.ghgWeightedAvg}
          numberOfEntriesLoaded={allocations.length}
          totalNumberOfEntries={
            props.queryStateAllocations.data?.totalAmountOfAllocations ?? 0
          }
        >
          <VirtualTable
            data={allocations}
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
