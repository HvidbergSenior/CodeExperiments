import { FuelTypeMock } from "../fuel-consumption/mock-data";
import { GeneralFuelTypes } from "./overview-consumption";

export type OverviewStats = {
  consumptionStats: ConsumptionStats;
  emissionsStats: EmissionsStats;
  username: string;
  date: string;
  pageForFurtherInformation: number;
};

export type ConsumptionStats = {
  generalFuelTypes: GeneralFuelTypes[];
  data: number[];
  totalConsumptionFossilFuels: number;
  totalConsumptionRenewableFuels: number;
  consumptionTotalForCircle: number;
  totalConsumptionAllFuels: number;
};

export type EmissionsStats = {
  fuelType: FuelTypeMock[];
  data: number[];
  totalEmissions: number;
  emissionReduction: number;
  emissionTotalForCircle: number;
};

export const consumptionStats: ConsumptionStats = {
  data: [18350, 5350],
  generalFuelTypes: ["Renewable fuel", "Fossil fuel"],
  totalConsumptionFossilFuels: 5350,
  totalConsumptionRenewableFuels: 18350,
  totalConsumptionAllFuels: 23700,

  consumptionTotalForCircle: 23,
};

export const emissionsStats: EmissionsStats = {
  data: [732, 1464, 2758, 3758],
  fuelType: ["HVO", "B100", "Diesel", "Petrol"],
  totalEmissions: 283750,
  emissionReduction: 100000,
  emissionTotalForCircle: 76,
};

export const overviewStats: OverviewStats = {
  consumptionStats,
  emissionsStats,
  username: "Volvo-User",
  date: new Date().toLocaleDateString(),
  pageForFurtherInformation: 10,
};
