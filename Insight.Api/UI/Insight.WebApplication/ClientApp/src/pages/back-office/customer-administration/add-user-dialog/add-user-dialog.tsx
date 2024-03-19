import {
  CircularProgress,
  FormControl,
  FormHelperText,
  MenuItem,
  Paper,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import Backdrop from "@mui/material/Backdrop";
import { Box } from "@mui/system";
import { useMemo, useState } from "react";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import {
  AllUserAdminResponse,
  RegisterUserRequest,
  UpdateUserRequest,
  UserRole,
  UserStatus,
} from "../../../../api/api";
import { CustomerAccessTreeView } from "../../../../components/customer-access-treeview/customer-access-tree-view";
import { OperationDialog } from "../../../../components/dialogs/operation-dialog";
import { DebouncedStandardTextfield } from "../../../../components/filter/debounced-standard-textfield";
import { DialogBaseProps } from "../../../../shared/types";
import { commonTranslations } from "../../../../translations/common";
import { customerAdminPageTranslations } from "../../../../translations/pages/customer-admin-page-translations";
import { emailRegExp } from "../../../../util/input-validation";
import { useAddUser } from "./use-add-user";

interface Props extends DialogBaseProps {
  userData?: AllUserAdminResponse;
  onSubmit: () => Promise<void>;
}

type UserRoleMenu = {
  value: UserRole;
  label: string;
};

type UserStatusMenu = {
  value: UserStatus;
  label: string;
};

export const AvailableUserStatus: Record<UserStatus, UserStatus> = {
  Blocked: "Blocked",
  Active: "Active",
  BlockedAndActive: "BlockedAndActive",
};

export const AddUserDialog = ({ userData, onSubmit, ...props }: Props) => {
  const addUserProps = useAddUser(userData);
  const [errorCustomerTree, setErrorCustomerTree] = useState<
    string | undefined
  >(undefined);

  const isEditing = useMemo(() => {
    return !!userData;
  }, [userData]);

  const { formState, handleSubmit, control, watch } =
    useForm<RegisterUserRequest>({
      defaultValues: {
        firstName: userData?.firstName ?? "",
        lastName: userData?.lastName ?? "",
        username: userData?.email ?? "",
        email: userData?.email ?? "",
        status: userData?.blocked
          ? AvailableUserStatus.Blocked
          : AvailableUserStatus.Active,
        confirmPassword: undefined,
        password: undefined,
        role: userData?.userType ?? "User",
        customerPermissions: [],
      },
    });

  const { role } = watch();

  const handleSaveUser: SubmitHandler<RegisterUserRequest> = async (data) => {
    if (addUserProps.selectedNodes.length === 0 && role !== "Admin") {
      // TODO - remove the error when a customer has been selected
      setErrorCustomerTree(
        customerAdminPageTranslations.addUserDialog.errorNoCustomersSelected,
      );
      return;
    }

    if (isEditing) {
      const request = {
        ...data,
        userId: userData?.userId ?? "",
        username: data.email, // username is the email
      } as UpdateUserRequest;

      const result = await addUserProps.updateUser(request);
      if (result) {
        onSubmit();
      }
      return;
    }

    const request: RegisterUserRequest = {
      ...data,
      username: data.email, // username is the email
    };

    const result = await addUserProps.registerUser(request);
    if (result) {
      onSubmit();
    }
  };

  const handleEnterKeyDown = (event: React.KeyboardEvent<HTMLDivElement>) => {
    if (event.key === "Enter") {
      handleSubmit(handleSaveUser)();
    }
  };

  const status: UserStatusMenu[] = [
    {
      value: "Active",
      label: customerAdminPageTranslations.statusActive,
    },
    {
      value: "Blocked",
      label: customerAdminPageTranslations.statusBlocked,
    },
  ];

  const userRole: UserRoleMenu[] = [
    {
      value: "User",
      label: customerAdminPageTranslations.userTypeCustomerUser,
    },
    {
      value: "Admin",
      label: customerAdminPageTranslations.userRoleAdmin,
    },
  ];

  const formControlHeight = "60px";

  return (
    <OperationDialog
      title={
        isEditing
          ? customerAdminPageTranslations.addUserDialog.titleEditUser
          : customerAdminPageTranslations.addUserDialog.titleAddUser
      }
      isOpen={props.isOpen}
      onConfirm={handleSubmit(handleSaveUser)}
      onClose={props.onClose}
      submitTitle={customerAdminPageTranslations.addUserDialog.save}
      cancelTitle={commonTranslations.cancel}
      disableSubmit={false}
      isLoading={addUserProps.isLoading}
      maxWidth={"md"}
      scroll="body"
    >
      <Box sx={{ m: 2 }} />
      <Stack direction="row" gap={20}>
        <Stack direction="column" width="60%" gap={4}>
          <FormControl sx={{ height: formControlHeight }}>
            <Controller
              name="firstName"
              control={control}
              rules={{
                required: customerAdminPageTranslations.addUserDialog.required(
                  customerAdminPageTranslations.addUserDialog.firstName,
                ),
              }}
              render={({ field }) => (
                <TextField
                  {...field}
                  size="small"
                  inputProps={{ style: { fontSize: 14 } }}
                  InputLabelProps={{ style: { fontSize: 14 } }}
                  label={customerAdminPageTranslations.addUserDialog.firstName}
                  variant="outlined"
                  onKeyDown={handleEnterKeyDown}
                />
              )}
            />
            <FormHelperText
              error={!!formState.errors.firstName}
              role={formState.errors.firstName ? "alert" : undefined}
            >
              {formState.errors.firstName?.message ?? ""}
            </FormHelperText>
          </FormControl>

          <FormControl sx={{ height: formControlHeight }}>
            <Controller
              name="lastName"
              control={control}
              rules={{
                required: customerAdminPageTranslations.addUserDialog.required(
                  customerAdminPageTranslations.addUserDialog.lastName,
                ),
              }}
              render={({ field }) => (
                <TextField
                  {...field}
                  size="small"
                  inputProps={{ style: { fontSize: 14 } }}
                  InputLabelProps={{ style: { fontSize: 14 } }}
                  label={customerAdminPageTranslations.addUserDialog.lastName}
                  variant="outlined"
                  onKeyDown={handleEnterKeyDown}
                />
              )}
            />
            <FormHelperText
              error={!!formState.errors.lastName}
              role={formState.errors.lastName ? "alert" : undefined}
            >
              {formState.errors.lastName?.message ?? ""}
            </FormHelperText>
          </FormControl>

          <FormControl sx={{ height: formControlHeight }}>
            <Controller
              name="email"
              control={control}
              rules={{
                required: customerAdminPageTranslations.addUserDialog.required(
                  customerAdminPageTranslations.addUserDialog.email,
                ),
                pattern: {
                  value: emailRegExp,
                  message:
                    customerAdminPageTranslations.addUserDialog.emailFormat,
                },
              }}
              render={({ field }) => (
                <TextField
                  {...field}
                  size="small"
                  inputProps={{ style: { fontSize: 14 } }}
                  InputLabelProps={{ style: { fontSize: 14 } }}
                  label={customerAdminPageTranslations.addUserDialog.email}
                  variant="outlined"
                  onKeyDown={handleEnterKeyDown}
                />
              )}
            />
            <FormHelperText
              error={!!formState.errors.email}
              role={formState.errors.email ? "alert" : undefined}
            >
              {formState.errors.email?.message ?? ""}
            </FormHelperText>
          </FormControl>

          <FormControl sx={{ height: formControlHeight }}>
            <Controller
              name="status"
              control={control}
              rules={{
                required: customerAdminPageTranslations.addUserDialog.required(
                  customerAdminPageTranslations.addUserDialog.status,
                ),
              }}
              render={({ field }) => (
                <TextField
                  {...field}
                  select
                  size="small"
                  inputProps={{ style: { fontSize: 14 } }}
                  InputLabelProps={{ style: { fontSize: 14 } }}
                  label={customerAdminPageTranslations.addUserDialog.status}
                  variant="outlined"
                >
                  {status.map((option) => (
                    <MenuItem key={option.value} value={option.value}>
                      <Box height="22px" display="flex" alignItems="center">
                        <Typography variant="body1" height="12px">
                          {option.label}
                        </Typography>
                      </Box>
                    </MenuItem>
                  ))}
                </TextField>
              )}
            />
            <FormHelperText
              error={!!formState.errors.status}
              role={formState.errors.status ? "alert" : undefined}
            >
              {formState.errors.status?.message ?? ""}
            </FormHelperText>
          </FormControl>
          <FormControl sx={{ height: formControlHeight }}>
            <Controller
              name="role"
              control={control}
              rules={{
                required: customerAdminPageTranslations.addUserDialog.required(
                  customerAdminPageTranslations.addUserDialog.userRole,
                ),
              }}
              render={({ field }) => (
                <TextField
                  {...field}
                  select
                  size="small"
                  inputProps={{ style: { fontSize: 14 } }}
                  InputLabelProps={{ style: { fontSize: 14 } }}
                  label={customerAdminPageTranslations.addUserDialog.userRole}
                  variant="outlined"
                >
                  {userRole.map((option) => (
                    <MenuItem key={option.value} value={option.value}>
                      <Box height="22px" display="flex" alignItems="center">
                        <Typography variant="body1" height="12px">
                          {option.label}
                        </Typography>
                      </Box>
                    </MenuItem>
                  ))}
                </TextField>
              )}
            />
            <FormHelperText
              error={!!formState.errors.role}
              role={formState.errors.role ? "alert" : undefined}
            >
              {formState.errors.role?.message ?? ""}
            </FormHelperText>
          </FormControl>
        </Stack>

        <Stack position="relative" direction="column" width="100%" gap={4}>
          <Backdrop
            open={role === "Admin"}
            sx={{
              color: (theme) => theme.palette.common.white,
              zIndex: (theme) => theme.zIndex.modal + 1,
              position: "absolute",
              height: "420px",
              borderRadius: "10px",
            }}
          >
            <Box width="200px">
              <Typography color={"white"}>
                {
                  customerAdminPageTranslations.addUserDialog
                    .disabledCustomerSelection
                }
              </Typography>
            </Box>
          </Backdrop>
          <Paper
            style={{
              width: "100%",
              height: "345px",
              overflow: "auto",
              border: "1px solid #CCCCCC",
              background: "white",
              padding: 8,
              position: "relative",
            }}
          >
            <Backdrop
              open={addUserProps.isLoadingCustomerPermissions}
              sx={{
                color: (theme) => theme.palette.common.white,
                zIndex: (theme) => theme.zIndex.modal + 1,
                position: "absolute",
                borderRadius: "10px",
              }}
            >
              <CircularProgress sx={{ color: "white" }} />
            </Backdrop>

            <DebouncedStandardTextfield
              label={customerAdminPageTranslations.addUserDialog.customerSearch}
              startValue=""
              onUpdate={addUserProps.filterCustomers}
            />
            <FormHelperText
              sx={{ ml: 2 }}
              error={!!errorCustomerTree}
              role={errorCustomerTree ? "alert" : undefined}
            >
              {errorCustomerTree ?? ""}
            </FormHelperText>
            <Box height="1rem" />
            <CustomerAccessTreeView
              data={addUserProps.filteredCustomers}
              selectedNodes={addUserProps.selectedNodes}
              setSelectedNodes={addUserProps.setSelectedNodes}
            />
          </Paper>
        </Stack>
      </Stack>
      <Box
        display="flex"
        width="100%"
        flexDirection="row"
        justifyContent="end"
        alignItems="center"
        mt={4}
      >
        {addUserProps.apiError && (
          <Typography variant="error">
            {addUserProps.apiError.detail}
          </Typography>
        )}
      </Box>
    </OperationDialog>
  );
};
