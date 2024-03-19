export interface TreeNode {
  id: string;
  parent: string;
  children: TreeNode[];
  toText(): string;
}
