/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface AccessTokenRequest {
  /** @minLength 1 */
  accessToken: string;
  /** @minLength 1 */
  refreshToken: string;
}

export interface Address {
  /** @minLength 1 */
  name: string;
  /** @minLength 1 */
  street: string;
  /** @minLength 1 */
  streetNumber: string;
  /** @minLength 1 */
  zipCode: string;
  /** @minLength 1 */
  city: string;
  /** @minLength 1 */
  country: string;
}

export interface AllUserAdminResponse {
  /** @minLength 1 */
  userId: string;
  /** @minLength 1 */
  userName: string;
  /** @minLength 1 */
  firstName: string;
  /** @minLength 1 */
  lastName: string;
  /** @minLength 1 */
  email: string;
  hasFuelConsumptionAccess: boolean;
  hasSustainabilityReportAccess: boolean;
  hasFleetManagementAccess: boolean;
  userType: UserRole;
  blocked: boolean;
}

export interface AllUserResponse {
  /** @minLength 1 */
  userId: string;
  /** @minLength 1 */
  userName: string;
  /** @minLength 1 */
  firstName: string;
  /** @minLength 1 */
  lastName: string;
  /** @minLength 1 */
  email: string;
  hasFuelConsumptionAccess: boolean;
  hasSustainabilityReportAccess: boolean;
  hasFleetManagementAccess: boolean;
  userType: UserRole;
  blocked: boolean;
}

export interface AllocationByIdResponse {
  /** @format uuid */
  id: string;
  /** @minLength 1 */
  customer: string;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  country: string;
  /** @minLength 1 */
  product: string;
  /** @format double */
  volume: number;
}

export interface AllocationIncomingDeclarationDto {
  company?: string | null;
  country?: string | null;
  product?: string | null;
  supplier?: string | null;
  rawMaterial?: string | null;
  posNumber?: string | null;
  countryOfOrigin?: string | null;
  placeOfDispatch?: string | null;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateOfDispatch?: string;
  /** @format double */
  quantity?: number;
  /** @format double */
  ghgEmissionSaving?: number;
}

export interface AllocationRequest {
  /** @format uuid */
  incomingDeclarationId?: string;
  /** @format double */
  volume?: number;
}

export interface AllocationResponse {
  /** @format uuid */
  id: string;
  /** @minLength 1 */
  posNumber: string;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  company: string;
  /** @minLength 1 */
  country: string;
  /** @minLength 1 */
  product: string;
  /** @minLength 1 */
  customer: string;
  /** @minLength 1 */
  storage: string;
  /** @minLength 1 */
  certificationSystem: string;
  /** @minLength 1 */
  rawMaterial: string;
  /** @minLength 1 */
  countryOfOrigin: string;
  /** @format double */
  ghgReduction: number;
  /** @format double */
  volume: number;
  warnings: string[];
  hasWarnings: boolean;
  /** @format double */
  fossilFuelComparatorgCO2EqPerMJ: number;
}

export interface ApproveIncomingDeclarationUploadRequest {
  /** @format uuid */
  incomingDeclarationUploadId: string;
}

export interface AuthenticatedResponse {
  /** @minLength 1 */
  refreshToken: string;
  /** @minLength 1 */
  accessToken: string;
}

export interface BlockUserRequest {
  /** @minLength 1 */
  userName: string;
}

export interface CancelIncomingDeclarationsByUploadIdRequest {
  /** @format uuid */
  incomingDeclarationUploadId: string;
}

export interface ChangePasswordRequest {
  /** @minLength 1 */
  userName: string;
  /** @minLength 1 */
  currentPassword: string;
  /** @minLength 1 */
  newPassword: string;
  /** @minLength 1 */
  confirmPassword: string;
}

export interface ConsumptionDevelopment {
  series: FuelConsumptionSeries[];
  categories: string[];
}

export interface ConsumptionPerProduct {
  data: FuelConsumptionNameValuePair[];
}

export interface ConsumptionStats {
  data: number[];
  generalFuelTypes: string[];
  /** @format int32 */
  totalConsumptionFossilFuels: number;
  /** @format int32 */
  totalConsumptionRenewableFuels: number;
  /** @format int32 */
  consumptionTotalForCircle: number;
  /** @format int32 */
  totalConsumptionAllFuels: number;
}

export interface Country {
  /** @minLength 1 */
  name: string;
  /** @format double */
  percentage: number;
}

export interface CreateStockTransactionRequest {
  /** @minLength 1 */
  productNumber: string;
  /** @format uuid */
  companyId: string;
  /** @format double */
  quantity: number;
  /**
   * @format date
   * @example "2024-01-01"
   */
  transactionDate: string;
  /** @minLength 1 */
  country: string;
  /** @minLength 1 */
  location: string;
}

export type CustomerPermission =
  | "Admin"
  | "FuelConsumption"
  | "SustainabilityReport"
  | "FleetManagement";

export interface CustomerPermissionDto {
  /** @minLength 1 */
  customerId: string;
  /** @minLength 1 */
  customerName: string;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  parentCustomerId: string;
  permissions: CustomerPermission[];
  children: CustomerPermissionDto[];
}

export interface Declarationinfo {
  /** @minLength 1 */
  id: string;
  /** @format date-time */
  dateOfIssuance: string;
}

export interface Emissionsstats {
  /** @format int32 */
  achievedEmissionReductions: number;
  /** @format int32 */
  netEmission: number;
  /** @format double */
  emissionSavingsForCircle: number;
}

export interface Error {
  /** @minLength 1 */
  type: string;
  /** @minLength 1 */
  title: string;
  /** @minLength 1 */
  detail: string;
  /** @minLength 1 */
  instance: string;
  /** @format int32 */
  status: number;
  traceId?: string | null;
}

export interface ErrorDetail {
  code?: string | null;
  field?: string | null;
  attemptedValue?: any;
  message?: string | null;
}

export interface Feedstock {
  /** @minLength 1 */
  name: string;
  /** @format double */
  percentage: number;
}

