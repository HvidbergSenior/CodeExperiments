import { AxiosError } from "axios";
import { Control } from "react-hook-form";
import { api } from "../api";
import {
  AllUserAdminResponse,
  AllocationResponse,
  GetAllUsersAdminResponse,
  GetAllocationsResponse,
  GetIncomingDeclarationsByPageAndPageSizeResponse,
  GetOutgoingDeclarationsByPageAndPageSizeResponse,
  GetOutgoingFuelTransactionsQueryResponse,
  GetStockTransactionsQueryResponse,
  IncomingDeclarationDto,
  IncomingDeclarationResponse,
  LoginRequest,
  OutgoingFuelTransactionResponse,
  RequestParams,
  StockTransactionResponse,
} from "../api/api";

// Api
export type QueryState<T> = {
  isLoading: boolean;
  data?: T;
  error?: AxiosError | undefined;
  hasError?: boolean;
  traceId?: string;
};

export type IncomingApiArgs = {
  page: number;
  pageSize: number;
  isOrderDescending: boolean;
  orderByProperty: string;
  dateFrom: string | undefined;
  dateTo: string | undefined;
  product: string;
  company: string;
  supplier: string;
};

export type OutgoingApiArgs = {
  page: number;
  pageSize: number;
  isOrderDescending: boolean;
  orderByProperty: string;
  filterProperty?: string | undefined;
  filterValue?: string | undefined;
  dateFrom: string;
  dateTo: string;
  product: string;
  company: string;
  customer: string;
};

export type PageArgs = {
  page: number;
  pageSize: number;
  isOrderDescending: boolean;
  orderByProperty: string;
};

export type AllocationSuggestionPageArgs = {
  page: number;
  pageSize: number;
  isOrderDescending: boolean;
  orderByProperty: string;
  customerId: string;
  startDate: string;
  endDate: string;
  product: string;
  country: string;
  location: string;
};

export type MutationState<T> = {
  isComplete: boolean;
  mutateData: (data: T, params: RequestParams) => Promise<void>;
};

export type IncomingResponseKeys = keyof IncomingDeclarationResponse;
export type OutgoingFuelTransactionKeys = keyof OutgoingFuelTransactionResponse;

export interface OutgoingFuelTransactionResponseWithWarnings
  extends OutgoingFuelTransactionResponse {
  hasWarnings: boolean;
  warnings: string[];
}

// Contexts

export interface DeclarationUploadContextProps {
  queryStateDeclarations: QueryState<GetIncomingDeclarationsByPageAndPageSizeResponse>;
  selectRow: (id: string) => void;
  selectedRows: string[];
  selectAllRows: () => void;
  loadMore: () => void;
  handleSubmitOfUploadDeclarations: (
    uploadId: string,
    oldestEntryDate?: string,
    newestEntryDate?: string,
  ) => Promise<void>;
  reconcileDeclarations: () => void;
  getLabelProps: (key: keyof IncomingDeclarationResponse) => {
    active: boolean;
    direction: "asc" | "desc";
    onClick: () => void;
  };
  saveEditedDeclaration: (
    declarationToBeUpdated: IncomingDeclarationDto,
  ) => Promise<void>;
  volume: number | undefined;
}

export interface ReconciliationContextProps {
  queryStateReconciliation: QueryState<GetIncomingDeclarationsByPageAndPageSizeResponse>;
  loadMore: () => void;
  getLabelProps: (key: keyof IncomingDeclarationResponse) => {
    active: boolean;
    direction: "asc" | "desc";
    onClick: () => void;
  };
  saveEditedDeclaration: (
    declarationToBeUpdated: IncomingDeclarationDto,
  ) => Promise<void>;
  volume: number | undefined;
}

export interface OutgoingContextProps {
  queryStateFuelTransactions: QueryState<GetOutgoingFuelTransactionsQueryResponse>;
  loadMore: () => void;
  handleSubmitManualAllocation: () => Promise<void>;
  handleAllocateConfirm: () => Promise<void>;
  getLabelProps: (key: keyof OutgoingFuelTransactionResponse) => {
    active: boolean;
    direction: "asc" | "desc";
    onClick: () => void;
  };
}

export interface StockContextProps {
  queryStateStocks: QueryState<GetStockTransactionsQueryResponse>;
  loadMore: () => void;
  handleSubmitManualAllocation: () => Promise<void>;
  handleCreateStockSubmit: () => Promise<void>;
  handleAllocateConfirm: () => Promise<void>;
  getLabelProps: (key: keyof StockTransactionResponse) => {
    active: boolean;
    direction: "asc" | "desc";
    onClick: () => void;
  };
}

export interface AllocationContextProps {
  queryStateAllocations: QueryState<GetAllocationsResponse>;
  loadMore: () => void;
  getLabelProps: (key: keyof AllocationResponse) => {
    active: boolean;
    direction: "asc" | "desc";
    onClick: () => void;
  };
  handleSubmitViewAllocation: (allocationId: string) => Promise<void>;
  ghgWeightedAvg: number | undefined;
  volume: number | undefined;
  publishAllocations: () => Promise<void>;
  lockAllocations: () => Promise<void>;
  unlockAllocations: () => Promise<void>;
  clearAllocations: () => void;
}

export interface PublishedContextProps {
  queryStateDeclarations: QueryState<GetOutgoingDeclarationsByPageAndPageSizeResponse>;
  loadMore: () => void;
  handleSubmitViewPublishedDeclaration: (id: string) => Promise<void>;
  ghgWeightedAvg: number | undefined;
  volume: number | undefined;
}

export interface CustomerAdminContextProps {
  queryStateUsers: QueryState<GetAllUsersAdminResponse>;
  loadMore: () => void;
  getLabelProps: (key: keyof AllUserAdminResponse) => {
    active: boolean;
    direction: "asc" | "desc";
    onClick: () => void;
  };
  handleRegisterUserSubmit: () => void;
}

export interface AuthValues {}

export interface AuthContextProps {
  authenticated: boolean;
  login: (loginRequest: LoginRequest) => Promise<void>;
  loading: boolean;
  authLoading: boolean;
  error: api.Error | undefined;
  logout: () => void;
  isUser: boolean;
  isBackOfficeAdmin: boolean;
  accessRights: string[];
  username: string;
}

// IDs

export type CertificatesSortId = "name" | "period" | "data" | "state";

// Subpages

export type IncomingSubPages = "declaration_upload" | "approved";

export type OutgoingSubPages =
  | "outgoing-tab"
  | "allocation"
  | "publish"
  | "stock";

// UseForm data

export type LoginFormData = {
  userName: string;
  password: string;
};

// Reconciliation

export type EditDeclarationData = {
  control: Control<IncomingDeclarationDto, any>;
  name: keyof IncomingDeclarationDto;
  value: string | number | boolean;
};

// Customer portal

export interface StyledTabsProps {
  children?: React.ReactNode;
  value: string;
  onChange: (event: React.SyntheticEvent, newValue: number) => void;
}
