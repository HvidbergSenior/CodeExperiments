import { HttpResponse, delay, http } from "msw";
import {
  GetFuelConsumptionResponse,
  GetFuelConsumptionTransactionsResponse,
} from "../api/api";

export const InterceptFuelConsumption = http.post(
  "https://localhost:7084/api/outgoingdeclarations/fuelconsumption",
  async () => {
    const response: GetFuelConsumptionResponse = {
      consumptionDevelopment: {
        categories: ["test 1", "test 2"],
        series: [{ data: [10, 20], productNameEnumeration: "B100" }],
      },
      consumptionPerProduct: {
        data: [
          { productNameEnumeration: "Diesel", value: 30345 },
          { productNameEnumeration: "B100", value: 7034 },
        ],
      },
      consumptionStats: {
        consumptionTotalForCircle: 70,
        data: [70, 30],
        generalFuelTypes: ["Renewable fuel", "Fossil fuel"],
        totalConsumptionAllFuels: 100,
        totalConsumptionFossilFuels: 30,
        totalConsumptionRenewableFuels: 70,
      },
    };

    await delay(500);
    return HttpResponse.json(response);

    /*  const errorResponse: api.Error = {
      detail: "These are the mocked details",
      instance: "instance mock",
      status: 404,
      title: "This is mock error title",
      type: "error mock",
      traceId: "mock-trace-id",
    }; */
    /*  return HttpResponse.json(errorResponse, {
      status: 404,
      statusText: "Out Of Apples",
    }); */
  },
);

export const InterceptFuelConsumptionTransactions = http.post(
  "https://localhost:7084/api/outgoingdeclarations/fuelconsumptiontransactions",
  async () => {
    const response: GetFuelConsumptionTransactionsResponse = {
      hasMoreTransactions: false,
      totalAmountOfTransactions: 100,
      data: [
        {
          cardNumber: "1234",
          customerName: "customerName",
          customerNumber: "customerNumber",
          date: new Date().toLocaleDateString(),
          id: "id",
          quantity: 10,
          productName: "productName",
          productNumber: "productNumber",
          stationId: "stationId",
          stationName: "stationName",
          time: "time",
          location: "location",
        },
      ],
    };
    await delay(500);
    return HttpResponse.json(response);
  },
);
