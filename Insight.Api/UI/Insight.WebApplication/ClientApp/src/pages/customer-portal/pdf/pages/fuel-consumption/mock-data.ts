export type FuelTypeMock = "B100" | "HVO" | "Diesel" | "Petrol";

type FuelConsumptionDTO = {
  consumptionPerProduct: ConsumptionPerProduct;
  consumptionDevelopment: ConsumptionDevelopment;
};

type ConsumptionDevelopment = {
  series: {
    name: FuelTypeMock;
    data: number[];
  }[];
  categories: string[];
};

type ConsumptionPerProduct = {
  data: {
    name: FuelTypeMock;
    value: number;
  }[];
};

const consumptionPerProduct: ConsumptionPerProduct = {
  data: [
    {
      name: "Diesel",
      value: 1095,
    },
    {
      name: "B100",
      value: 695,
    },
    {
      name: "HVO",
      value: 555,
    },
    {
      name: "Petrol",
      value: 900,
    },
  ],
};

const consumptionDevelopment: ConsumptionDevelopment = {
  series: [
    {
      name: "B100",
      data: [
        4400, 5500, 4100, 6700, 2200, 4300, 4400, 5500, 4100, 6070, 202, 4300,
      ],
    },
    {
      name: "HVO",
      data: [
        1030, 2300, 2000, 800, 1300, 2070, 1003, 2003, 2000, 800, 1300, 2007,
      ],
    },
    {
      name: "Diesel",
      data: [
        1010, 1070, 1050, 1050, 2010, 1040, 1010, 1070, 1050, 1050, 2001, 1004,
      ],
    },
    {
      name: "Petrol",
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

export const fuelConsumptionDTO: FuelConsumptionDTO = {
  consumptionPerProduct,
  consumptionDevelopment,
};
