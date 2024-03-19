export interface TransactionsResponse {
  id: string;
  date: string;
  // time in ms
  time: number;
  stationId: string;
  stationName: string;
  productNumber: string;
  productName: string;
  customerNumber: string;
  customerName: string;
  cardNumber: string;
  liter: number;
}

export const transactionsData: TransactionsResponse[] = [
  {
    id: "0",
    date: "2024-01-16",
    time: 217788.3904,
    stationId: "2105",
    stationName: "Vimmerby",
    productNumber: "445",
    productName: "HVO100",
    customerNumber: "1234567890987",
    customerName: "Arriva",
    cardNumber: "56789",
    liter: 260,
  },
  {
    id: "1",
    date: "2024-01-17",
    time: 1000000,
    stationId: "2105",
    stationName: "Vimmerby",
    productNumber: "445",
    productName: "HVO100",
    customerNumber: "7890987654321",
    customerName: "DSB",
    cardNumber: "12345",
    liter: 390,
  },
];