export interface ForgotPasswordRequest {
  userName?: string | null;
  email?: string | null;
}

export interface FuelConsumptionNameValuePair {
  productNameEnumeration: ProductNameEnumeration;
  /** @format double */
  value: number;
}

export interface FuelConsumptionSeries {
  productNameEnumeration: ProductNameEnumeration;
  data: number[];
}

export interface FuelConsumptionTransaction {
  /** @minLength 1 */
  id: string;
  /** @minLength 1 */
  date: string;
  /** @minLength 1 */
  time: string;
  /** @minLength 1 */
  stationId: string;
  /** @minLength 1 */
  stationName: string;
  /** @minLength 1 */
  productNumber: string;
  /** @minLength 1 */
  productName: string;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  customerName: string;
  /** @minLength 1 */
  cardNumber: string;
  /** @format double */
  quantity: number;
  /** @minLength 1 */
  location: string;
}

export interface FuelTransactionsBatchRequest {
  /** @format uuid */
  customerId?: string;
  /**
   * @format date
   * @example "2024-01-01"
   */
  startDate?: string;
  /**
   * @format date
   * @example "2024-01-01"
   */
  endDate?: string;
  productNumber?: string | null;
  country?: string | null;
  stationName?: string | null;
  productName?: string | null;
  locationId?: string | null;
}

export interface GetAllUsersAdminResponse {
  hasMoreUsers: boolean;
  /** @format int32 */
  totalAmountOfUsers: number;
  users: AllUserAdminResponse[];
}

export interface GetAllUsersResponse {
  hasMoreUsers: boolean;
  /** @format int32 */
  totalAmountOfUsers: number;
  users: AllUserResponse[];
}

export interface GetAllocationByIdResponse {
  allocationByIdResponse?: AllocationByIdResponse;
  allocationIncomingDeclarationDtos?: AllocationIncomingDeclarationDto[] | null;
}

export interface GetAllocationSuggestionsResponse {
  hasMoreSuggestions: boolean;
  /** @format double */
  totalAmountOfSuggestions: number;
  suggestions: SuggestionResponse[];
}

export interface GetAllocationsResponse {
  hasMoreAllocations: boolean;
  /** @format double */
  totalAmountOfAllocations: number;
  allocations: AllocationResponse[];
  isDraftLocked: boolean;
}

export interface GetAvailableCustomersPermissionsResponse {
  customerNodes: CustomerPermissionDto[];
}

export interface GetFuelConsumptionRequest {
  productNames: ProductNameEnumeration[];
  customerIds: string[];
  customerNumbers: string[];
  /** @format int32 */
  maxColumns: number;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateTo: string;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateFrom: string;
}

export interface GetFuelConsumptionResponse {
  consumptionPerProduct: ConsumptionPerProduct;
  consumptionDevelopment: ConsumptionDevelopment;
  consumptionStats: ConsumptionStats;
}

export interface GetFuelConsumptionTransactionsRequest {
  productNames: ProductNameEnumeration[];
  customerIds: string[];
  customerNumbers: string[];
  /** @format int32 */
  maxColumns: number;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateTo: string;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateFrom: string;
}

export interface GetFuelConsumptionTransactionsResponse {
  data: FuelConsumptionTransaction[];
  hasMoreTransactions: boolean;
  /** @format int32 */
  totalAmountOfTransactions: number;
}

export interface GetIncomingDeclarationsByPageAndPageSizeResponse {
  hasMoreDeclarations: boolean;
  /** @format double */
  totalAmountOfDeclarations: number;
  incomingDeclarationsByPageAndPageSize: IncomingDeclarationResponse[];
}

export interface GetOutgoingDeclarationByIdResponse {
  outgoingDeclarationByIdResponse?: OutgoingDeclarationByIdResponse;
}

export interface GetOutgoingDeclarationIncomingDeclarationResponse {
  /** @format uuid */
  incomingDeclarationId?: string;
  company?: string | null;
  country?: string | null;
  product?: string | null;
  supplier?: string | null;
  rawMaterial?: string | null;
  posNumber?: string | null;
  countryOfOrigin?: string | null;
  placeOfDispatch?: string | null;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateOfDispatch?: string;
  /** @format double */
  quantity?: number;
  /** @format double */
  ghgEmissionSaving?: number;
  /** @format int64 */
  batchId?: number;
}

export interface GetOutgoingDeclarationsByCustomerIdResponse {
  outgoingDeclarationByCustomerIdResponse?:
    | OutgoingDeclarationsByCustomerIdResponse[]
    | null;
}

export interface GetOutgoingDeclarationsByPageAndPageSizeResponse {
  hasMoreDeclarations: boolean;
  /** @format double */
  totalAmountOfDeclarations: number;
  outgoingDeclarationsByPageAndPageSizeResponse: OutgoingDeclarationResponse[];
}

export interface GetOutgoingDeclarationsResponse {
  outgoingDeclarationsResponses?: OutgoingDeclarationsResponse[] | null;
}

export interface GetOutgoingFuelTransactionsQueryResponse {
  hasMoreOutgoingFuelTransactions: boolean;
  /** @format int32 */
  totalAmountOfOutgoingFuelTransactions: number;
  /** @format double */
  totalQuantity: number;
  outgoingFuelTransactions: OutgoingFuelTransactionResponse[];
}

export interface GetPossibleCustomerPermissionsCustomerNodeDto {
  /** @minLength 1 */
  customerId: string;
  /** @minLength 1 */
  customerName: string;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  parentCustomerId: string;
  permissions: CustomerPermission[];
  children: GetPossibleCustomerPermissionsCustomerNodeDto[];
}

export interface GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto {
  /** @minLength 1 */
  customerId: string;
  /** @minLength 1 */
  customerName: string;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  parentCustomerId: string;
  permissionsGiven: CustomerPermission[];
  permissionsAvailable: CustomerPermission[];
  children: GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto[];
}

