import { HttpResponse, delay, http } from "msw";
import {
  GetStockTransactionsQueryResponse, StockTransactionResponse,
} from "../api/api";

const AMOUNT_OF_DUMMY_DATA = 500;

export const InterceptStockTransactions = http.get(
  "https://localhost:7084/api/stocktransactions",
  async ({ request }) => {
    // Uncomment to get page number and page size and add { request } in the above function's param
    const url = new URL(request.url);
    const page = Number.parseInt(url.searchParams.get("page") ?? "1");
    const pageSize = Number.parseInt(url.searchParams.get("pageSize") ?? "50");
    const declarations: GetStockTransactionsQueryResponse = {
      hasMoreStockTransactions: pageSize * page < AMOUNT_OF_DUMMY_DATA,
      totalAmountOfStockTransactions: 100,
      stockTransactions: generateDummyStockTransactions().slice(
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

const generateDummyStockTransactions = () => {
  const result: StockTransactionResponse[] = [];
  for (let i = 1; i <= 500; i++) {
    result.push({
      id: `id${i}`,
      country: `country${i}`,
      companyId: `companyId${i}`,
      companyName: `companyName${i}`,
      productNumber: `productNumber${i}`,
      productName: `productName${i}`,
      quantity: 1000 + i,
      location: `location${i}`,
      allocatedQuantity: 1000 + i,
      alreadyAllocatedPercentage: 50 + i,
      missingAllocationQuantity: 1000 + i,
      locationId: `locationId${i}`,
    });
  }
  return result;
};