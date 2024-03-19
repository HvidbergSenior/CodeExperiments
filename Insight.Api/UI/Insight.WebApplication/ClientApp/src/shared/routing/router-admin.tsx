import { createBrowserRouter, Navigate } from "react-router-dom";
import { ForgotPasswordPage } from "../../pages/authentication/forgot-password/forgot-password-page";
import { Login } from "../../pages/authentication/login/login";
import { CustomerAdminPage } from "../../pages/back-office/customer-administration/customer-admin-page";
import { Incoming } from "../../pages/back-office/incoming/declaration-upload-tab/declaration-upload";
import { IncomingDefault } from "../../pages/back-office/incoming/incoming-page";
import { Reconciliation } from "../../pages/back-office/incoming/reconciliation-tab/reconciliation-tab";
import { AllocationTab } from "../../pages/back-office/outgoing/allocation-tab/allocation-tab";
import { OutgoingPage } from "../../pages/back-office/outgoing/outgoing-page";
import { OutgoingTab } from "../../pages/back-office/outgoing/outgoing-tab/outgoing-tab";
import { PublishedTab } from "../../pages/back-office/outgoing/publish-tab/published-tab";
import { StockTab } from "../../pages/back-office/outgoing/stock-tab/stock-tab";
import { Root } from "../../pages/root";
import { ChangePasswordPage } from "../../pages/authentication/change-password/change-password-page";
import { CustomerPortalContentRoutes } from "./router-customer-portal";
import { MassBalancePage } from "../../pages/back-office/mass-balance/mass-balance-page";

export const authenticatedRoutesBackOffice = createBrowserRouter([
  {
    path: "/",
    element: <Root />,
    children: [
      {
        index: true,
        element: <Navigate to="/incoming" />,
      },
      {
        path: "/incoming",
        element: <IncomingDefault />,
        children: [
          {
            index: true,
            element: <Incoming />,
          },
          {
            path: "declaration_upload",
            element: <Incoming />,
          },
          {
            path: "approved",
            element: <Reconciliation />,
          },
        ],
      },
      {
        path: "/outgoing",
        element: <OutgoingPage />,
        children: [
          {
            index: true,
            element: <OutgoingTab />,
          },
          {
            path: "outgoing-tab",
            element: <OutgoingTab />,
          },
          {
            path: "stock",
            element: <StockTab />,
          },
          {
            path: "allocation",
            element: <AllocationTab />,
          },
          {
            path: "published",
            element: <PublishedTab />,
          },
        ],
      },
      {
        path: "/mass-balance",
        element: <MassBalancePage />,
      },
      {
        path: "/customer-admin",
        element: <CustomerAdminPage />,
      },
    ],

    errorElement: <Navigate to="/" replace={true} />,
  },
  ...CustomerPortalContentRoutes,
  {
    path: "*",
    element: <Navigate to="/" replace={true} />,
  },
]);

export const unauthenticatedRoutes = createBrowserRouter([
  {
    path: "/",
    element: <Navigate to="/login" replace={true} />,
  },
  {
    path: "/login",
    element: <Login />,
    children: [],

    errorElement: <Navigate to="/login" replace={true} />,
  },
  {
    path: "/forgot-password",
    element: <ForgotPasswordPage />,
    children: [],
  },
  {
    path: "/reset-password",
    element: <ChangePasswordPage />,
    children: [],
  },
  {
    path: "*",
    element: <Navigate to="/login" replace={true} />,
  },
]);
