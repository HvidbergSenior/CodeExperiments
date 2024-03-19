export const outgoingTabTranslations = {
  outgoingTabTitle: "Outgoing",
  allocateButton: "Automatic allocation",
  emptyTextDeclarationTable: "No fuel transactions have been found",
  companyColumnHeader: "Company",
  countryColumnHeader: "Country",
  productColumnHeader: "Product",
  customerNumberColumnHeader: "Customer no.",
  customerNameColumnHeader: "Customer name",
  customerTypeColumnHeader: "Customer type",
  segmentColumnHeader: "Segment",
  volumeColumnHeader: "Volume (L)",
  allocationColumnHeader: "Allocation",
  allocationPercentColumnHeader: "Allocation %",
  missingAllocationColumnHeader: "Missing allocation",
  stateColumnHeader: "State",
  manualAllocationContextMenuItem: "Manual allocation",
  viewCustomerContextMenuItem: "View customer",

  manualAllocationDialog: {
    title: "Manual allocation",
    submitButton: "Allocate",
    allocationFullWarning: "Allocation is already full",
    allocationComplete: "Manual allocation complete",
  },
  snackbarMessages: {
    automaticAllocationSuccess: "Successfully allocated automatically",
  },
  confirmAllocateTitle: "Automatic allocation",
  confirmAllocateDescription: (dateFrom: string, dateTo: string) =>
    `Are you sure, that you want to allocate all outgoing volume in the period ${dateFrom} - ${dateTo}?`,
  confirmAllocateSubmit: "Allocate",
  allocating: "Allocating...",
};