export interface GetPossibleCustomerPermissionsForGivenUserResponse {
  customerNodes: GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto[];
}

export interface GetPossibleCustomerPermissionsResponse {
  customerNodes: GetPossibleCustomerPermissionsCustomerNodeDto[];
}

export interface GetStockTransactionsQueryResponse {
  hasMoreStockTransactions: boolean;
  /** @format int32 */
  totalAmountOfStockTransactions: number;
  stockTransactions: StockTransactionResponse[];
}

export interface GetSustainabilityReportPdfRequest {
  productNames: ProductNameEnumeration[];
  customerIds: string[];
  customerNumbers: string[];
  /** @format int32 */
  maxColumns: number;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateTo: string;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateFrom: string;
}

export interface GetSustainabilityReportPdfResponse {
  consumptionStats: ConsumptionStats;
  emissionsStats: Emissionsstats;
  consumptionPerProduct: ConsumptionPerProduct;
  consumptionDevelopment: ConsumptionDevelopment;
  progress: Progress;
  feedstocks: Feedstock[];
  countries: Country[];
  productSpecificationItems: ProductSpecificationItem[];
  pdfReportPosResponses: PdfReportPosResponse[];
}

export interface GetSustainabilityReportRequest {
  productNames: ProductNameEnumeration[];
  customerIds: string[];
  customerNumbers: string[];
  /** @format int32 */
  maxColumns: number;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateTo: string;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateFrom: string;
}

export interface GetSustainabilityReportResponse {
  feedstocks: Feedstock[];
  countries: Country[];
  progress: Progress;
  productSpecificationItems: ProductSpecificationItem[];
  emissionsStats: Emissionsstats;
}

export interface Greenhousegasemissionssavings {
  /** @format double */
  ghgPercent: number;
}

export interface IncomingDeclarationDto {
  /** @format uuid */
  incomingDeclarationId: string;
  /** @format uuid */
  incomingDeclarationUploadId: string;
  /** @minLength 1 */
  company: string;
  /** @minLength 1 */
  country: string;
  /** @minLength 1 */
  product: string;
  /** @minLength 1 */
  supplier: string;
  /** @minLength 1 */
  rawMaterial: string;
  /** @minLength 1 */
  posNumber: string;
  /** @minLength 1 */
  countryOfOrigin: string;
  incomingDeclarationState: IncomingDeclarationState;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateOfDispatch: string;
  /** @minLength 1 */
  certificationSystem: string;
  /** @minLength 1 */
  supplierCertificateNumber: string;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateOfIssuance: string;
  /** @minLength 1 */
  placeOfDispatch: string;
  /** @minLength 1 */
  productionCountry: string;
  /** @minLength 1 */
  dateOfInstallation: string;
  /** @minLength 1 */
  typeOfProduct: string;
  /** @minLength 1 */
  additionalInformation: string;
  /** @format double */
  quantity: number;
  unitOfMeasurement: UnitOfMeasurement;
  /** @format double */
  energyContentMJ: number;
  /** @format double */
  energyQuantityGJ: number;
  complianceWithSustainabilityCriteria: boolean;
  cultivatedAsIntermediateCrop: boolean;
  fulfillsMeasuresForLowILUCRiskFeedstocks: boolean;
  meetsDefinitionOfWasteOrResidue: boolean;
  /** @minLength 1 */
  specifyNUTS2Region: string;
  totalDefaultValueAccordingToREDII: boolean;
  /** @format double */
  ghgEec: number;
  /** @format double */
  ghgEl: number;
  /** @format double */
  ghgEp: number;
  /** @format double */
  ghgEtd: number;
  /** @format double */
  ghgEu: number;
  /** @format double */
  ghgEsca: number;
  /** @format double */
  ghgEccs: number;
  /** @format double */
  ghgEccr: number;
  /** @format double */
  ghgEee: number;
  /** @format double */
  ghGgCO2EqPerMJ: number;
  /** @format double */
  fossilFuelComparatorgCO2EqPerMJ: number;
  /** @format double */
  ghgEmissionSaving: number;
  /** @format int32 */
  declarationRowNumber: number;
  /** @format double */
  remainingVolume: number;
}

export interface IncomingDeclarationParseResponse {
  /** @format int32 */
  rowNumber: number;
  /** @minLength 1 */
  posNumber: string;
  /** @minLength 1 */
  errorMessage: string;
  success: boolean;
}

export interface IncomingDeclarationResponse {
  /** @minLength 1 */
  company: string;
  /** @minLength 1 */
  country: string;
  /** @minLength 1 */
  product: string;
  /** @minLength 1 */
  supplier: string;
  /** @minLength 1 */
  rawMaterial: string;
  /** @minLength 1 */
  id: string;
  /** @minLength 1 */
  posNumber: string;
  /** @minLength 1 */
  countryOfOrigin: string;
  incomingDeclarationState: IncomingDeclarationState;
  /** @minLength 1 */
  placeOfDispatch: string;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateOfDispatch: string;
  /** @format double */
  quantity: number;
  /** @format double */
  ghgEmissionSaving: number;
  /** @format double */
  remainingVolume: number;
}

export type IncomingDeclarationState =
  | "Temporary"
  | "New"
  | "Reconciled"
  | "Allocated";

export type IncomingDeclarationSupplier = "BFE" | "Neste";

export interface IncomingDeclarationUploadIdResponse {
  /** @format uuid */
  id: string;
}

export interface Lifecyclegreenhousegasemissions {
  /** @format double */
  extractionOrCultivation: number;
  /** @format int32 */
  landUse: number;
  /** @format double */
  processing: number;
  /** @format double */
  transportAndDistribution: number;
  /** @format int32 */
  fuelInUse: number;
  /** @format int32 */
  soilCarbonAccumulation: number;
  /** @format int32 */
  carbonCaptureAndGeologicalStorage: number;
  /** @format int32 */
  carbonCaptureAndReplacement: number;
  /** @format double */
  totalGHGEmissionFromSupplyAndUseOfFuel: number;
}

