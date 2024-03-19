import { Typography } from "@mui/material";
import { Box } from "@mui/system";
import { TableComponents } from "react-virtuoso";
import {
  AllocationByIdResponse,
  AllocationIncomingDeclarationDto,
} from "../../../../../api/api";
import { DataPage } from "../../../../../components/data-page/data-page";
import { OperationDialog } from "../../../../../components/dialogs/operation-dialog";
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
import { virtualDeclarationDialogColumns } from "./view-decleration-dialog-columns";
import { useViewAllocation } from "./use-view-allocation";
import { virtualAllocatedDialogColumns } from "./view-allocation-dialog-columns";

interface Props extends DialogBaseProps {
  allocationId: string;
  onSubmit: (allocationId: string) => Promise<void>;
}

const allocationVirtualTableComponents: TableComponents<
  AllocationByIdResponse,
  TableContext
> = genericVirtualTableComponents;

const declerationsVirtualTableComponents: TableComponents<
  AllocationIncomingDeclarationDto,
  TableContext
> = genericVirtualTableComponents;

export const ViewAllocationDialog = ({
  allocationId,
  onSubmit,
  ...props
}: Props) => {
  const useViewAllocationProps = useViewAllocation(allocationId);

  const allocatedId =
    useViewAllocationProps.queryStateAllocationById.data
      ?.allocationByIdResponse!;

  const declerations =
    useViewAllocationProps.queryStateAllocationById.data
      ?.allocationIncomingDeclarationDtos ?? [];

  const fixedHeaderAndRowContentAllocation = getFixedHeaderAndRowContent({
    data: [allocatedId],
    columns: virtualAllocatedDialogColumns,
  });

  const virtualComponentsForTableAllocation: VirtuosoTableComponentsGroup<AllocationByIdResponse> =
    {
      virtuosoTableComponents: allocationVirtualTableComponents,
      fixedHeaderContent: fixedHeaderAndRowContentAllocation.fixedHeaderContent,
      rowContent: fixedHeaderAndRowContentAllocation.rowContent,
    };

  const fixedHeaderAndRowDeclerations = getFixedHeaderAndRowContent({
    data: declerations,
    columns: virtualDeclarationDialogColumns,
    getLabelProps: useViewAllocationProps.getLabelProps,
  });

  const virtualComponentsForTableDeclerations: VirtuosoTableComponentsGroup<AllocationIncomingDeclarationDto> =
    {
      virtuosoTableComponents: declerationsVirtualTableComponents,
      fixedHeaderContent: fixedHeaderAndRowDeclerations.fixedHeaderContent,
      rowContent: fixedHeaderAndRowDeclerations.rowContent,
    };

  return (
    <OperationDialog
      title={allocationTabTranslations.viewAllocationDialog.allocationTitle}
      isOpen={props.isOpen}
      onClose={props.onClose}
      cancelTitle={translations.commonTranslations.close}
      disableSubmit={true}
      isLoading={useViewAllocationProps.queryStateAllocationById.isLoading}
      maxWidth={false}
      showSubmit={false}
      scroll="body"
    >
      <DataPage queryState={useViewAllocationProps.queryStateAllocationById}>
        {({ allocationByIdResponse, allocationIncomingDeclarationDtos }) => (
          <>
            <Box height="160px">
              <VirtualTable
                data={[allocationByIdResponse!]}
                virtualTableComponents={virtualComponentsForTableAllocation}
                tableContext={{
                  hasMore: false,
                }}
                loadMore={() => {}}
              />
            </Box>
            <Typography variant="h3" sx={{ mb: 3 }}>
              {
                allocationTabTranslations.viewAllocationDialog
                  .declarationTableTitle
              }
            </Typography>
            <Box sx={{ height: "40vh" }}>
              <VirtualTable
                data={allocationIncomingDeclarationDtos!}
                virtualTableComponents={virtualComponentsForTableDeclerations}
                tableContext={{
                  hasMore: false,
                }}
                loadMore={() => {}}
              />
            </Box>
          </>
        )}
      </DataPage>
    </OperationDialog>
  );
};
