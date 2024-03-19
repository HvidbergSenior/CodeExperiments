import { HttpResponse, delay, http } from "msw";
import {
  GetOutgoingFuelTransactionsQueryResponse,
  OutgoingFuelTransactionResponse,
} from "../api/api";

const AMOUNT_OF_DUMMY_DATA = 500;

export const InterceptOutgoingFuelTransactions = http.get(
  "https://localhost:7084/api/outgoingfueltransactions",
  async ({ request }) => {
    // Uncomment to get page number and page size and add { request } in the above function's param
    const url = new URL(request.url);
    const page = Number.parseInt(url.searchParams.get("page") ?? "1");
    const pageSize = Number.parseInt(url.searchParams.get("pageSize") ?? "50");
    const declarations: GetOutgoingFuelTransactionsQueryResponse = {
      hasMoreOutgoingFuelTransactions: pageSize * page < AMOUNT_OF_DUMMY_DATA,
      totalAmountOfOutgoingFuelTransactions: 100,
      totalQuantity: 500000,
      outgoingFuelTransactions: generateDummyFuelTransactions().slice(
        0,
        page * pageSize,
      ),
    };
    await delay(500);
    return HttpResponse.json(declarations);
    /* return HttpResponse.json(null, {
      status: 404,
      statusText: "Out Of Apples",
    }); */
  },
);

const generateDummyFuelTransactions = () => {
  const result: OutgoingFuelTransactionResponse[] = [];
  for (let i = 1; i <= 500; i++) {
    result.push({
      productNumber: `productNumber${i}`,
      productName: `productName${i}`,
      stationName: `stationName${i}`,
      quantity: i === 1 ? 0 : 1000 + 100 * i,
      customerNumber: `customerNumber${i}`,
      customerName: `customerName${i}`,
      country: `country${i}`,
      id: `id${i}`,
      locationId: `locationId${i}`,
      customerId: `customerId${i}`,
      location: `location${i}`,
      customerType: `customerType${i}`,
      customerSegment: `customerSegment${i}`,
      allocatedQuantity:
        i % 3 == 0 ? 1000 + 100 * i : i % 3 == 1 ? (1000 + 100 * i) / 2 : 0,
      alreadyAllocatedPercentage: i % 3 == 0 ? 50 : i % 3 == 1 ? 33.33 : 0,
      missingAllocationQuantity: i === 1 ? 0 : 1000 + 100 * i,
    });
  }
  return result;
};
