export enum AvailableAccessRights {
  admin = "Admin",
  fuelConsumption = "FuelConsumption",
  sustainabilityReport = "SustainabilityReport",
  fleetManagement = "FleetManagement",
}

import { ReactNode } from "react";
import { useAuthContext } from "../../pages/authentication/login/context/auth-context";
import { Navigate } from "react-router-dom";

export const usePermissions = () => {
  const { accessRights } = useAuthContext();

  const hasAccessTo = (requiredAccessRight: AvailableAccessRights) => {
    return accessRights.includes(requiredAccessRight);
  };

  return { hasAccessTo };
};

interface Props {
  requiredPermission: AvailableAccessRights;
  children: ReactNode;
}
export const PermissionRoute = ({ requiredPermission, children }: Props) => {
  const { hasAccessTo } = usePermissions();
  const enabled = hasAccessTo(requiredPermission);
  return enabled ? <>{children}</> : <Navigate to="/" />;
};
