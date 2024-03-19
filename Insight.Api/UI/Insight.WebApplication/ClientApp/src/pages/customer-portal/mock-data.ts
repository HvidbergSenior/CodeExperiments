interface Fuel {
  name: string;
  value: string;
}

export const fuelFilter: Fuel[] = [
  { name: "Diesel", value: "diesel" },
  { name: "B100", value: "b100" },
  { name: "HVO", value: "hvo" },
  { name: "Petrol", value: "petrol" },
];

interface Account {
  name: string;
  value: string;
}

export const accountFilter: Account[] = [
  { name: "Volvo 1", value: "volvo1" },
  { name: "Volvo 2", value: "volvo2" },
  { name: "Volvo 3", value: "volvo3" },
  { name: "Volvo 4", value: "volvo4" },
];