export interface LoginRequest {
  /** @minLength 1 */
  username: string;
  /** @minLength 1 */
  password: string;
}

export interface OutgoingDeclarationByIdResponse {
  /** @minLength 1 */
  outgoingDeclarationId: string;
  /** @minLength 1 */
  country: string;
  /** @minLength 1 */
  product: string;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  customerName: string;
  bfeId?: string | null;
  /**
   * @format date
   * @example "2024-01-01"
   */
  startDate?: string;
  /**
   * @format date
   * @example "2024-01-01"
   */
  endDate?: string;
  certificateId?: string | null;
  sustainabilityDeclarationNumber?: string | null;
  /**
   * @format date
   * @example "2024-01-01"
   */
  dateOfIssuance?: string;
  rawMaterialName?: string | null;
  rawMaterialCode?: string | null;
  productionCountry?: string | null;
  additionalInformation?: string | null;
  /** @format double */
  mt?: number;
  /** @format double */
  density?: number;
  /** @format double */
  liter?: number;
  /** @format double */
  energyContent?: number;
  /** @format double */
  greenhouseGasEmission?: number;
  /** @format double */
  greenhouseGasReduction?: number;
  /** @format double */
  emissionSavingControl?: number;
  /** @format double */
  energyContentControl?: number;
  getOutgoingDeclarationIncomingDeclarationResponse?:
    | GetOutgoingDeclarationIncomingDeclarationResponse[]
    | null;
}

export interface OutgoingDeclarationResponse {
  /** @format uuid */
  outgoingDeclarationId: string;
  /** @minLength 1 */
  country: string;
  /** @minLength 1 */
  product: string;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  customerName: string;
  /** @format double */
  volumeTotal: number;
  /** @format double */
  allocationTotal: number;
  /** @format double */
  ghgReduction: number;
  /** @format double */
  fossilFuelComparatorgCO2EqPerMJ: number;
  incomingDeclarationIds: string[];
}

export interface OutgoingDeclarationsByCustomerIdResponse {
  /** @minLength 1 */
  outgoingDeclarationId: string;
  /** @minLength 1 */
  country: string;
  /** @minLength 1 */
  product: string;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  customerName: string;
}

export interface OutgoingDeclarationsResponse {
  /** @format uuid */
  outgoingDeclarationId: string;
  /** @minLength 1 */
  country: string;
  /** @minLength 1 */
  product: string;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  customerName: string;
}

export interface OutgoingFuelTransactionResponse {
  /** @minLength 1 */
  id: string;
  /** @format uuid */
  customerId: string;
  /** @minLength 1 */
  productNumber: string;
  /** @minLength 1 */
  productName: string;
  /** @minLength 1 */
  stationName: string;
  /** @format double */
  quantity: number;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  customerName: string;
  /** @minLength 1 */
  country: string;
  /** @minLength 1 */
  location: string;
  /** @minLength 1 */
  customerType: string;
  /** @minLength 1 */
  customerSegment: string;
  /** @format double */
  allocatedQuantity: number;
  /** @format double */
  alreadyAllocatedPercentage: number;
  /** @format double */
  missingAllocationQuantity: number;
  /** @minLength 1 */
  locationId: string;
}

export interface PdfReportPosResponse {
  recipient: Recipient;
  declarationinfo: Declarationinfo;
  renewablefuelsupplier: Renewablefuelsupplier;
  renewablefuel: Renewablefuel;
  scopeofcertificationandghgemission: Scopeofcertificationandghgemission;
  rawmaterialsustainability: Rawmaterialsustainability;
  scopeofcertificationofrawmaterial: Scopeofcertificationofrawmaterial;
  lifecyclegreenhousegasemissions: Lifecyclegreenhousegasemissions;
  greenhousegasemissionssavings: Greenhousegasemissionssavings;
}

export interface PostAutomaticAllocationRequest {
  /**
   * @format date
   * @example "2024-01-01"
   */
  startDate?: string;
  /**
   * @format date
   * @example "2024-01-01"
   */
  endDate?: string;
  product?: string | null;
  company?: string | null;
  customer?: string | null;
}

export interface PostManualAllocationRequest {
  fuelTransactionsBatch?: FuelTransactionsBatchRequest;
  allocations?: AllocationRequest[] | null;
}

export type ProductNameEnumeration =
  | "Unknown"
  | "Hvo100"
  | "HvoDiesel"
  | "Adblue"
  | "B100"
  | "Diesel"
  | "Petrol"
  | "HeatingOil"
  | "Other";

export interface ProductSpecificationItem {
  /** @minLength 1 */
  fuelType: string;
  /** @format double */
  volume: number;
  /** @format double */
  ghgBaseline: number;
  /** @format double */
  ghgEmissionSaving: number;
  /** @format double */
  achievedEmissionReduction: number;
  /** @format double */
  netEmission: number;
}

export interface Progress {
  emissions: number[];
  emissionReduction: number[];
  categories: string[];
}

export interface Rawmaterialsustainability {
  /** @minLength 1 */
  rawMaterial: string;
  /** @minLength 1 */
  countryOfOrigin: string;
  /** @minLength 1 */
  productionCountry: string;
  /** @minLength 1 */
  dateOfInstallation: string;
}

export interface Recipient {
  address: Address;
  /** @format date-time */
  periodFrom: string;
  /** @format date-time */
  periodTo: string;
}

export interface ReconcileIncomingDeclarationsRequest {
  incomingDeclarationIds: string[];
}

export interface RegisterUserCustomerPermissionDto {
  /** @format uuid */
  customerId: string;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  customerName: string;
  permissions: CustomerPermission[];
}

export interface RegisterUserRequest {
  /** @minLength 1 */
  firstName: string;
  /** @minLength 1 */
  lastName: string;
  status: UserStatus;
  /** @minLength 1 */
  username: string;
  /**
   * @format email
   * @minLength 1
   */
  email: string;
  role: UserRole;
  password?: string | null;
  confirmPassword?: string | null;
  customerPermissions: RegisterUserCustomerPermissionDto[];
}

