import { HttpResponse, delay, http } from "msw";
import {
  GetAllocationSuggestionsResponse,
  IncomingDeclarationState,
  SuggestionResponse,
} from "../api/api";

const AMOUNT_OF_DUMMY_DATA = 500;

export const InterceptAllocationSuggestions = http.get(
  "https://localhost:7084/api/allocations/suggestions",
  async ({ request }) => {
    // Uncomment to get page number and page size and add { request } in the above function's param
    const url = new URL(request.url);
    const page = Number.parseInt(url.searchParams.get("page") ?? "1");
    const pageSize = Number.parseInt(url.searchParams.get("pageSize") ?? "50");
    const allocations: GetAllocationSuggestionsResponse = {
      hasMoreSuggestions: pageSize * page < AMOUNT_OF_DUMMY_DATA,
      totalAmountOfSuggestions: 100,
      suggestions: generateDummyAllocationSuggestions().slice(
        0,
        page * pageSize,
      ),
    };
    await delay(500);
    return HttpResponse.json(allocations);
    /* return HttpResponse.json(null, {
      status: 404,
      statusText: "Out Of Apples",
    }); */
  },
);

const generateDummyAllocationSuggestions = () => {
  const result: SuggestionResponse[] = [];
  for (let i = 1; i <= 500; i++) {
    result.push({
      id: `id${i}`,
      posNumber: `posNumber${i}`,
      period: `period${i}`,
      storage: `storage${i}`,
      company: `company${i}`,
      product: `product${i}`,
      rawMaterial: `rawMaterial${i}`,
      supplier: `supplier${i}`,
      countryOfOrigin: `countryOfOrigin${i}`,
      incomingDeclarationState: `Reconciled` as IncomingDeclarationState,
      volumeAvailable: 100 * i,
      volume: 100 * i,
      ghgReduction: 1000 + i,
      hasWarnings: i % 3 === 0 ? true : false,
      warnings: i % 3 === 0 ? [`warning${i}`] : [],
      country: `country${i}`
    });
  }
  return result;
};
