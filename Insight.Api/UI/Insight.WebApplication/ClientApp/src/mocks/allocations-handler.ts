import { HttpResponse, delay, http } from "msw";
import { AllocationResponse, Error, GetAllocationsResponse } from "../api/api";

const AMOUNT_OF_DUMMY_DATA = 500;

export const InterceptAllocations = http.get(
  "https://localhost:7084/api/allocations",
  async ({ request }) => {
    // Uncomment to get page number and page size and add { request } in the above function's param
    const url = new URL(request.url);
    const page = Number.parseInt(url.searchParams.get("page") ?? "1");
    const pageSize = Number.parseInt(url.searchParams.get("pageSize") ?? "50");
    const allocations: GetAllocationsResponse = {
      hasMoreAllocations: pageSize * page < AMOUNT_OF_DUMMY_DATA,
      totalAmountOfAllocations: 100,
      allocations: generateDummyAllocations().slice(0, page * pageSize),
      isDraftLocked: false,
    };
    await delay(500);
    return HttpResponse.json(allocations);
    /* return HttpResponse.json(null, {
      status: 404,
      statusText: "Out Of Apples",
    }); */
  },
);

const generateDummyAllocations = () => {
  const result: AllocationResponse[] = [];
  for (let i = 1; i <= 500; i++) {
    result.push({
      id: `id${i}`,
      posNumber: `posNumber${i}`,
      company: `company${i}`,
      country: `country${i}`,
      product: `product${i}`,
      customer: `customer${i}`,
      storage: `storage${i}`,
      certificationSystem: `certificationSystem${i}`,
      rawMaterial: `rawMaterial${i}`,
      countryOfOrigin: `countryOfOrigin${i}`,
      ghgReduction: 0.5 + (i % 10) * 0.05,
      volume: 1000 + i,
      warnings: [`warning${i}`],
      hasWarnings: true,
      fossilFuelComparatorgCO2EqPerMJ: 94,
      customerNumber: `customerNumber${i}`,
    });
  }
  return result;
};

export const InterceptPublishAllocations = http.post(
  "https://localhost:7084/api/allocations/publish",
  async () => {
    const mockAnError = false;

    if (mockAnError) {
      const response: Error = {
        detail: "Mocked error for publish allocations",
        title: "Mock error",
        instance: "instance",
        status: 404,
        type: "mock",
      };
      await delay(500);
      return HttpResponse.json(response, { status: 404, statusText: "OK" });
    } else {
      await delay(500);
      return HttpResponse.json(null, { status: 200, statusText: "OK" });
    }
  },
);

export const InterceptLockAllocations = http.post(
  "https://localhost:7084/api/allocations/lock",
  async () => {
    const response: Error = {
      detail: "Error while locking allocations",
      status: 404,
      title: "Locking allocations error",
      type: "test",
      instance: "test",
    };
    await delay(500);
    return HttpResponse.json(response, {
      status: 404,
      statusText: "Out Of Apples",
    });
  },
);
