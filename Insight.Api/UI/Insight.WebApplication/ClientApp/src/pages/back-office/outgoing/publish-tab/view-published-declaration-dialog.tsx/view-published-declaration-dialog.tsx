import { Typography } from "@mui/material";
import { Box } from "@mui/system";
import { TableComponents } from "react-virtuoso";
import {
  GetOutgoingDeclarationIncomingDeclarationResponse,
  OutgoingDeclarationResponse,
} from "../../../../../api/api";
import { DataPage } from "../../../../../components/data-page/data-page";
import { OperationDialog } from "../../../../../components/dialogs/operation-dialog";
import { TableFrame } from "../../../../../components/table/table-frame";
import { getFixedHeaderAndRowContent } from "../../../../../components/table/virtual-table/header-row-content";
import VirtualTable from "../../../../../components/table/virtual-table/virtual-table";
import { genericVirtualTableComponents } from "../../../../../components/table/virtual-table/virtual-table-components";
import {
  TableContext,
  VirtuosoTableComponentsGroup,
} from "../../../../../components/types";
import { DialogBaseProps } from "../../../../../shared/types";
import { translations } from "../../../../../translations";
import { allocationTabTranslations } from "../../../../../translations/pages/allocation-tab-translations";
import { publishedDeclarationsVirtualTableComponents } from "../table/declarations-table";
import { useViewPublishedDeclarations } from "./use-view-published-declarations";
import { virtualPublishedDialogColumns } from "./view-published-dialog-columns";
import { virtualPublishedDialogIncomingDeclarationColumns } from "./virtual-dialog-columns";
import { publishedTabTranslations } from "../../../../../translations/pages/published-tab-translations";

interface Props extends DialogBaseProps {
  publishedDeclaration: OutgoingDeclarationResponse;
}

const declarationVirtualTableComponents: TableComponents<
  GetOutgoingDeclarationIncomingDeclarationResponse,
  TableContext
> = genericVirtualTableComponents;

export const ViewPublishedDeclarationDialog = ({
  publishedDeclaration,
  ...props
}: Props) => {
  const { queryStateDeclarations, isLoading, volume, getLabelProps } =
    useViewPublishedDeclarations(publishedDeclaration);

  const fixedHeaderAndRowContentAllocation = getFixedHeaderAndRowContent({
    data: [publishedDeclaration],
    columns: virtualPublishedDialogColumns,
  });

  const virtualComponentsForTableAllocation: VirtuosoTableComponentsGroup<OutgoingDeclarationResponse> =
    {
      virtuosoTableComponents: publishedDeclarationsVirtualTableComponents,
      fixedHeaderContent: fixedHeaderAndRowContentAllocation.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContentAllocation.rowContent,
    };

  const fixedHeaderAndRowContent = getFixedHeaderAndRowContent({
    data:
      queryStateDeclarations.data?.outgoingDeclarationByIdResponse
        ?.getOutgoingDeclarationIncomingDeclarationResponse ?? [],
    columns: virtualPublishedDialogIncomingDeclarationColumns,
    getLabelProps: getLabelProps,
  });

  const virtualComponentsForTable: VirtuosoTableComponentsGroup<GetOutgoingDeclarationIncomingDeclarationResponse> =
    {
      virtuosoTableComponents: declarationVirtualTableComponents,
      fixedHeaderContent: fixedHeaderAndRowContent.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContent.rowContent,
    };

  return (
    <OperationDialog
      title={publishedTabTranslations.viewDeclarationDialog.title}
      isOpen={props.isOpen}
      onClose={props.onClose}
      cancelTitle={translations.commonTranslations.close}
      isLoading={isLoading}
      maxWidth={false}
      showSubmit={false}
      scroll="body"
    >
      <Box height="160px">
        <VirtualTable
          data={[publishedDeclaration]}
          virtualTableComponents={virtualComponentsForTableAllocation}
          tableContext={{
            hasMore: false,
          }}
          loadMore={() => {}}
        />
      </Box>

      <Box sx={{ m: 10 }} />

      <Typography variant="h3">
        {allocationTabTranslations.viewAllocationDialog.declarationTableTitle}
      </Typography>

      <Box sx={{ m: 3 }} />

      <DataPage queryState={queryStateDeclarations}>
        {({ outgoingDeclarationByIdResponse }) => (
          <TableFrame
            offset={600}
            addBordersForDialog={true}
            numberOfEntriesLoaded={
              (
                outgoingDeclarationByIdResponse?.getOutgoingDeclarationIncomingDeclarationResponse ??
                []
              ).length
            }
            totalNumberOfEntries={
              (
                outgoingDeclarationByIdResponse?.getOutgoingDeclarationIncomingDeclarationResponse ??
                []
              ).length
            }
            volume={volume}
          >
            <VirtualTable
              data={
                outgoingDeclarationByIdResponse?.getOutgoingDeclarationIncomingDeclarationResponse ??
                []
              }
              loadMore={() => {}}
              virtualTableComponents={virtualComponentsForTable}
              tableContext={{
                hasMore: false,
              }}
            />
          </TableFrame>
        )}
      </DataPage>
    </OperationDialog>
  );
};
