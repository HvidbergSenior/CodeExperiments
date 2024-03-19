import { HttpResponse, delay, http } from "msw";
import {
  CustomerPermission,
  GetPossibleCustomerPermissionsResponse,
} from "../api/api";

export const InterceptPossiblePermissions = http.get(
  "https://localhost:7084/api/customers/possiblepermissions",
  async ({}) => {
    const allPermissions: CustomerPermission[] = [
      "Admin",
      "FuelConsumption",
      "SustainabilityReport",
      "FleetManagement",
    ];
    const response: GetPossibleCustomerPermissionsResponse = {
      customerNodes: [
        {
          customerId: "1",
          customerName: "Frode Lauersen",
          customerNumber: "123-1",
          parentCustomerId: "",
          permissions: allPermissions,
          children: [
            {
              customerId: "11",
              customerName: "Station One",
              customerNumber: "123-1-1",
              parentCustomerId: "1",
              permissions: allPermissions,
              children: [],
            },
            {
              customerId: "12",
              customerName: "Station Two",
              customerNumber: "123-1-2",
              parentCustomerId: "1",
              permissions: allPermissions,
              children: [],
            },
          ],
        },
        {
          customerId: "2",
          customerName: "STARK",
          customerNumber: "123-2",
          parentCustomerId: "",
          permissions: allPermissions,
          children: [
            {
              customerId: "21",
              customerName: "STARK Viby",
              customerNumber: "123-2-1",
              parentCustomerId: "2",
              permissions: allPermissions,
              children: [],
            },
            {
              customerId: "22",
              customerName: "STARK Risskov",
              customerNumber: "123-2-2",
              parentCustomerId: "2",
              permissions: allPermissions,
              children: [],
            },
            {
              customerId: "23",
              customerName: "STARK Randers",
              customerNumber: "123-2-3",
              parentCustomerId: "2",
              permissions: allPermissions,
              children: [],
            },
          ],
        },
        {
          customerId: "3",
          customerName: "Frode F",
          customerNumber: "123-3",
          parentCustomerId: "",
          permissions: allPermissions,
          children: [
            {
              customerId: "31",
              customerName: "Frode One",
              customerNumber: "123-3-1",
              parentCustomerId: "3",
              permissions: allPermissions,
              children: [
                {
                  customerId: "311",
                  customerName: "Frode One A",
                  customerNumber: "123-3-1-1",
                  parentCustomerId: "31",
                  permissions: allPermissions,
                  children: [],
                },
                {
                  customerId: "312",
                  customerName: "Frode One B",
                  customerNumber: "123-3-1-2",
                  parentCustomerId: "31",
                  permissions: allPermissions,
                  children: [],
                },
              ],
            },
            {
              customerId: "32",
              customerName: "Frode Two",
              customerNumber: "123-3-2",
              parentCustomerId: "3",
              permissions: allPermissions,
              children: [],
            },
          ],
        },
      ],
    };
    await delay(500);
    return HttpResponse.json(response);
    /* return HttpResponse.json(null, {
      status: 404,
      statusText: "Out Of Apples",
    }); */
  },
);