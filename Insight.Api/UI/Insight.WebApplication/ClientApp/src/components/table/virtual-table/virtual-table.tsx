import { TableVirtuoso } from "react-virtuoso";
import { TableContext, VirtuosoTableComponentsGroup } from "../../types";

interface Props<T> {
  data: T[];
  loadMore: () => void;
  virtualTableComponents: VirtuosoTableComponentsGroup<T>;
  tableContext: TableContext;
}

export default function VirtualTable<T>({
  data,
  loadMore,
  virtualTableComponents,
  tableContext,
}: Props<T>) {
  return (
    <TableVirtuoso
      context={tableContext}
      data={data}
      components={virtualTableComponents.virtuosoTableComponents}
      fixedHeaderContent={virtualTableComponents.fixedHeaderContent}
      itemContent={virtualTableComponents.rowContent}
      endReached={loadMore}
    />
  );
}
