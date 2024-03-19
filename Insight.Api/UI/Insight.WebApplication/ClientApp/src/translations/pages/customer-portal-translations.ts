export const customerPortalTranslations = {
  logout: "Logout",
  menuItemFuelConsumption: "Fuel Consumption",
  menuItemSustainabilityReporting: "Sustainability Reporting",
  menuItemSettings: "Settings",
  settings: {
    addUser: "Add User",
    editUser: "Edit User",
    filterEmail: "Email",
    filterAccountId: "Account Id",
    filterStatus: "Status",
    emptyTableText: "No users have been found",
    usersLoaded: "Users loaded:",
  },
  filter: {
    period: "Period",
    fuel: "Products",
    account: "Account",
    accounts: "Accounts",
    accountsSelected: (selected: number) => `Accounts (${selected} selected)`,
  },
  fuelConsumption: {
    downloadButtonTitle: "Download Fuel Consumption Report",
    fuelConsumptionStats: {
      consumptionTitle: "Total fuel consumption",
      renewablesTitle: "Renewable fuels",
      renewableShareTitle: "Renewable share",
      unit: "Liters",
      noData: "No data available",
    },
    consumptionDevelopment: {
      columnChartTitle: "Consumption development",
    },
    consumptionPerProduct: {
      treeMapTitle: "Consumption per product",
    },
    consumptionTransactions: {
      title: "Transactions",
      table: {
        date: "Date",
        time: "Time",
        stationId: "Station ID",
        stationName: "Station name",
        accountNumber: "Account number",
        accountName: "Account name",
        productNumber: "Product number",
        productName: "Product name",
        customerNumber: "Customer no",
        customerName: "Customer name",
        cardNumber: "Card no name",
        liter: "Liter",
        location: "Location",
      },
    },
  },
  sustainability: {
    download: "Download sustainability report",
    emissionStats: {
      CO2eLabel: "GHG emission saving",
      netEmissionTitle: "Net emissions",
      kgCO2eLabel: "tCO2eq",
      emissionsReductionTitle: "Achieved emission reduction",
    },
    emissionProgress: {
      title: "Emissions & emission reduction development",
      legendEmissions: "Emissions (ton CO2e)",
      legendAchivedEmissions: "Achieved emission reduction (ton CO2e)",
      unit: "kg CO2e",
    },
    traceability: {
      title: "Traceability",
      feedstockTitle: "Feedstocks",
      countryOfOriginTitle: "Country of origin",
      number: "No.",
      cooFeedstock: "COO Feedstock",
      volume: "Volume (l)",
      averageGhgSavings: "Average GHG savings",
    },
  },
};