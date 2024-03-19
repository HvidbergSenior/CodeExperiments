// Types

import {
  Address,
  Declarationinfo,
  PdfReportPosResponse,
  Recipient,
  Renewablefuelsupplier,
  Scopeofcertificationandghgemission,
} from "../../../../../api/api";
import { formatDate } from "../../../../../util/formatters/formatters";

export type RecipientMock = {
  address: AddressMock;
  periodFrom: Date;
  periodTo: Date;
};

export type AddressMock = {
  name: string;
  street: string;
  streetNumber: number;
  zipCode: number;
  city: string;
  country: string;
};

export type RenewableFuelSupplierMock = {
  address: AddressMock;
  certificateSystem: string;
  certificateNumber: string;
};

export type RenewableFuel = {
  volume: number;
  product: string;
  energyContent: number;
};

export type ScopeOfCertificationAndGHGEmissionMock = {
  euRedCompliantMaterial: string;
  isccCompliantMaterial: string;
  chainOfCustodyOption: string;
  totalDefaultValueAccordingToRed2Applied: string;
};

export type RawMaterialSustainability = {
  rawMaterial: string;
  countryOfOrigin: string;
  productionCountry: string;
  dateOfInstallation: string;
};

export type ScopeOfCertificationOfRawMaterial = {
  option1: boolean;
  option2: boolean;
  option3: boolean;
  option4: boolean;
  option5: string;
};

export type LifeCycleGreenhouseGasEmissions = {
  extractionOrCultivation: number;
  landUse: number;
  processing: number;
  transportAndDistribution: number;
  fuelInUse: number;
  soilCarbonAccumulation: number;
  carbonCaptureAndGeologicalStorage: number;
  carbonCaptureAndReplacement: number;
  totalGHGEmissionFromSupplyAndUseOfFuel: number;
};

export type GreenhouseGasEmissionsSavings = {
  ghgPercent: number;
};

// Data

const declarationinfo: Declarationinfo = {
  id: "BFE_AB_20220901_20220931_084527432",
  dateOfIssuance: "2023-09-17T03:24:00",
};

const recipientAddress: Address = {
  name: "<Customer name>",
  street: "<Address>",
  streetNumber: "100",
  zipCode: "2000",
  city: "<City>",
  country: "<Country>",
};

const recipient: Recipient = {
  address: recipientAddress,
  periodFrom: formatDate(new Date()),
  periodTo: formatDate(new Date()),
};

const supplierAddress: Address = {
  name: "Biofuel Express AB",
  street: "Mariebergsgatan",
  streetNumber: "6",
  zipCode: "26151",
  city: "Landskrona",
  country: "Sverige",
};

const renewablefuelsupplier: Renewablefuelsupplier = {
  address: supplierAddress,
  certificateSystem: "ISCC EU",
  certificateNumber: "EU-ISCC-Cert-DKxxx-xxxx",
};

const renewablefuel: RenewableFuel = {
  volume: 43418,
  product: "B100 Biodiesel RME",
  energyContent: 8867,
};

const scopeOfCertificationAndGHGEmission: Scopeofcertificationandghgemission = {
  euRedCompliantMaterial: false,
  isccCompliantMaterial: true,
  chainOfCustodyOption: "someValue",
  totalDefaultValueAccordingToRed2Applied: false,
};

const rawMaterialSustainability: RawMaterialSustainability = {
  rawMaterial: "Rapeseed",
  countryOfOrigin: "Germany",
  productionCountry: "Sweeden",
  dateOfInstallation: "2007",
};

const scopeOfCertificationOfRawMaterial: ScopeOfCertificationOfRawMaterial = {
  option1: true,
  option2: false,
  option3: false,
  option4: true,
  option5: "someValue",
};

const lifeCycleGreenhouseGasEmissions: LifeCycleGreenhouseGasEmissions = {
  extractionOrCultivation: 24.74,
  landUse: 0,
  processing: 8.81,
  transportAndDistribution: 1.8,
  fuelInUse: 0,
  soilCarbonAccumulation: 0,
  carbonCaptureAndGeologicalStorage: 0,
  carbonCaptureAndReplacement: 0,
  totalGHGEmissionFromSupplyAndUseOfFuel: 35.35,
};

const greenhouseGasEmissionsSavings: GreenhouseGasEmissionsSavings = {
  ghgPercent: 76,
};

export const proofOfSustainabilityDTO: PdfReportPosResponse = {
  declarationinfo,
  recipient,
  renewablefuelsupplier,
  renewablefuel,
  scopeofcertificationandghgemission: scopeOfCertificationAndGHGEmission,
  rawmaterialsustainability: rawMaterialSustainability,
  scopeofcertificationofrawmaterial: scopeOfCertificationOfRawMaterial,
  lifecyclegreenhousegasemissions: lifeCycleGreenhouseGasEmissions,
  greenhousegasemissionssavings: greenhouseGasEmissionsSavings,
};
