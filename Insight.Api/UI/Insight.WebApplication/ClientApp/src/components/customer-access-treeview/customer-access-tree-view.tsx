import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { Checkbox, Typography } from "@mui/material";
import { Box } from "@mui/system";
import { TreeItem, TreeView } from "@mui/x-tree-view";
import { MouseEvent, memo } from "react";
import { AccessNode } from "./access-node";
import { CustomerNode } from "./customer-node";
import { TreeNode } from "./tree-node";

type Props = {
  data: TreeNode[];
  selectedNodes: string[];
  setSelectedNodes: React.Dispatch<React.SetStateAction<string[]>>;
};

type TreeNodeRenderResult = {
  element: JSX.Element;
  hasAnyCheckedChildren: boolean;
};

export const CustomerAccessTreeView = memo(
  ({ data, selectedNodes, setSelectedNodes }: Props) => {
    /**
     * Breadth First Search algorithm to find node by it's ID
     *
     */
    const bfsSearch = (
      rootNode: TreeNode[],
      targetId: string,
    ): TreeNode | undefined => {
      const queue = [...rootNode];

      while (queue.length > 0) {
        const currNode = queue.shift();
        if (currNode !== undefined && currNode.id === targetId) {
          return currNode;
        }
        if (currNode !== undefined && currNode.children) {
          queue.push(...currNode.children);
        }
        if (
          currNode instanceof CustomerNode &&
          currNode !== undefined &&
          currNode.access
        ) {
          queue.push(...currNode.access);
        }
      }
      return undefined; // Target node not found
    };

    /**
     * Get IDs of all descendants from specific node, including the node itself.
     *
     */
    function getAllDescendantsByNode(
      node: TreeNode,
      outputIdList: string[] = [],
    ): string[] {
      outputIdList.push(node.id);
      if (node instanceof CustomerNode) {
        node.access.forEach((child) =>
          getAllDescendantsByNode(child, outputIdList),
        );
      }
      if (node.children) {
        node.children.forEach((child) =>
          getAllDescendantsByNode(child, outputIdList),
        );
      }
      return outputIdList;
    }

    /**
     * Get IDs of all descendants from specific node with given id, including the node itself.
     *
     */
    const getAllDescendants = (id: string): string[] => {
      const node = bfsSearch(data, id);
      if (node === undefined) {
        return [];
      }
      return getAllDescendantsByNode(node);
    };

    /**
     * Get all ancestors from specific node with given id (not including the node itself).
     * Ordered from the nearest ancestor first.
     *
     */
    const getAllAncestors = (
      id: string,
      outputIdList: string[] = [],
    ): string[] => {
      const node = bfsSearch(data, id);
      if (node === undefined) {
        return outputIdList;
      }
      if (node.parent) {
        outputIdList.push(node.parent);

        return getAllAncestors(node.parent, outputIdList);
      }

      return outputIdList;
    };

    function isAllDescendantsChecked(node: TreeNode, list: string[]) {
      const allChildren = getAllDescendants(node.id);
      const nodeIdIndex = allChildren.indexOf(node.id);
      // Remove the node itself
      allChildren.splice(nodeIdIndex, 1);

      return allChildren.every((nodeId) =>
        selectedNodes.concat(list).includes(nodeId),
      );
    }

    const handleNodeSelect = (
      event: MouseEvent<HTMLButtonElement>,
      nodeId: string,
    ) => {
      event.stopPropagation();
      const allDescendants = getAllDescendants(nodeId);
      const allAncestors = getAllAncestors(nodeId);

      if (selectedNodes.includes(nodeId)) {
        // Need to uncheck
        setSelectedNodes((prevSelectedNodes) =>
          prevSelectedNodes.filter(
            (id) => !allDescendants.concat(allAncestors).includes(id),
          ),
        );
      } else {
        // Need to check
        // Check all descendants
        var toBeChecked = allDescendants;
        // Check all ancestors that have all children selected now
        for (let i = 0; i < allAncestors.length; ++i) {
          const id = allAncestors[i];
          const node = bfsSearch(data, id);
          if (
            node !== undefined &&
            isAllDescendantsChecked(node, toBeChecked)
          ) {
            toBeChecked.push(allAncestors[i]);
          }
        }
        // Perform automatic selection based on access permissions
        const node = bfsSearch(data, nodeId);
        if (node instanceof AccessNode) {
          const parentId = node?.parent;
          if (parentId !== undefined) {
            const parentNode = bfsSearch(data, parentId);
            if (parentNode !== undefined) {
              if (node.type === "Admin") {
                // If admin is selected, ensure selection of admins in all descendants of the customer and all other permissions
                // (That is actually all descendant nodes from the parent/customer of this admin node)
                const allParentDescendants = getAllDescendants(parentNode.id);
                toBeChecked = toBeChecked.concat(allParentDescendants);
                const allParentAncestors = getAllAncestors(parentNode.id);
                // Check all ancestors that have all children selected now
                for (let i = 0; i < allParentAncestors.length; ++i) {
                  const id = allParentAncestors[i];
                  const node = bfsSearch(data, id);
                  if (
                    node !== undefined &&
                    isAllDescendantsChecked(node, toBeChecked)
                  ) {
                    toBeChecked.push(allParentAncestors[i]);
                  }
                }
              } else if (node.type !== "FuelConsumption") {
                // Ensure selection of fuel consumption if one of the other is selected
                if (parentNode instanceof CustomerNode) {
                  for (var accessNode of parentNode.access) {
                    if (accessNode.type === "FuelConsumption") {
                      const accessNodeId = accessNode.id;
                      if (
                        !selectedNodes.includes(accessNodeId) &&
                        !toBeChecked.includes(accessNodeId)
                      ) {
                        toBeChecked.push(accessNodeId);
                      }
                    }
                  }
                }
              }
            }
          }
        }

        setSelectedNodes((prevSelectedNodes) =>
          [...prevSelectedNodes].concat(toBeChecked),
        );
      }
    };

    const handleExpandClick = (event: MouseEvent<HTMLLIElement>) => {
      // prevent the click event from propagating to the checkbox
      event.stopPropagation();
    };

    const renderTree = (
      node: TreeNode,
      isDisabled: boolean,
      isParentAdminSelected: boolean,
    ): TreeNodeRenderResult => {
      var hasAnyCheckedChildren = false;
      var childNodes: JSX.Element[] = [];
      var isAdminSelected = false;
      if (node instanceof CustomerNode) {
        var isOtherThanFuelConsumptionSelected = false;
        // Check selection state of all child access nodes
        for (var accessNode of node.access) {
          if (accessNode.type === "Admin") {
            isAdminSelected =
              isAdminSelected || selectedNodes.includes(accessNode.id);
          } else if (accessNode.type !== "FuelConsumption") {
            if (isOtherThanFuelConsumptionSelected) {
              break;
            } else {
              isOtherThanFuelConsumptionSelected = selectedNodes.includes(
                accessNode.id,
              );
            }
          }
        }
        // Render all child access nodes
        const accessNodes = node.access.map((childNode: AccessNode) => {
          var isAdminNode = childNode.type == "Admin";
          var disableChild =
            (isAdminNode && isParentAdminSelected) ||
            (!isAdminNode && isAdminSelected) ||
            (childNode.type === "FuelConsumption" &&
              isOtherThanFuelConsumptionSelected);
          var result = renderTree(childNode, disableChild, isAdminSelected);
          hasAnyCheckedChildren = result.hasAnyCheckedChildren || hasAnyCheckedChildren;
          return result.element;
        });
        childNodes = childNodes.concat(accessNodes);
      }
      // Render all child customer nodes
      const customerNodes = node.children.map((childNode: TreeNode) => {
        // The child customer node is disabled if admin is selected for its parent
        var result = renderTree(childNode, isAdminSelected, isAdminSelected);
        hasAnyCheckedChildren = result.hasAnyCheckedChildren || hasAnyCheckedChildren;
        return result.element;
      });
      childNodes = childNodes.concat(customerNodes);
      var isChecked = selectedNodes.indexOf(node.id) !== -1;
      var isIndeterminate = !isChecked && hasAnyCheckedChildren;
      var element = (
        <TreeItem
          key={node.id}
          nodeId={node.id}
          onClick={handleExpandClick}
          label={
            <Box display="flex">
              <Checkbox
                checked={isChecked}
                indeterminate={isIndeterminate}
                disableRipple
                onClick={(event: MouseEvent<HTMLButtonElement>) =>
                  handleNodeSelect(event, node.id)
                }
                disabled={isDisabled}
              />
              {node instanceof CustomerNode && (
                <Typography mt={3.3} mb={3.3}>
                  {node.toText()}
                </Typography>
              )}
              {node instanceof AccessNode && (
                <Typography mt={3.3} mb={3.3} variant="subheading1">
                  {node.toText()}
                </Typography>
              )}
            </Box>
          }
        >
          {childNodes.length > 0 ? childNodes : null}
        </TreeItem>
      );
      return {
        element: element,
        hasAnyCheckedChildren: isChecked || hasAnyCheckedChildren,
      };
    };

    return (
      <TreeView
        multiSelect
        defaultCollapseIcon={<ExpandMoreIcon />}
        defaultExpandIcon={<ChevronRightIcon />}
        selected={selectedNodes}
      >
        {data.map((node: TreeNode) => renderTree(node, false, false).element)}
      </TreeView>
    );
  },
);
