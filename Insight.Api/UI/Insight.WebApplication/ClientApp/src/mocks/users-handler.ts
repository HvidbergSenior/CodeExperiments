import { HttpResponse, delay, http } from "msw";
import { GetAllUsersResponse } from "../api/api";

export const InterceptUsers = http.get(
  "https://localhost:7084/api/users",
  async () => {
    const response: GetAllUsersResponse = {
      hasMoreUsers: false,
      totalAmountOfUsers: 50,
      users: [
        {
          blocked: false,
          email: "jens@arriva.nu",
          hasFleetManagementAccess: true,
          hasFuelConsumptionAccess: true,
          hasSustainabilityReportAccess: true,
          userId: "12345",
          userName: "Jens1234",
          userType: "User",
          firstName: "Jens",
          lastName: "Jensen",
        },
      ],
    };
    await delay(500);
    return HttpResponse.json(response);
  },
);
