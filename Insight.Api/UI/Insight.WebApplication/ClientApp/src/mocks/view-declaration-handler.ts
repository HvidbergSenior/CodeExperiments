import { IncomingDeclarationDto } from "./../api/api";
/* https://localhost:7084/api/incomingdeclaration/declaration?id=id1 */

import { HttpResponse, delay, http } from "msw";

export const InterceptViewDeclaration = http.get(
  "https://localhost:7084/api/incomingdeclarations/*",
  async () => {
    await delay(500);
    const response: IncomingDeclarationDto = {
      additionalInformation: "mock data",
      certificationSystem: "mock data",
      company: "mock data",
      complianceWithSustainabilityCriteria: true,
      country: "mock data",
      countryOfOrigin: "mock data",
      cultivatedAsIntermediateCrop: true,
      dateOfDispatch: "mock data",
      dateOfInstallation: "mock data",
      dateOfIssuance: "mock data",
      declarationRowNumber: 1232,
      energyContentMJ: 123,
      energyQuantityGJ: 1223,
      fossilFuelComparatorgCO2EqPerMJ: 1232,
      fulfillsMeasuresForLowILUCRiskFeedstocks: false,
      ghgEccr: 123,
      ghgEccs: 123,
      ghgEec: 123,
      ghgEee: 1232,
      ghgEl: 123,
      ghgEmissionSaving: 123,
      ghgEp: 123,
      ghgEsca: 123,
      ghgEtd: 123,
      ghgEu: 123,
      ghGgCO2EqPerMJ: 123,
      incomingDeclarationState: "Reconciled",
      incomingDeclarationUploadId: "mock data",
      incomingDeclarationId: "mock data",
      meetsDefinitionOfWasteOrResidue: true,
      placeOfDispatch: "mock data",
      posNumber: "mock data",
      product: "mock data",
      productionCountry: "mock data",
      quantity: 123,
      rawMaterial: "mock data",
      specifyNUTS2Region: "mock data",
      supplier: "mock data",
      supplierCertificateNumber: "mock data",
      totalDefaultValueAccordingToREDII: true,
      typeOfProduct: "mock data",
      unitOfMeasurement: "Litres",
      remainingVolume: 1234,
    };
    await delay(500);
    return HttpResponse.json(response);
  },
);
