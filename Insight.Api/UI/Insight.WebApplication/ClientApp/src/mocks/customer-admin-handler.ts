import { HttpResponse, delay, http } from "msw";
import { GetAllUsersResponse } from "../api/api";

export const InterceptAdmininistrationUsers = http.get(
  "https://localhost:7084/api/administrationusers",
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
          firstName: "Jens",
          lastName: "Jensen",
          userType: "User",
        },
      ],
    };
    await delay(500);
    return HttpResponse.json(response);
  },
);
