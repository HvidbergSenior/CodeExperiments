import { AccessNode } from "./access-node";
import { TreeNode } from "./tree-node";

export class CustomerNode implements TreeNode {
  id: string;
  name: string;
  parent: string;
  customerNumber: string;
  children: TreeNode[] = [];
  access: AccessNode[] = [];

  constructor(
    id = "",
    name = "",
    number = "",
    parent = "",
    children: TreeNode[] = [],
    access: AccessNode[] = [],
  ) {
    this.id = id;
    this.name = name;
    this.parent = parent;
    this.customerNumber = number;
    this.children = children;
    this.access = access;
  }

  toText() {
    return this.name + " (" + this.customerNumber + ")";
  }
}
