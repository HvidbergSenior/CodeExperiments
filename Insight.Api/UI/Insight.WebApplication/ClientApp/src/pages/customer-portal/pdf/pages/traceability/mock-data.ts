type TraceabilityDTO = {
  countryDonut: Country[]; // Should have a maxLength of 13
  feedStocks: FeedStocks[];
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

export const traceability: TraceabilityDTO = {
  countryDonut,
  feedStocks,
};
