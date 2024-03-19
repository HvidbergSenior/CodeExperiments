import { CustomerPermission } from "../../api/api";
import { customerAdminPageTranslations } from "../../translations/pages/customer-admin-page-translations";
import { TreeNode } from "./tree-node";

export class AccessNode implements TreeNode {
  id: string;
  type: CustomerPermission;
  parent: string;
  children: TreeNode[] = [];

  constructor(id: string, type: CustomerPermission, parent = "") {
    this.id = id;
    this.type = type;
    this.parent = parent;
    this.children = [];
  }

  toText() {
    switch (this.type) {
      case "Admin":
        return customerAdminPageTranslations.addUserDialog.accessFlagAdmin;
      case "FleetManagement":
        return customerAdminPageTranslations.addUserDialog
          .accessFlagFleetManagement;
      case "FuelConsumption":
        return customerAdminPageTranslations.addUserDialog
          .accessFlagFuelConsumption;
      case "SustainabilityReport":
        return customerAdminPageTranslations.addUserDialog
          .accessFlagSustainabilityReporting;
    }
  }
}
