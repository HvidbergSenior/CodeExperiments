import { HttpResponse, delay, http } from "msw";
import {
  GetIncomingDeclarationsByPageAndPageSizeResponse,
  IncomingDeclarationResponse,
} from "../api/api";
const AMOUNT_OF_DUMMY_DATA = 500;

export const InterceptDeclarationReconciliation = http.get(
  "https://localhost:7084/api/incomingdeclarations/reconciled",
  async ({ request }) => {
    // Uncomment to get page number and page size and add { request } in the above function's param
    const url = new URL(request.url);
    const page = Number.parseInt(url.searchParams.get("page") ?? "1");
    const pageSize = Number.parseInt(url.searchParams.get("pageSize") ?? "50");
    const declarations: GetIncomingDeclarationsByPageAndPageSizeResponse = {
      hasMoreDeclarations: pageSize * page < AMOUNT_OF_DUMMY_DATA,
      totalAmountOfDeclarations: 100,
      incomingDeclarationsByPageAndPageSize: generateDummyDeclerations().slice(
        0,
        page * pageSize,
      ),
    };
    await delay(500);
    return HttpResponse.json(declarations);
  },
);

const generateDummyDeclerations = () => {
  const result: IncomingDeclarationResponse[] = [];
  for (let i = 1; i <= 500; i++) {
    result.push({
      company: `company${i}`,
      country: `country${i}`,
      product: `product${i}`,
      supplier: `supplier${i}`,
      rawMaterial: `rawMaterial${i}`,
      id: `id${i}`,
      posNumber: `posNumber${i}`,
      countryOfOrigin: `countryOfOrigin${i}`,
      incomingDeclarationState: `Reconciled`,
      ghgEmissionSaving: 1000 + i,
      dateOfDispatch: `period${i}`,
      quantity: 1000 + i,
      placeOfDispatch: `storage${i}`,
      remainingVolume: 1000 + i,
    });
  }
  return result;
};

export const InterceptReconcileSelectedDeclarations = http.post(
  "https://localhost:7084/api/incomingdeclarations/reconcile",
  async () => {
    await delay(500);
    return HttpResponse.json(null, {
      status: 200,
      statusText: "Success",
    });
  },
);
