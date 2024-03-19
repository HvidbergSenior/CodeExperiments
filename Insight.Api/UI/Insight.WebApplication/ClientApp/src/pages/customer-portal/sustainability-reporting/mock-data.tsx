type SustainabilityReportingDTO = {
  progress: Progress;
  productSpecifications: ProductSpecificationItem[];
  countryDonut: Country[]; // Should have a maxLength of 13
  feedStocks: FeedStocks[];
  emissionSpeedometer: EmissionSpeedometer;
};

type EmissionSpeedometer = {
  netEmission: number;
  achievedEmissionREductions: number;
  ghgEmissionSaving: number;
};

type Progress = {
  emissions: number[];
  emissionReduction: number[];
  categories: string[];
};

type ProductSpecificationItem = {
  fuelType: string;
  volume: number;
  ghgBaseline: number;
  ghgEmissionSaving: number;
  achievedEmissionReduction: number;
  netEmission: number;
};

const emissionSpeedometer = {
  netEmission: 100000,
  achievedEmissionREductions: 283750,
  ghgEmissionSaving: 76,
};

const productSpecifications: ProductSpecificationItem[] = [
  {
    fuelType: "HVO",
    achievedEmissionReduction: 1234,
    ghgBaseline: 1234,
    ghgEmissionSaving: 1234,
    netEmission: 1234,
    volume: 1234,
  },
  {
    fuelType: "Diesel",
    achievedEmissionReduction: 1234,
    ghgBaseline: 1234,
    ghgEmissionSaving: 1234,
    netEmission: 1234,
    volume: 1234,
  },
  {
    fuelType: "Petrol",
    achievedEmissionReduction: 1234,
    ghgBaseline: 1234,
    ghgEmissionSaving: 1234,
    netEmission: 1234,
    volume: 1234,
  },
  {
    fuelType: "B100",
    achievedEmissionReduction: 1234,
    ghgBaseline: 1234,
    ghgEmissionSaving: 1234,
    netEmission: 1234,
    volume: 1234,
  },
];

const progress: Progress = {
  emissions: [312, 222, 312, 123, 321, 412, 123, 222, 111, 200, 301, 123],
  emissionReduction: [
    123, 301, 200, 111, 222, 123, 412, 321, 123, 312, 222, 312,
  ],
  categories: [
    "1/2021",
    "2/2021",
    "3/2021",
    "4/2021",
    "5/2021",
    "6/2021",
    "7/2021",
    "8/2021",
    "9/2021",
    "10/2021",
    "11/2021",
    "12/2021",
  ],
};

type Country = {
  name: string;
  percentage: number;
};

type FeedStockTypes = "Animal fat" | "Used cooking oil" | "Oil" | "PFAD";

type FeedStocks = {
  name: FeedStockTypes;
  percentage: number;
};

const feedStocks: FeedStocks[] = [
  {
    name: "Animal fat",
    percentage: 15.741,
  },
  {
    name: "Used cooking oil",
    percentage: 20.532,
  },
  {
    name: "Oil",
    percentage: 12.876,
  },
  {
    name: "PFAD",
    percentage: 18.235,
  },
];

const countryDonut: Country[] = [
  {
    name: "Austria",
    percentage: 15.741,
  },
  {
    name: "Germany",
    percentage: 20.532,
  },
  {
    name: "Sweden",
    percentage: 12.876,
  },
  {
    name: "Norway",
    percentage: 18.235,
  },
  {
    name: "Denmark",
    percentage: 25.674,
  },
  {
    name: "France",
    percentage: 8.943,
  },
  {
    name: "Spain",
    percentage: 14.567,
  },

  {
    name: "Italy",
    percentage: 22.345,
  },
  {
    name: "Finland",
    percentage: 17.89,
  },
];

export const sustainabilityReportingDTO: SustainabilityReportingDTO = {
  progress,
  productSpecifications,
  countryDonut,
  feedStocks,
  emissionSpeedometer,
};
