import { GetPossibleCustomerPermissionsCustomerNodeDto } from "../api/api";
import { AccessNode } from "../components/customer-access-treeview/access-node";
import { CustomerNode } from "../components/customer-access-treeview/customer-node";
import { TreeNode } from "../components/customer-access-treeview/tree-node";

export const mapPossibleCustomerPermissions = (
  node: GetPossibleCustomerPermissionsCustomerNodeDto,
) => {
  const children: TreeNode[] = [];
  for (const child of node.children) {
    const childNode = mapPossibleCustomerPermissions(child);
    children.push(childNode);
  }
  const access: AccessNode[] = [];

  return new CustomerNode(
    node.customerId,
    node.customerName,
    node.customerNumber,
    node.parentCustomerId,
    children,
    access,
  );
};

export const hasDecendantsOrSelfAMatch = (
  customer: TreeNode,
  filter: string,
): boolean => {
  const isCustomerNode = customer instanceof CustomerNode;
  if (!isCustomerNode) {
    return false;
  }
  const isMatch = (customer.name + customer.customerNumber)
    .toLowerCase()
    .includes(filter.toLowerCase());
  if (isMatch) {
    return true;
  }
  for (var child of customer.children) {
    if (hasDecendantsOrSelfAMatch(child, filter)) {
      return true;
    }
  }
  return false;
};

export const hasDecendantsOrSelfWithId = (
  customer: TreeNode,
  selectedIds: string[],
): boolean => {
  const isCustomerNode = customer instanceof CustomerNode;
  if (!isCustomerNode) {
    return false;
  }
  let isMatch = false;
  for (var selectedId of selectedIds) {
    isMatch = customer.id.toLowerCase().includes(selectedId.toLowerCase());
    if (isMatch) {
      return true;
    }
  }

  for (var child of customer.children) {
    if (hasDecendantsOrSelfWithId(child, selectedIds)) {
      return true;
    }
  }
  return false;
};
