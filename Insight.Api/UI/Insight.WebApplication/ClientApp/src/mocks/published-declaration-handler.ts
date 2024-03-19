import { HttpResponse, delay, http } from "msw";
import {
  GetOutgoingDeclarationByIdResponse,
  GetOutgoingDeclarationsByPageAndPageSizeResponse,
  OutgoingDeclarationResponse,
} from "../api/api";

const AMOUNT_OF_DUMMY_DATA = 500;

export const InterceptPublishedDeclarations = http.get(
  "https://localhost:7084/api/outgoingdeclarations/pagination",
  async ({ request }) => {
    // Uncomment to get page number and page size and add { request } in the above function's param
    const url = new URL(request.url);
    const page = Number.parseInt(url.searchParams.get("page") ?? "1");
    const pageSize = Number.parseInt(url.searchParams.get("pageSize") ?? "50");
    const declarations: GetOutgoingDeclarationsByPageAndPageSizeResponse = {
      hasMoreDeclarations: pageSize * page < AMOUNT_OF_DUMMY_DATA,
      totalAmountOfDeclarations: 100,
      outgoingDeclarationsByPageAndPageSizeResponse:
        generateDummyDeclarations().slice(0, page * pageSize),
    };
    await delay(500);
    return HttpResponse.json(declarations);
    /* return HttpResponse.json(null, {
      status: 404,
      statusText: "Out Of Apples",
    }); */
  },
);

const generateDummyDeclarations = () => {
  const result: OutgoingDeclarationResponse[] = [];
  for (let i = 1; i <= 500; i++) {
    result.push({
      country: `country${i}`,
      product: `B100`,
      customerName: `customerName${i}`,
      customerNumber: `customerNumber${i}`,
      ghgReduction: 0.5 + (i % 10) * 0.05,
      volumeTotal: i,
      allocationTotal: 0,
      outgoingDeclarationId: "-",
      fossilFuelComparatorgCO2EqPerMJ: 1234,
      incomingDeclarationIds: [],
    });
  }
  return result;
};

export const InterceptViewPublishedDetails = http.get(
  "https://localhost:7084/api/outgoingdeclarations/-",
  async () => {
    const declarations: GetOutgoingDeclarationByIdResponse = {
      outgoingDeclarationByIdResponse: {
        country: "country",
        customerName: "customerName",
        customerNumber: "customerNumber",
        outgoingDeclarationId: "declarationId",
        product: "HVO100",
        additionalInformation: "additionalInformation_mock",
        bfeId: "bfeId_mock",
        certificateId: "certificateId_mock",
        dateOfIssuance: "dateOfIssuance_mock",
        density: 12345,
        emissionSavingControl: 12345,
        energyContent: 1234,
        energyContentControl: 1234,
        greenhouseGasEmission: 1234,
        greenhouseGasReduction: 1234,
        liter: 1234,
        mt: 1234,
        productionCountry: "productionCountry_mock",
        rawMaterialCode: "rawMaterialCode_mock",
        rawMaterialName: "rawMaterialName_mock",
        sustainabilityDeclarationNumber: "sustainabilityDeclarationNumber_mock",        
      },
    };
    await delay(500);
    return HttpResponse.json(declarations);
  },
);
