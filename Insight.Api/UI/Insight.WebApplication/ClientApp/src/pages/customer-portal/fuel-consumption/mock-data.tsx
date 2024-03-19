import {
  Emissionsstats,
  FuelConsumptionNameValuePair,
  FuelConsumptionSeries,
} from "../../../api/api";

export type GeneralFuelTypes = "Renewable fuel" | "Fossil fuel";

export type FuelType = "B100" | "HVO" | "Diesel" | "Petrol";

type ConsumptionPerProduct = {
  data: FuelConsumptionNameValuePair[];
};

const consumptionPerProduct: ConsumptionPerProduct = {
  data: [
    {
      productNameEnumeration: "Diesel",
      value: 1095,
    },
    {
      productNameEnumeration: "B100",
      value: 695,
    },
    {
      productNameEnumeration: "Hvo100",
      value: 555,
    },
    {
      productNameEnumeration: "Petrol",
      value: 900,
    },
  ],
};

type ConsumptionDevelopment = {
  series: FuelConsumptionSeries[];
  categories: string[];
};

const consumptionDevelopment: ConsumptionDevelopment = {
  series: [
    {
      productNameEnumeration: "B100",
      data: [
        4400, 5500, 4100, 6700, 2200, 4300, 4400, 5500, 4100, 6070, 202, 4300,
      ],
    },
    {
      productNameEnumeration: "Hvo100",
      data: [
        1030, 2300, 2000, 800, 1300, 2070, 1003, 2003, 2000, 800, 1300, 2007,
      ],
    },
    {
      productNameEnumeration: "Diesel",
      data: [
        1010, 1070, 1050, 1050, 2010, 1040, 1010, 1070, 1050, 1050, 2001, 1004,
      ],
    },
    {
      productNameEnumeration: "Petrol",
      data: [
        1731, 1752, 1576, 1551, 2261, 1415, 1214, 1170, 1050, 1050, 2100, 1004,
      ],
    },
  ],
  categories: [
    "OCT '19",
    "NOV '19",
    "DEC '19",
    "JAN '20",
    "FEB '20",
    "MAR '20",
    "APR '20",
    "MAY '20",
    "JUN '20",
    "JUL '20",
    "AUG '20",
    "SEP '20",
  ],
};

type FuelConsumptionDTO = {
  consumptionPerProduct: ConsumptionPerProduct;
  consumptionDevelopment: ConsumptionDevelopment;
};

export const fuelConsumptionDTO: FuelConsumptionDTO = {
  consumptionPerProduct,
  consumptionDevelopment,
};

type ConsumptionStats = {
  generalFuelTypes: GeneralFuelTypes[];
  data: number[];
  totalConsumptionFossilFuels: number;
  totalConsumptionRenewableFuels: number;
  consumptionTotalForCircle: number;
  totalConsumptionAllFuels: number;
};

const consumptionStats: ConsumptionStats = {
  data: [18350, 5350],
  generalFuelTypes: ["Renewable fuel", "Fossil fuel"],
  totalConsumptionFossilFuels: 5350,
  totalConsumptionRenewableFuels: 18350,
  totalConsumptionAllFuels: 23700,

  consumptionTotalForCircle: 77,
};

const emissionsStats: Emissionsstats = {
  achievedEmissionReductions: 283750,
  netEmission: 100000,
  emissionSavingsForCircle: 76,
};

type OverviewStats = {
  consumptionStats: ConsumptionStats;
  emissionsStats: Emissionsstats;
  username: string;
  date: string;
  pageForFurtherInformation: number;
};

export const overviewStats: OverviewStats = {
  consumptionStats,
  emissionsStats,
  username: "Volvo-User",
  date: new Date().toLocaleDateString(),
  pageForFurtherInformation: 10,
};
