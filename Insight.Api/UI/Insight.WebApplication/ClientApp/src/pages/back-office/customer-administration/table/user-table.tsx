import { TableComponents } from "react-virtuoso";
import { DataPage } from "../../../../components/data-page/data-page";
import { TableFrame } from "../../../../components/table/table-frame";
import { getFixedHeaderAndRowContent } from "../../../../components/table/virtual-table/header-row-content";
import VirtualTable from "../../../../components/table/virtual-table/virtual-table";
import {
  TableContext,
  VirtuosoTableComponentsGroup,
} from "../../../../components/types";
import { genericVirtualTableComponents } from "../../../../components/table/virtual-table/virtual-table-components";
import { userTableColumns } from "./user-table-columns";
import { useCustomerAdminContext } from "../context/customer-admin-context";
import { AllUserAdminResponse } from "../../../../api/api";
import { customerAdminPageTranslations } from "../../../../translations/pages/customer-admin-page-translations";

export const userVirtualTableComponents: TableComponents<
  AllUserAdminResponse,
  TableContext
> = genericVirtualTableComponents;

export const UserTable = () => {
  const props = useCustomerAdminContext();
  const hasMore = props.queryStateUsers.data?.hasMoreUsers ?? false;

  const fixedHeaderAndRowContent = getFixedHeaderAndRowContent({
    data: props.queryStateUsers.data?.users ?? [],
    columns: userTableColumns,
    getLabelProps: props.getLabelProps,
  });

  const virtualComponentsForTable: VirtuosoTableComponentsGroup<AllUserAdminResponse> =
    {
      virtuosoTableComponents: userVirtualTableComponents,
      fixedHeaderContent: fixedHeaderAndRowContent.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContent.rowContent,
    };

  const numberOfEntriesLoaded = new Set(
    props.queryStateUsers.data?.users.map((user) => user.userName),
  ).size;

  return (
    <DataPage queryState={props.queryStateUsers}>
      {({ users }) => (
        <TableFrame
          numberOfEntriesLoaded={numberOfEntriesLoaded}
          totalNumberOfEntries={
            props.queryStateUsers.data?.totalAmountOfUsers ?? 0
          }
          customEntriesText={customerAdminPageTranslations.usersLoaded}
        >
          <VirtualTable
            data={users}
            loadMore={props.loadMore}
            virtualTableComponents={virtualComponentsForTable}
            tableContext={{
              hasMore,
              emptyText: customerAdminPageTranslations.emptyTableText,
            }}
          />
        </TableFrame>
      )}
    </DataPage>
  );
};
