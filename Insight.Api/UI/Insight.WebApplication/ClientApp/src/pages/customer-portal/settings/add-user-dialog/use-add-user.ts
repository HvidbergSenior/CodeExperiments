import { useCallback, useEffect, useState } from "react";
import { api, authorizedHttpClient } from "../../../../api";
import {
  AllUserAdminResponse,
  CustomerPermission,
  GetPossibleCustomerPermissionsCustomerNodeDto,
  GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto,
  RegisterUserCustomerPermissionDto,
  RegisterUserRequest,
  UpdateUserRequest,
} from "../../../../api/api";
import { AccessNode } from "../../../../components/customer-access-treeview/access-node";
import { CustomerNode } from "../../../../components/customer-access-treeview/customer-node";
import { TreeNode } from "../../../../components/customer-access-treeview/tree-node";
import { useSnackBar } from "../../../../shared/snackbar/use-snackbar";
import { customerAdminPageTranslations } from "../../../../translations/pages/customer-admin-page-translations";
import { isApiError } from "../../../../util/errors/predicates";
import useHandleNetworkError from "../../../../util/errors/use-handle-network-error";
import { hasDecendantsOrSelfAMatch } from "../../../../util/mapping";

export interface UserData {
  firstName: string;
  lastName: string;
  email: string;
  customerNumber: number;
  status: string;
  userType: string;
}

