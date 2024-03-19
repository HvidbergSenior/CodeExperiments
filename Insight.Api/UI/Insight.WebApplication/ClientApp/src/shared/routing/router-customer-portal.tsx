import { createBrowserRouter, Navigate } from "react-router-dom";
import { FuelConsumptionPage } from "../../pages/customer-portal/fuel-consumption/fuel-consumption";
import { SustainabilityReportPdf } from "../../pages/customer-portal/pdf/sustainability-report-pdf-page";
import { CustomerPortalRoot } from "../../pages/customer-portal/customer-portal-root";
import { SustainabilityReportingPage } from "../../pages/customer-portal/sustainability-reporting/sustainability-reporting";
import { SettingsPage } from "../../pages/customer-portal/settings/settings";
import {
  AvailableAccessRights,
  PermissionRoute,
} from "../../hooks/use-permissions/use-permissions";

export const CustomerPortalContentRoutes = [
  {
    path: "/customer-portal",
    element: <CustomerPortalRoot />,
    children: [
      {
        index: true,
        element: <Navigate to="fuel-consumption" />,
      },
      {
        path: "fuel-consumption",
        element: (
          <PermissionRoute
            requiredPermission={AvailableAccessRights.fuelConsumption}
          >
            <FuelConsumptionPage />
          </PermissionRoute>
        ),
      },
      {
        path: "sustainability-reporting",
        element: (
          <PermissionRoute
            requiredPermission={AvailableAccessRights.sustainabilityReport}
          >
            <SustainabilityReportingPage />
          </PermissionRoute>
        ),
      },
      {
        path: "settings",
        element: (
          <PermissionRoute requiredPermission={AvailableAccessRights.admin}>
            <SettingsPage />
          </PermissionRoute>
        ),
      },
    ],

    errorElement: <Navigate to="/customer-portal" replace={true} />,
  },
  {
    path: "/pdf",
    element: (
      <PermissionRoute
        requiredPermission={AvailableAccessRights.sustainabilityReport}
      >
        <SustainabilityReportPdf />
      </PermissionRoute>
    ),
  },
];

export const authenticatedRoutesCustomerPortal = createBrowserRouter([
  {
    path: "/",
    element: <Navigate to="/customer-portal" replace={true} />,
  },
  ...CustomerPortalContentRoutes,
  {
    path: "*",
    element: <Navigate to="/customer-portal" replace={true} />,
  },
]);