export interface Renewablefuel {
  /** @format int32 */
  volume: number;
  /** @minLength 1 */
  product: string;
  /** @format int32 */
  energyContent: number;
}

export interface Renewablefuelsupplier {
  address: Address;
  /** @minLength 1 */
  certificateSystem: string;
  /** @minLength 1 */
  certificateNumber: string;
}

export interface ResetPasswordRequest {
  /** @minLength 1 */
  userName: string;
  /** @minLength 1 */
  token: string;
  /** @minLength 1 */
  newPassword: string;
}

export interface Scopeofcertificationandghgemission {
  euRedCompliantMaterial: boolean;
  isccCompliantMaterial: boolean;
  /** @minLength 1 */
  chainOfCustodyOption: string;
  totalDefaultValueAccordingToRed2Applied: boolean;
}

export interface Scopeofcertificationofrawmaterial {
  option1: boolean;
  option2: boolean;
  option3: boolean;
  option4: boolean;
  /** @minLength 1 */
  option5: string;
}

export interface StockTransactionResponse {
  /** @format uuid */
  id: string;
  /** @format uuid */
  companyId: string;
  /** @minLength 1 */
  companyName: string;
  /** @minLength 1 */
  productNumber: string;
  /** @minLength 1 */
  productName: string;
  /** @format double */
  quantity: number;
  /** @minLength 1 */
  country: string;
  /** @minLength 1 */
  location: string;
  /** @format double */
  allocatedQuantity: number;
  /** @format double */
  alreadyAllocatedPercentage: number;
  /** @format double */
  missingAllocationQuantity: number;
  /** @minLength 1 */
  locationId: string;
}

export interface SuggestionResponse {
  /** @format uuid */
  id: string;
  /** @minLength 1 */
  posNumber: string;
  /**
   * @format date
   * @example "2024-01-01"
   */
  period: string;
  /** @minLength 1 */
  storage: string;
  /** @minLength 1 */
  company: string;
  /** @minLength 1 */
  product: string;
  /** @minLength 1 */
  rawMaterial: string;
  /** @minLength 1 */
  supplier: string;
  /** @minLength 1 */
  countryOfOrigin: string;
  /** @minLength 1 */
  country: string;
  incomingDeclarationState: IncomingDeclarationState;
  /** @format double */
  volumeAvailable: number;
  /** @format double */
  volume: number;
  /** @format double */
  ghgReduction: number;
  hasWarnings: boolean;
  warnings: string[];
}

export interface UnblockUserRequest {
  /** @minLength 1 */
  userName: string;
}

export type UnitOfMeasurement =
  | "Litres"
  | "CubicMeters"
  | "Kilograms"
  | "MetricTon";

export interface UpdateIncomingDeclarationResponse {
  /** @minLength 1 */
  company: string;
  /** @minLength 1 */
  country: string;
  /** @minLength 1 */
  product: string;
  /** @minLength 1 */
  supplier: string;
  /** @minLength 1 */
  rawMaterial: string;
  /** @format uuid */
  updatedIncomingDeclarationId: string;
  /** @minLength 1 */
  posNumber: string;
  /** @minLength 1 */
  countryOfOrigin: string;
  incomingDeclarationState: IncomingDeclarationState;
}

export interface UpdateUserCustomerPermissionDto {
  /** @format uuid */
  customerId: string;
  /** @minLength 1 */
  customerNumber: string;
  /** @minLength 1 */
  customerName: string;
  permissions: CustomerPermission[];
}

export interface UpdateUserRequest {
  /** @minLength 1 */
  userId: string;
  /** @minLength 1 */
  firstName: string;
  /** @minLength 1 */
  lastName: string;
  status: UserStatus;
  /** @minLength 1 */
  username: string;
  /**
   * @format email
   * @minLength 1
   */
  email: string;
  role: UserRole;
  customerPermissions?: UpdateUserCustomerPermissionDto[] | null;
}

export interface UpdateUserResponse {
  /** @minLength 1 */
  userName: string;
  /** @format uuid */
  userId: string;
}

export interface UploadIncomingDeclarationCommandResponse {
  incomingDeclarationUploadId: IncomingDeclarationUploadIdResponse;
  incomingDeclarationParseResponses: IncomingDeclarationParseResponse[];
  /**
   * @format date
   * @example "2024-01-01"
   */
  oldestEntry: string;
  /**
   * @format date
   * @example "2024-01-01"
   */
  newestEntry: string;
}

export type UserRole = "User" | "Admin";

export type UserStatus = "Blocked" | "Active" | "BlockedAndActive";

export interface ValidationError {
  /** @minLength 1 */
  type: string;
  /** @minLength 1 */
  title: string;
  /** @minLength 1 */
  detail: string;
  /** @minLength 1 */
  instance: string;
  /** @format int32 */
  status: number;
  traceId?: string | null;
  errors?: ErrorDetail[] | null;
}

import type {
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  HeadersDefaults,
  ResponseType,
} from "axios";
import axios from "axios";

export type QueryParamsType = Record<string | number, any>;

export interface FullRequestParams
  extends Omit<AxiosRequestConfig, "data" | "params" | "url" | "responseType"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseType;
  /** request body */
  body?: unknown;
}

export type RequestParams = Omit<
  FullRequestParams,
  "body" | "method" | "query" | "path"
>;