export const useAddUserCustomerPortal = (userData?: AllUserAdminResponse) => {
  const { showErrorDialog } = useHandleNetworkError();
  const { showSnackBar } = useSnackBar();
  const [isLoading, setIsLoading] = useState(false);
  const [isLoadingCustomerPermissions, setIsLoadingCustomerPermissions] =
    useState(false);
  const [apiError, setApiError] = useState<api.Error | undefined>(undefined);

  const [customers, setCustomers] = useState<TreeNode[]>([]);

  const [filteredCustomers, setFilteredCustomers] = useState<TreeNode[]>([]);

  const [selectedNodes, setSelectedNodes] = useState<string[]>([]);

  const mapCustomerPermissions = (
    node: GetPossibleCustomerPermissionsCustomerNodeDto,
  ) => {
    const children: TreeNode[] = [];
    for (const child of node.children) {
      const childNode = mapCustomerPermissions(child);
      children.push(childNode);
    }
    const access: AccessNode[] = [];
    for (const permission of node.permissions) {
      const accessNodeId = node.customerId + "." + permission; // This format is also used when collecting selected permission data for registerUser
      const childNode = new AccessNode(
        accessNodeId,
        permission,
        node.customerId,
      );
      access.push(childNode);
    }
    return new CustomerNode(
      node.customerId,
      node.customerName,
      node.customerNumber,
      node.parentCustomerId,
      children,
      access,
    );
  };

  const getPossibleCustomerPermissions = useCallback(async () => {
    setIsLoadingCustomerPermissions(true);
    try {
      const response =
        await authorizedHttpClient.api.getPossibleCustomerPermissions();
      const mappedCustomerPermissions = response.data.customerNodes.map(
        (customer) => mapCustomerPermissions(customer),
      );
      setCustomers(mappedCustomerPermissions);
      filterCustomers(
        "",
        mappedCustomerPermissions.sort((a, b) => (a.name < b.name ? -1 : 1)),
      );
      setIsLoadingCustomerPermissions(false);
      setApiError(undefined);
    } catch (error) {
      const errorData = isApiError(error);
      if (errorData) {
        setApiError(errorData);
      } else {
        showErrorDialog(error);
      }
      setIsLoadingCustomerPermissions(false);
    }
  }, [setCustomers]);

  const mapPossibleAndGivenCustomerPermissions = useCallback(
    (
      node: GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto,
      selectedNodesOutputList: string[],
    ) => {
      const children: TreeNode[] = [];
      for (const child of node.children) {
        const childNode = mapPossibleAndGivenCustomerPermissions(
          child,
          selectedNodesOutputList,
        );
        children.push(childNode);
      }
      const access: AccessNode[] = [];
      for (const permission of node.permissionsAvailable) {
        const accessNodeId = node.customerId + "." + permission; // This format is also used when collecting selected permission data for registerUser
        const childNode = new AccessNode(
          accessNodeId,
          permission,
          node.customerId,
        );
        access.push(childNode);
        // Add the node id to the selected nodes, if the permission represented by this node is already given.
        for (const givenPermission of node.permissionsGiven) {
          if (givenPermission === permission) {
            selectedNodesOutputList.push(accessNodeId);
            break;
          }
        }
      }
      return new CustomerNode(
        node.customerId,
        node.customerName,
        node.customerNumber,
        node.parentCustomerId,
        children,
        access,
      );
    },
    [setSelectedNodes],
  );

  const getPossibleAndGivenCustomerPermissions = useCallback(
    async (userName: string) => {
      setIsLoadingCustomerPermissions(true);
      try {
        const response =
          await authorizedHttpClient.api.getPossibleCustomerPermissionsForGivenUser(
            { userName: userName },
          );
        const selectedNodesOutputList: string[] = [];
        const mappedCustomerPermissions = response.data.customerNodes.map(
          (customer) =>
            mapPossibleAndGivenCustomerPermissions(
              customer,
              selectedNodesOutputList,
            ),
        );
        setSelectedNodes(selectedNodesOutputList);
        setCustomers(mappedCustomerPermissions);
        filterCustomers(
          "",
          mappedCustomerPermissions.sort((a, b) => (a.name < b.name ? -1 : 1)),
        );
        setIsLoadingCustomerPermissions(false);
        setApiError(undefined);
      } catch (error) {
        const errorData = isApiError(error);
        if (errorData) {
          setApiError(errorData);
        } else {
          showErrorDialog(error);
        }
        setIsLoadingCustomerPermissions(false);
      }
    },
    [setCustomers, setSelectedNodes],
  );

  useEffect(() => {
    getPossibleCustomerPermissions();
    if (userData) {
      getPossibleAndGivenCustomerPermissions(userData.userName);
    } else {
      getPossibleCustomerPermissions();
    }
  }, []);

  const createPermissionsForNode = useCallback(
    (
      node: TreeNode,
      outputList: RegisterUserCustomerPermissionDto[] = [],
    ): RegisterUserCustomerPermissionDto[] => {
      const permissions: CustomerPermission[] = [];
      if (node instanceof CustomerNode) {
        node.access.forEach((accessNode) => {
          const accessNodeId = node.id + "." + accessNode.type; // This format is also used when mapping incoming customer data
          if (selectedNodes.includes(accessNodeId)) {
            permissions.push(accessNode.type);
          }
        });
        if (permissions.length > 0) {
          outputList.push({
            customerId: node.id,
            customerName: node.name,
            customerNumber: node.customerNumber,
            permissions: permissions,
          });
        }
        // If a parent node's permissions include admin, then we only send the parent node in the api request
        // For that reason, we do not want to traverse the children as they will automatically be added to permissions by the backend
        // when the user is set as admin of the parent.
        if (!permissions.includes("Admin")) {
          node.children.forEach((child) => {
            createPermissionsForNode(child, outputList);
          });
        }
      }

      return outputList;
    },
    [selectedNodes],
  );

  const createPermissionsFromSelectedNodes =
    useCallback((): RegisterUserCustomerPermissionDto[] => {
      const outputList: RegisterUserCustomerPermissionDto[] = [];
      for (const customer of customers) {
        createPermissionsForNode(customer, outputList);
      }
      return outputList;
    }, [selectedNodes, customers]);

  const registerUser = useCallback(
    async (registerUserRequest: RegisterUserRequest) => {
      setIsLoading(true);
      try {
        await authorizedHttpClient.api.registerUser({
          ...registerUserRequest,
          customerPermissions: createPermissionsFromSelectedNodes(),
        });
        showSnackBar(
          customerAdminPageTranslations.addUserDialog.userSaved,
          "success",
        );
        setIsLoading(false);
        setApiError(undefined);
        return true;
      } catch (error) {
        const errorData = isApiError(error);
        if (errorData) {
          setApiError(errorData);
        } else {
          showErrorDialog(error);
        }
        setIsLoading(false);
        return false;
      }
    },
    [selectedNodes, customers],
  );

  const updateUser = useCallback(
    async (updateUserRequest: UpdateUserRequest) => {
      setIsLoading(true);
      updateUserRequest.customerPermissions =
        createPermissionsFromSelectedNodes();
      try {
        await authorizedHttpClient.api.updateUser(
          updateUserRequest.userId,
          updateUserRequest,
        );
        showSnackBar(
          customerAdminPageTranslations.addUserDialog.userSaved,
          "success",
        );
        setIsLoading(false);
        setApiError(undefined);
        return true;
      } catch (error) {
        const errorData = isApiError(error);
        if (errorData) {
          setApiError(errorData);
        } else {
          showErrorDialog(error);
        }
        setIsLoading(false);
        return false;
      }
    },
    [selectedNodes, customers],
  );

  const filterCustomers = useCallback(
    (filter: string, baseCustomers: TreeNode[] | undefined = undefined) => {
      if (baseCustomers === undefined) {
        baseCustomers = customers;
      }
      if (filter.trim() === "") {
        setFilteredCustomers(baseCustomers);
        return;
      }
      const filtered = baseCustomers.filter((customer) =>
        hasDecendantsOrSelfAMatch(customer, filter),
      );
      setFilteredCustomers(filtered);
    },
    [customers],
  );

  return {
    isLoading,
    setIsLoading,
    isLoadingCustomerPermissions,
    registerUser,
    apiError,
    filteredCustomers,
    filterCustomers,
    selectedNodes,
    setSelectedNodes,
    updateUser,
  };
};
