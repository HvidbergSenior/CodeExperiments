import CheckIcon from "@mui/icons-material/Check";
import CircleIcon from "@mui/icons-material/Circle";
import { Typography } from "@mui/material";
import { CustomerPortalUserRowMenu } from "./user-row-menu";
import { AllUserResponse } from "../../../../api/api";
import { VirtualTableColumnDef } from "../../../../components/types";
import { customerAdminPageTranslations } from "../../../../translations/pages/customer-admin-page-translations";
import { theme } from "../../../../theme/theme";

export type AllUserResponseKeys = keyof AllUserResponse | "contextMenu";

const columnIdentifiers: Record<AllUserResponseKeys, string> = {
  userName: "userName",
  firstName: "firstName",
  lastName: "lastName",
  email: "email",
  userType: "userType",
  blocked: "blocked",
  hasFleetManagementAccess: "hasFleetManagementAccess",
  hasFuelConsumptionAccess: "hasFuelConsumptionAccess",
  hasSustainabilityReportAccess: "hasSustainabilityReportAccess",
  userId: "userId",
  contextMenu: "contextMenu",
};

export const userTableCustomerPortalColumns: Array<
  VirtualTableColumnDef<AllUserResponse>
> = [
  {
    key: columnIdentifiers.firstName,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header: customerAdminPageTranslations.nameColumnHeader,
    cell: ({ firstName, lastName }) => (
      <Typography variant="body2" align="left">
        {`${firstName} ${lastName}`}
      </Typography>
    ),
  },

  {
    key: columnIdentifiers.blocked,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header: customerAdminPageTranslations.statusColumnHeader,
    cell: ({ blocked }) => (
      <>
        <CircleIcon
          sx={{ fontSize: 10 }}
          htmlColor={
            blocked ? theme.palette.Red3.main : theme.palette.primary.main
          }
        />
        <Typography ml={2} variant="body2" align="left">
          {blocked
            ? customerAdminPageTranslations.statusBlocked
            : customerAdminPageTranslations.statusActive}
        </Typography>
      </>
    ),
  },
  {
    key: columnIdentifiers.email,
    isSortable: true,
    minWidth: "50px",
    width: "100%",
    header: customerAdminPageTranslations.emailColumnHeader,
    cell: ({ email }) => (
      <Typography variant="body2" align="left">
        {email}
      </Typography>
    ),
  },
  {
    key: columnIdentifiers.userType,
    isSortable: false,
    minWidth: "50px",
    width: "100%",
    header: customerAdminPageTranslations.userTypeColumnHeader,
    cell: ({ userType }) => (
      <Typography variant="body2" align="left">
        {userType === "Admin"
          ? customerAdminPageTranslations.userRoleAdmin
          : customerAdminPageTranslations.userRoleUser}
      </Typography>
    ),
  },
  {
    key: columnIdentifiers.hasFuelConsumptionAccess,
    isSortable: false,
    minWidth: "50px",
    width: "100%",
    header: customerAdminPageTranslations.accessFuelConsumptionColumnHeader,
    cell: ({ hasFuelConsumptionAccess }) => (
      <CheckMarkCell isChecked={hasFuelConsumptionAccess} />
    ),
  },
  {
    key: columnIdentifiers.hasSustainabilityReportAccess,
    isSortable: false,
    minWidth: "50px",
    width: "100%",
    header:
      customerAdminPageTranslations.accessSustainabilityReportColumnHeader,
    cell: ({ hasSustainabilityReportAccess }) => (
      <CheckMarkCell isChecked={hasSustainabilityReportAccess} />
    ),
  },
  {
    key: columnIdentifiers.hasFleetManagementAccess,
    isSortable: false,
    minWidth: "50px",
    width: "100%",
    header: customerAdminPageTranslations.accessFleetManagementColumnHeader,
    cell: ({ hasFleetManagementAccess }) => (
      <CheckMarkCell isChecked={hasFleetManagementAccess} />
    ),
  },
  {
    key: columnIdentifiers.contextMenu,
    minWidth: "60px",
    width: "60px",
    isSortable: false,
    header: "",
    headerProps: { padding: "none", sortDirection: false },
    cell: (user) => <CustomerPortalUserRowMenu user={user} />,
    cellProps: {
      padding: "none",
      onClick: (event) => event.stopPropagation(),
    },
  },
];

function CheckMarkCell({ isChecked }: { isChecked: boolean }) {
  if (isChecked) {
    return <CheckIcon fontSize="small"></CheckIcon>;
  } else {
    return <div></div>;
  }
}
