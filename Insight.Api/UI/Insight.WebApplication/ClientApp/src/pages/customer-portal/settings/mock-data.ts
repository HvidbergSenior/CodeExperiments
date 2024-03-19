// Define the enum types
enum StatusEnum {
  Active = "Active",
  Inactive = "Inactive",
}

enum UserRole {
  Admin = "Admin",
  User = "User",
}

// Define the TableRowDTO type
export type TableRowDTO = {
  userId: string;
  userName: string;
  accountName: string;
  accountNumber: string;
  status: StatusEnum;
  email: string;
  userType: UserRole;
  hasFuelConsumptionAccess: boolean;
  hasSustainabilityReportAccess: boolean;
  hasFleetManagementAccess: boolean;
};

// Function to generate an array of TableRowDTO objects
export function generateTableRows(x: number): TableRowDTO[] {
  const tableRows: TableRowDTO[] = [];

  for (let i = 0; i < x; i++) {
    const row: TableRowDTO = {
      userId: `user${i + 1}`,
      userName: `User ${i + 1}`,
      accountName: `Account ${i + 1}`,
      accountNumber: `ACC-${i + 1}`,
      status: i % 2 === 0 ? StatusEnum.Active : StatusEnum.Inactive,
      email: `user${i + 1}@example.com`,
      userType: i % 2 === 0 ? UserRole.Admin : UserRole.User,
      hasFuelConsumptionAccess: i % 2 === 0,
      hasSustainabilityReportAccess: i % 3 === 0,
      hasFleetManagementAccess: i % 4 === 0,
    };

    tableRows.push(row);
  }

  return tableRows;
}
