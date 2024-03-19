import { HttpResponse, delay, http } from "msw";
import {
  CustomerPermission,
  CustomerPermissionDto,
  GetAvailableCustomersPermissionsResponse,
} from "../api/api";

const AMOUNT_OF_DUMMY_DATA = 150;

export const InterceptCustomersPermissions = http.get(
  "https://localhost:7084/api/customers/permissions",
  async ({ request }) => {
    // Uncomment to get page number and page size and add { request } in the above function's param
    const url = new URL(request.url);
    const page = Number.parseInt(url.searchParams.get("page") ?? "1");
    const pageSize = Number.parseInt(url.searchParams.get("pageSize") ?? "50");
    const response: GetAvailableCustomersPermissionsResponse = {
      customerNodes: generateDummyCustomersPermissions().slice(
        (page - 1) * pageSize,
        page * pageSize,
      ),
    };
    await delay(500);
    return HttpResponse.json(response);
    /* return HttpResponse.json(null, {
      status: 404,
      statusText: "Out Of Apples",
    }); */
  },
);

const generateDummyCustomersPermissions = () => {
  const result: CustomerPermissionDto[] = [];
  for (let i = 1; i <= AMOUNT_OF_DUMMY_DATA; i++) {
    const permissions: CustomerPermission[] = [];
    permissions.push("Admin");
    if (i % 20 === 0) {
      permissions.push("FuelConsumption");
    }
    permissions.push("SustainabilityReport");
    permissions.push("FleetManagement");

    result.push({
      customerId: `id${i}`,
      customerName: `customerName ${i}`,
      customerNumber: `customerNumber ${i}`,
      children: [],
      parentCustomerId: `parent ${i}`,
      permissions: permissions,
    });
  }
  return result;
};
