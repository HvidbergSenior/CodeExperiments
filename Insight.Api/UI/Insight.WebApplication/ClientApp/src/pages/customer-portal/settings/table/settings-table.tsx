import { TableComponents } from "react-virtuoso";

import { AllUserResponse } from "../../../../api/api";
import { DataPage } from "../../../../components/data-page/data-page";
import { TableFrame } from "../../../../components/table/table-frame";
import { getFixedHeaderAndRowContent } from "../../../../components/table/virtual-table/header-row-content";
import VirtualTable from "../../../../components/table/virtual-table/virtual-table";
import { genericVirtualTableComponents } from "../../../../components/table/virtual-table/virtual-table-components";
import {
  TableContext,
  VirtuosoTableComponentsGroup,
} from "../../../../components/types";
import { customerPortalTranslations } from "../../../../translations/pages/customer-portal-translations";
import { useSettingsContext } from "../context/settings-context";
import { userTableCustomerPortalColumns } from "./user-columns";

export const userVirtualTableComponents: TableComponents<
  AllUserResponse,
  TableContext
> = genericVirtualTableComponents;

export const CustomerAdminUserTable = () => {
  const props = useSettingsContext();

  const fixedHeaderAndRowContent = getFixedHeaderAndRowContent({
    data: props.queryStateUsers.data?.users ?? [],
    columns: userTableCustomerPortalColumns,
    getLabelProps: props.getLabelProps,
  });

  const virtualComponentsForTable: VirtuosoTableComponentsGroup<AllUserResponse> =
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
      {({ users, hasMoreUsers, totalAmountOfUsers }) => (
        <TableFrame
          numberOfEntriesLoaded={numberOfEntriesLoaded}
          totalNumberOfEntries={totalAmountOfUsers}
          customEntriesText={customerPortalTranslations.settings.usersLoaded}
        >
          <VirtualTable
            data={users}
            loadMore={props.loadMore}
            virtualTableComponents={virtualComponentsForTable}
            tableContext={{
              hasMore: hasMoreUsers,
              emptyText: customerPortalTranslations.settings.emptyTableText,
            }}
          />
        </TableFrame>
      )}
    </DataPage>
  );
};
