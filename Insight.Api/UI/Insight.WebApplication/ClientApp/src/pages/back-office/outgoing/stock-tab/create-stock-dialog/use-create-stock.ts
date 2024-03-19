import { useCallback, useState } from "react";
import { api, authorizedHttpClient } from "../../../../../api";
import { OutgoingFuelTransactionResponse } from "../../../../../api/api";
import { useSnackBar } from "../../../../../shared/snackbar/use-snackbar";
import { stockTabTranslations } from "../../../../../translations/pages/stock-tab-translations";
import { isApiError } from "../../../../../util/errors/predicates";
import useHandleNetworkError from "../../../../../util/errors/use-handle-network-error";

export interface StockData {
  storage: string;
  country: string;
  product: string;
  companyId: string;
  volume: number;
  period: string;
}

export class Company {
  id: string;
  name: string;
  products: Product[] = [];

  constructor(id = "", name = "", products: Product[] = []) {
    this.id = id;
    this.name = name;

    this.products = products;
  }
}

export class Product {
  number: string;
  name: string;

  constructor(number = "", name = "") {
    this.number = number;
    this.name = name;
  }
}

export const useCreateStock = (
  fuelTransaction?: OutgoingFuelTransactionResponse,
) => {
  const { showErrorDialog } = useHandleNetworkError();
  const { showSnackBar } = useSnackBar();
  const [isLoading, setIsLoading] = useState(false);
  const [apiError, setApiError] = useState<api.Error | undefined>(undefined);
  const [selectableProducts, setSelectableProducts] = useState<Product[]>([]);
  const [isProductSelectionEnabled, setIsProductSelectionEnabled] =
    useState(false);

  const disableSubmit = false;

  console.log("Fueltransaction: " + fuelTransaction?.country);

  const companies = [
    new Company("ba61a6f1-3ef6-ed11-8848-6045bd937f02", "Biofuel Express AB", [
      new Product("200", "B100"),
      new Product("201", "B100"),
      new Product("206", "B100"),
      new Product("208", "B100"),
      new Product("439", "B100"),
      new Product("250", "HVO DIESEL"),
      new Product("445", "HVO DIESEL"),
      new Product("230", "HVO100"),
      new Product("231", "HVO100"),
      new Product("232", "HVO100"),
      new Product("235", "HVO100"),
      new Product("440", "HVO100"),
      new Product("465", "HVO100"),
    ]),
    new Company("fd937a13-3ff6-ed11-8848-6045bd937f02", "Biofuel Express AS", [
      new Product("200", "B100"),
      new Product("201", "B100"),
      new Product("230", "HVO100"),
      new Product("231", "HVO100"),
      new Product("235", "HVO100"),
      new Product("435", "HVO100"),
    ]),
    new Company(
      "b49ae72c-3ff6-ed11-8848-6045bd937f02",
      "Biofuel Express Austria GmbH",
      [
        new Product("200", "B100"),
        new Product("201", "B100"),
        new Product("230", "HVO100"),
        new Product("231", "HVO100"),
      ],
    ),
    new Company(
      "db71d54f-3ff6-ed11-8848-6045bd937f02",
      "Biofuel Express DMCC",
      [
        new Product("200", "B100"),
        new Product("201", "B100"),
        new Product("230", "HVO100"),
        new Product("231", "HVO100"),
      ],
    ),
    new Company(
      "fa3ee39d-ed73-ee11-817a-000d3a2fa3a7",
      "Biofuel Express GmbH",
      [
        new Product("200", "B100"),
        new Product("201", "B100"),
        new Product("230", "HVO100"),
        new Product("231", "HVO100"),
      ],
    ),
  ];

  const createStock = useCallback(async (stockData: StockData) => {
    setIsLoading(true);
    try {
      await authorizedHttpClient.api.createStockTransaction({
        companyId: stockData.companyId,
        location: stockData.storage,
        country: stockData.country,
        productNumber: stockData.product,
        quantity: stockData.volume,
        transactionDate: stockData.period,
      });
      showSnackBar(
        stockTabTranslations.createStockDialog.stockCreated,
        "success",
      );
      setIsLoading(false);
      setApiError(undefined);
      return true;
    } catch (error) {
      const errorData = isApiError(error);
      if (errorData) {
        setApiError(errorData);
      } else {
        showErrorDialog(error);
      }
      setIsLoading(false);
      return false;
    }
  }, []);

  const onCompanyChanged = useCallback(
    (companyId: string, selectedProduct: string | undefined) => {
      // Returns true, if the selected product is still valid
      const match = companies.find((item) => item.id === companyId);
      if (match === undefined) {
        return false;
      }
      setIsProductSelectionEnabled(true);
      setSelectableProducts(match.products);
      if (selectedProduct !== undefined && selectedProduct !== "") {
        const productMatch = match.products.find(
          (item) => item.number === selectedProduct,
        );
        return productMatch !== undefined;
      }
      return true;
    },
    [],
  );

  return {
    isLoading,
    setIsLoading,
    disableSubmit,
    createStock,
    apiError,
    companies,
    onCompanyChanged,
    selectableProducts,
    isProductSelectionEnabled,
  };
};
