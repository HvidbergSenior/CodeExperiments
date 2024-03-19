import { InterceptAllocationSuggestions } from "./allocation-suggestions-handler";
import {
  InterceptAllocations,
  InterceptLockAllocations,
  InterceptPublishAllocations,
} from "./allocations-handler";
import { InterceptCreateStock } from "./create-stock-handler";
import { InterceptCustomersPermissions } from "./customers-permissions-handler";
import {
  InterceptDeclarationUpload,
  InterceptDeclarationUploadApprove,
} from "./declaration-upload-handler";
import {
  InterceptFuelConsumption,
  InterceptFuelConsumptionTransactions,
} from "./fuel-consumption-handler";
import { InterceptIncomingDeclarations } from "./incoming-declarations-handler";
import { InterceptLogin, InterceptRefresh } from "./loginHandler";
import { InterceptManualAllocation } from "./manual-allocation-handler";
import { InterceptOutgoingFuelTransactions } from "./outgoing-fuel-transactions-handler";
import {
  InterceptPublishedDeclarations,
  InterceptViewPublishedDetails,
} from "./published-declaration-handler";
import {
  InterceptDeclarationReconciliation,
  InterceptReconcileSelectedDeclarations,
} from "./reconcile-declarations-handler";
import { InterceptStockTransactions } from "./stock-transactions-handler";
import { InterceptViewDeclaration } from "./view-declaration-handler";

import { InterceptUsers } from "./users-handler";
import { InterceptAdmininistrationUsers } from "./customer-admin-handler";
import { InterceptPossiblePermissions } from "./possible-permissions-handler";
import { InterceptForgotPassword } from "./forgot-password-handler";
import { InterceptResetPassword } from "./reset-password-handler";

export const handlers = [
  InterceptIncomingDeclarations,
  InterceptLogin,
  InterceptRefresh,
  InterceptDeclarationUpload,
  InterceptDeclarationUploadApprove,
  InterceptDeclarationReconciliation,
  InterceptOutgoingFuelTransactions,
  InterceptViewDeclaration,
  InterceptReconcileSelectedDeclarations,
  InterceptAllocations,
  InterceptAllocationSuggestions,
  InterceptPublishedDeclarations,
  InterceptPublishedDeclarations,
  InterceptViewPublishedDetails,
  InterceptPublishAllocations,
  InterceptLockAllocations,
  InterceptManualAllocation,
  InterceptFuelConsumption,
  InterceptFuelConsumptionTransactions,
  InterceptCreateStock,
  InterceptStockTransactions,
  InterceptCustomersPermissions,
  InterceptUsers,
  InterceptAdmininistrationUsers,
  InterceptPossiblePermissions,
  InterceptForgotPassword,
  InterceptResetPassword,
];