export interface ApiConfig<SecurityDataType = unknown>
  extends Omit<AxiosRequestConfig, "data" | "cancelToken"> {
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<AxiosRequestConfig | void> | AxiosRequestConfig | void;
  secure?: boolean;
  format?: ResponseType;
}

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public instance: AxiosInstance;
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private secure?: boolean;
  private format?: ResponseType;

  constructor({
    securityWorker,
    secure,
    format,
    ...axiosConfig
  }: ApiConfig<SecurityDataType> = {}) {
    this.instance = axios.create({
      ...axiosConfig,
      baseURL: axiosConfig.baseURL || "",
    });
    this.secure = secure;
    this.format = format;
    this.securityWorker = securityWorker;
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected mergeRequestParams(
    params1: AxiosRequestConfig,
    params2?: AxiosRequestConfig,
  ): AxiosRequestConfig {
    const method = params1.method || (params2 && params2.method);

    return {
      ...this.instance.defaults,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...((method &&
          this.instance.defaults.headers[
            method.toLowerCase() as keyof HeadersDefaults
          ]) ||
          {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected stringifyFormItem(formItem: unknown) {
    if (typeof formItem === "object" && formItem !== null) {
      return JSON.stringify(formItem);
    } else {
      return `${formItem}`;
    }
  }

  protected createFormData(input: Record<string, unknown>): FormData {
    return Object.keys(input || {}).reduce((formData, key) => {
      const property = input[key];
      const propertyContent: any[] =
        property instanceof Array ? property : [property];

      for (const formItem of propertyContent) {
        const isFileType = formItem instanceof Blob || formItem instanceof File;
        formData.append(
          key,
          isFileType ? formItem : this.stringifyFormItem(formItem),
        );
      }

      return formData;
    }, new FormData());
  }

  public request = async <T = any, _E = any>({
    secure,
    path,
    type,
    query,
    format,
    body,
    ...params
  }: FullRequestParams): Promise<AxiosResponse<T>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const responseFormat = format || this.format || undefined;

    if (
      type === ContentType.FormData &&
      body &&
      body !== null &&
      typeof body === "object"
    ) {
      body = this.createFormData(body as Record<string, unknown>);
    }

    if (
      type === ContentType.Text &&
      body &&
      body !== null &&
      typeof body !== "string"
    ) {
      body = JSON.stringify(body);
    }

    return this.instance.request({
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type && type !== ContentType.FormData
          ? { "Content-Type": type }
          : {}),
      },
      params: query,
      responseType: responseFormat,
      data: body,
      url: path,
    });
  };
}

/**
 * @title Insight API
 * @version v1
 */
export class Api<
  SecurityDataType extends unknown,
> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags Administrationusers
     * @name GetAllUsersAdmin
     * @request GET:/api/administrationusers
     * @secure
     */
    getAllUsersAdmin: (
      query: {
        /** @format int32 */
        page: number;
        /** @format int32 */
        pageSize: number;
        isOrderDescending: boolean;
        orderByProperty: string;
        status: string;
        customerName?: string;
        customerNumber?: string;
        email?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<GetAllUsersAdminResponse, any>({
        path: `/api/administrationusers`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Allocations
     * @name GetAllocationSuggestions
     * @request GET:/api/allocations/suggestions
     * @secure
     */
    getAllocationSuggestions: (
      query: {
        customerId: string;
        startDate?: string;
        endDate?: string;
        product: string;
        country: string;
        location: string;
        /** @format int32 */
        page: number;
        /** @format int32 */
        pageSize: number;
        isOrderDescending: boolean;
        orderByProperty: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<GetAllocationSuggestionsResponse, any>({
        path: `/api/allocations/suggestions`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Allocations
     * @name PostManualAllocation
     * @request POST:/api/allocations/manual
     * @secure
     */
    postManualAllocation: (
      data: PostManualAllocationRequest,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/allocations/manual`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Allocations
     * @name PostAutomaticAllocation
     * @request POST:/api/allocations/automatic
     * @secure
     */
    postAutomaticAllocation: (
      data: PostAutomaticAllocationRequest,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/allocations/automatic`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Allocations
     * @name GetAllocations
     * @request GET:/api/allocations
     * @secure
     */
    getAllocations: (
      query: {
        dateFrom?: string;
        dateTo?: string;
        product?: string;
        company?: string;
        customer?: string;
        /** @format int32 */
        page: number;
        /** @format int32 */
        pageSize: number;
        isOrderDescending: boolean;
        orderByProperty: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<GetAllocationsResponse, any>({
        path: `/api/allocations`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Allocations
     * @name LockAllocations
     * @request POST:/api/allocations/lock
     * @secure
     */
    lockAllocations: (params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/allocations/lock`,
        method: "POST",
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Allocations
     * @name UnlockAllocations
     * @request POST:/api/allocations/unlock
     * @secure
     */
    unlockAllocations: (params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/allocations/unlock`,
        method: "POST",
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Allocations
     * @name PublishAllocations
     * @request POST:/api/allocations/publish
     * @secure
     */
    publishAllocations: (params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/allocations/publish`,
        method: "POST",
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Allocations
     * @name ClearAllocations
     * @request POST:/api/allocations/clear
     * @secure
     */
    clearAllocations: (params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/allocations/clear`,
        method: "POST",
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Allocations
     * @name GetAllocationById
     * @request GET:/api/allocations/{id}
     * @secure
     */
    getAllocationById: (
      id: string,
      query: {
        isOrderDescending: boolean;
        orderByProperty: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<GetAllocationByIdResponse, any>({
        path: `/api/allocations/${id}`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Authentication
     * @name Login
     * @request POST:/api/authentication/login
     * @secure
     */
    login: (data: LoginRequest, params: RequestParams = {}) =>
      this.request<AuthenticatedResponse, any>({
        path: `/api/authentication/login`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Authentication
     * @name ChangePassword
     * @request POST:/api/users/changepassword
     * @secure
     */
    changePassword: (data: ChangePasswordRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/users/changepassword`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Authentication
     * @name BlockUser
     * @request POST:/api/users/blockuser
     * @secure
     */
    blockUser: (data: BlockUserRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/users/blockuser`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Authentication
     * @name UnblockUser
     * @request POST:/api/users/unblockuser
     * @secure
     */
    unblockUser: (data: UnblockUserRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/users/unblockuser`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Authentication
     * @name Refresh
     * @request POST:/api/authentication/refresh
     * @secure
     */
    refresh: (data: AccessTokenRequest, params: RequestParams = {}) =>
      this.request<AuthenticatedResponse, any>({
        path: `/api/authentication/refresh`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Authentication
     * @name ForgotPassword
     * @request POST:/api/users/forgotpassword
     * @secure
     */
    forgotPassword: (data: ForgotPasswordRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/users/forgotpassword`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Authentication
     * @name ResetPassword
     * @request POST:/api/users/resetpassword
     * @secure
     */
    resetPassword: (data: ResetPasswordRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/users/resetpassword`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Customers
     * @name GetAvailableCustomersPermissions
     * @request GET:/api/customers/permissions
     * @secure
     */
    getAvailableCustomersPermissions: (params: RequestParams = {}) =>
      this.request<GetAvailableCustomersPermissionsResponse, any>({
        path: `/api/customers/permissions`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Customers
     * @name GetPossibleCustomerPermissions
     * @request GET:/api/customers/possiblepermissions
     * @secure
     */
    getPossibleCustomerPermissions: (params: RequestParams = {}) =>
      this.request<GetPossibleCustomerPermissionsResponse, any>({
        path: `/api/customers/possiblepermissions`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Customers
     * @name GetPossibleCustomerPermissionsForGivenUser
     * @request GET:/api/customers/possiblepermissionsforgivenuser
     * @secure
     */
    getPossibleCustomerPermissionsForGivenUser: (
      query: {
        userName: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<GetPossibleCustomerPermissionsForGivenUserResponse, any>({
        path: `/api/customers/possiblepermissionsforgivenuser`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags GeneratePDF
     * @name Generatepdf
     * @request GET:/api/generatepdf
     * @secure
     */
    generatepdf: (
      query: {
        targetUrl: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/generatepdf`,
        method: "GET",
        query: query,
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags IncomingDeclarations
     * @name GetIncomingDeclarationsByPageAndPageSize
     * @request GET:/api/incomingdeclarations/pagination
     * @secure
     */
    getIncomingDeclarationsByPageAndPageSize: (
      query: {
        dateFrom?: string;
        dateTo?: string;
        product?: string;
        company?: string;
        supplier?: string;
        /** @format int32 */
        page: number;
        /** @format int32 */
        pageSize: number;
        isOrderDescending: boolean;
        orderByProperty: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<GetIncomingDeclarationsByPageAndPageSizeResponse, any>({
        path: `/api/incomingdeclarations/pagination`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags IncomingDeclarations
     * @name GetReconciledIncomingDeclarations
     * @request GET:/api/incomingdeclarations/reconciled
     * @secure
     */
    getReconciledIncomingDeclarations: (
      query: {
        dateFrom?: string;
        dateTo?: string;
        product?: string;
        company?: string;
        supplier?: string;
        /** @format int32 */
        page: number;
        /** @format int32 */
        pageSize: number;
        isOrderDescending: boolean;
        orderByProperty: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<GetIncomingDeclarationsByPageAndPageSizeResponse, any>({
        path: `/api/incomingdeclarations/reconciled`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags IncomingDeclarations
     * @name ApproveIncomingDeclarationUpload
     * @request POST:/api/incomingdeclarations/approve
     * @secure
     */
    approveIncomingDeclarationUpload: (
      data: ApproveIncomingDeclarationUploadRequest,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/incomingdeclarations/approve`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags IncomingDeclarations
     * @name ReconcileIncomingDeclaration
     * @request POST:/api/incomingdeclarations/reconcile
     * @secure
     */
    reconcileIncomingDeclaration: (
      data: ReconcileIncomingDeclarationsRequest,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/incomingdeclarations/reconcile`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags IncomingDeclarations
     * @name UpdateIncomingDeclaration
     * @request PUT:/api/incomingdeclarations/{id}
     * @secure
     */
    updateIncomingDeclaration: (
      id: string,
      data: IncomingDeclarationDto,
      params: RequestParams = {},
    ) =>
      this.request<UpdateIncomingDeclarationResponse, any>({
        path: `/api/incomingdeclarations/${id}`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags IncomingDeclarations
     * @name GetIncomingDeclarationById
     * @request GET:/api/incomingdeclarations/{id}
     * @secure
     */
    getIncomingDeclarationById: (id: string, params: RequestParams = {}) =>
      this.request<IncomingDeclarationDto, any>({
        path: `/api/incomingdeclarations/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags IncomingDeclarations
     * @name CancelIncomingDeclarationByUploadId
     * @request POST:/api/incomingdeclarations/cancel/{uploadId}
     * @secure
     */
    cancelIncomingDeclarationByUploadId: (
      uploadId: string,
      data: CancelIncomingDeclarationsByUploadIdRequest,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/incomingdeclarations/cancel/${uploadId}`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags IncomingDeclarations
     * @name IncomingdeclarationsUploadCreate
     * @request POST:/api/incomingdeclarations/upload
     * @secure
     */
    incomingdeclarationsUploadCreate: (
      data: {
        /** @format binary */
        ExcelFile: File;
        IncomingDeclarationSupplier: IncomingDeclarationSupplier;
      },
      params: RequestParams = {},
    ) =>
      this.request<
        UploadIncomingDeclarationCommandResponse,
        ValidationError | Error
      >({
        path: `/api/incomingdeclarations/upload`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.FormData,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags OutgoingDeclarations
     * @name GetOutgoingDeclarationById
     * @request GET:/api/outgoingdeclarations/{id}
     * @secure
     */
    getOutgoingDeclarationById: (id: string, params: RequestParams = {}) =>
      this.request<GetOutgoingDeclarationByIdResponse, any>({
        path: `/api/outgoingdeclarations/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags OutgoingDeclarations
     * @name GetSustainabilityReportPdf
     * @request POST:/api/outgoingdeclarations/sustainabilityreportpdf
     * @secure
     */
    getSustainabilityReportPdf: (
      data: GetSustainabilityReportPdfRequest,
      params: RequestParams = {},
    ) =>
      this.request<GetSustainabilityReportPdfResponse, any>({
        path: `/api/outgoingdeclarations/sustainabilityreportpdf`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags OutgoingDeclarations
     * @name GetSustainabilityReport
     * @request POST:/api/outgoingdeclarations/sustainabilityreport
     * @secure
     */
    getSustainabilityReport: (
      data: GetSustainabilityReportRequest,
      params: RequestParams = {},
    ) =>
      this.request<GetSustainabilityReportResponse, any>({
        path: `/api/outgoingdeclarations/sustainabilityreport`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags OutgoingDeclarations
     * @name GetFuelConsumption
     * @request POST:/api/outgoingdeclarations/fuelconsumption
     * @secure
     */
    getFuelConsumption: (
      data: GetFuelConsumptionRequest,
      params: RequestParams = {},
    ) =>
      this.request<GetFuelConsumptionResponse, any>({
        path: `/api/outgoingdeclarations/fuelconsumption`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags OutgoingDeclarations
     * @name GetFuelConsumptionTransactions
     * @request POST:/api/outgoingdeclarations/fuelconsumptiontransactions
     * @secure
     */
    getFuelConsumptionTransactions: (
      query: {
        /** @format int32 */
        page: number;
        /** @format int32 */
        pageSize: number;
        isOrderDescending: boolean;
        orderByProperty: string;
      },
      data: GetFuelConsumptionTransactionsRequest,
      params: RequestParams = {},
    ) =>
      this.request<GetFuelConsumptionTransactionsResponse, any>({
        path: `/api/outgoingdeclarations/fuelconsumptiontransactions`,
        method: "POST",
        query: query,
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags OutgoingDeclarations
     * @name GetFuelConsumptionTransactionsExcelFile
     * @request GET:/api/outgoingdeclarations/fuelconsumptiontransactionsexcelfile
     * @secure
     */
    getFuelConsumptionTransactionsExcelFile: (
      query: {
        fromDate: string;
        toDate: string;
        productNames?: ProductNameEnumeration[];
        customerIds?: string[];
      },
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/outgoingdeclarations/fuelconsumptiontransactionsexcelfile`,
        method: "GET",
        query: query,
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags OutgoingDeclarations
     * @name GetOutgoingDeclarationsByCustomerId
     * @request GET:/api/outgoingdeclarations/customer/{customerId}
     * @secure
     */
    getOutgoingDeclarationsByCustomerId: (
      customerId: string,
      params: RequestParams = {},
    ) =>
      this.request<GetOutgoingDeclarationsByCustomerIdResponse, any>({
        path: `/api/outgoingdeclarations/customer/${customerId}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags OutgoingDeclarations
     * @name GetOutgoingDeclarations
     * @request GET:/api/outgoingdeclarations
     * @secure
     */
    getOutgoingDeclarations: (params: RequestParams = {}) =>
      this.request<GetOutgoingDeclarationsResponse, any>({
        path: `/api/outgoingdeclarations`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags OutgoingDeclarations
     * @name GetOutgoingDeclarationsByPageAndPageSize
     * @request GET:/api/outgoingdeclarations/pagination
     * @secure
     */
    getOutgoingDeclarationsByPageAndPageSize: (
      query: {
        dateFrom?: string;
        dateTo?: string;
        product?: string;
        company?: string;
        customer?: string;
        /** @format int32 */
        page: number;
        /** @format int32 */
        pageSize: number;
        isOrderDescending: boolean;
        orderByProperty: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<GetOutgoingDeclarationsByPageAndPageSizeResponse, any>({
        path: `/api/outgoingdeclarations/pagination`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags OutgoingFuelTransactions
     * @name GetOutgoingFuelTransactions
     * @request GET:/api/outgoingfueltransactions
     * @secure
     */
    getOutgoingFuelTransactions: (
      query: {
        dateFrom?: string;
        dateTo?: string;
        product?: string;
        company?: string;
        customer?: string;
        /** @format int32 */
        page: number;
        /** @format int32 */
        pageSize: number;
        isOrderDescending: boolean;
        orderByProperty: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<GetOutgoingFuelTransactionsQueryResponse, any>({
        path: `/api/outgoingfueltransactions`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Stocks
     * @name GetStockTransactions
     * @request GET:/api/stocktransactions
     * @secure
     */
    getStockTransactions: (
      query: {
        dateFrom?: string;
        dateTo?: string;
        product?: string;
        company?: string;
        /** @format int32 */
        page: number;
        /** @format int32 */
        pageSize: number;
        isOrderDescending: boolean;
        orderByProperty: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<GetStockTransactionsQueryResponse, any>({
        path: `/api/stocktransactions`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags StockTransactions
     * @name CreateStockTransaction
     * @request POST:/api/stocktransactions
     * @secure
     */
    createStockTransaction: (
      data: CreateStockTransactionRequest,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/stocktransactions`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Users
     * @name RegisterUser
     * @request POST:/api/users/register
     * @secure
     */
    registerUser: (data: RegisterUserRequest, params: RequestParams = {}) =>
      this.request<AuthenticatedResponse, any>({
        path: `/api/users/register`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Users
     * @name GetAllUsers
     * @request GET:/api/users
     * @secure
     */
    getAllUsers: (
      query: {
        /** @format int32 */
        page: number;
        /** @format int32 */
        pageSize: number;
        isOrderDescending: boolean;
        orderByProperty: string;
        status: string;
        customerName?: string;
        customerNumber?: string;
        email?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<GetAllUsersResponse, any>({
        path: `/api/users`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Users
     * @name UpdateUser
     * @request PUT:/api/users/{id}
     * @secure
     */
    updateUser: (
      id: string,
      data: UpdateUserRequest,
      params: RequestParams = {},
    ) =>
      this.request<UpdateUserResponse, any>({
        path: `/api/users/${id}`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
}
