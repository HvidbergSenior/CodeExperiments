import { ProductNameEnumeration } from "../../api/api";
import { getDateAYearAgoFromStartOfMonth } from "../date-utils";
import { formatDate } from "../formatters/formatters";

export const addParamsToUrl = (
  baseUrl: string,
  params: { [key: string]: string | string[] },
): string => {
  const url = new URL(baseUrl);
  Object.entries(params).forEach(([key, value]) => {
    if (Array.isArray(value)) {
      value.forEach((val) => url.searchParams.append(key, val));
    } else {
      url.searchParams.append(key, value);
    }
  });
  return url.toString();
};

export const decodeUrlParams = (
  url: string,
): { [key: string]: string | string[] } => {
  const urlObj = new URL(url);
  const params: { [key: string]: string | string[] } = {};

  for (const [key, value] of urlObj.searchParams.entries()) {
    if (params[key] === undefined) {
      params[key] = value;
    } else {
      if (Array.isArray(params[key])) {
        (params[key] as string[]).push(value);
      } else {
        params[key] = [params[key] as string, value];
      }
    }
  }
  return params;
};

type CustomUrlFilterParams = {
  fromDate: string;
  toDate: string;
  fuels: ProductNameEnumeration[];
  accountsIds: string[];
  username: string;
};

export function ensureCustomUrlFilterParams(params: {
  [key: string]: string | string[];
}): CustomUrlFilterParams {
  // Define default values for missing properties
  const defaults: CustomUrlFilterParams = {
    fromDate: formatDate(getDateAYearAgoFromStartOfMonth()),
    toDate: formatDate(new Date()),
    fuels: [],
    accountsIds: [],
    username: "-",
  };

  // Merge the default values with the decoded parameters
  const parameters: CustomUrlFilterParams = {
    ...defaults,
    fromDate: (params.fromDate as string) || defaults.fromDate,
    toDate: (params.toDate as string) || defaults.toDate,
    fuels:
      Array.isArray(params.fuels) && areValidFuelTypes(params.fuels)
        ? (params.fuels as ProductNameEnumeration[])
        : defaults.fuels,
    accountsIds: Array.isArray(params.accountsIds)
      ? params.accountsIds
      : defaults.accountsIds,
    username: (params.username as string) || defaults.username,
  };

  return parameters;
}

function areValidFuelTypes(fuels: string[]): boolean {
  const products: ProductNameEnumeration[] = [
    "Adblue",
    "B100",
    "Diesel",
    "HeatingOil",
    "Hvo100",
    "HvoDiesel",
    "Other",
    "Petrol",
    "Unknown",
  ];
  return fuels.every((fuel) =>
    Object.values(products).includes(fuel as ProductNameEnumeration),
  );
}
