import { getDateAYearAgoFromStartOfMonth } from "../date-utils";
import { formatDate } from "../formatters/formatters";
import {
  addParamsToUrl,
  decodeUrlParams,
  ensureCustomUrlFilterParams,
} from "./url-params";

describe("ensureCustomerFilterParams", () => {
  it("should return default filter when no parameters are provided", () => {
    const params = {};
    const filter = ensureCustomUrlFilterParams(params);
    expect(filter).toEqual({
      fromDate: formatDate(getDateAYearAgoFromStartOfMonth()),
      toDate: formatDate(new Date()),
      fuels: [],
      accountsIds: [],
      username: "-",
    });
  });

  it("should return filter with decoded parameters", () => {
    const params = {
      fromDate: "2024-01-01",
      toDate: "2024-02-02",
      fuels: ["Hvo100"],
      accountsIds: ["account1", "account2"],
      username: "-",
    };
    const filter = ensureCustomUrlFilterParams(params);
    expect(filter).toEqual(params);
  });

  it("should return filter with default fromDate when fromDate is missing", () => {
    const params = {
      toDate: "2024-02-02",
      fuels: ["Hvo100", "HvoDiesel"],
      accountsIds: ["account1", "account2"],
    };
    const filter = ensureCustomUrlFilterParams(params);
    expect(filter.fromDate).toBe(formatDate(getDateAYearAgoFromStartOfMonth()));
  });

  it("should return filter with default toDate when toDate is missing", () => {
    const params = {
      fromDate: "2024-01-01",
      fuels: ["fuel1", "fuel2"],
      accountsIds: ["account1", "account2"],
    };
    const filter = ensureCustomUrlFilterParams(params);
    expect(filter.toDate).toStrictEqual(formatDate(new Date()));
  });

  it("should return filter with the correct fuel types", () => {
    const params = {
      fromDate: "2024-01-01",
      fuels: ["Hvo100", "HvoDiesel"],
      accountsIds: ["account1", "account2"],
    };
    const filter = ensureCustomUrlFilterParams(params);
    expect(filter.fuels).toStrictEqual(["Hvo100", "HvoDiesel"]);
  });

  it("should return empty fuel type array when fuel types are not correct", () => {
    const params = {
      fromDate: "2024-01-01",
      fuels: ["asdads", "asdas"],
      accountsIds: ["account1", "account2"],
    };
    const filter = ensureCustomUrlFilterParams(params);
    expect(filter.fuels).toStrictEqual([]);
  });

  it("should return filter with default fuels when fuels is missing", () => {
    const params = {
      fromDate: "2024-01-01",
      toDate: "2024-02-02",
      accountsIds: ["account1", "account2"],
    };
    const filter = ensureCustomUrlFilterParams(params);
    expect(filter.fuels).toEqual([]);
  });

  it("should return filter with default accountsIds when accountsIds is missing", () => {
    const params = {
      fromDate: "2024-01-01",
      toDate: "2024-02-02",
      fuels: ["fuel1", "fuel2"],
    };
    const filter = ensureCustomUrlFilterParams(params);
    expect(filter.accountsIds).toEqual([]);
  });
});

describe("addParamsToUrl", () => {
  it("should add parameters to the URL", () => {
    const baseUrl = "https://localhost:3000/pdf";
    const params = {
      fromDate: "2023-01-04",
      toDate: "2024-02-29",
      fuels: ["HvoDiesel", "B100", "Petrol"],
    };
    const expectedUrl =
      "https://localhost:3000/pdf?fromDate=2023-01-04&toDate=2024-02-29&fuels=HvoDiesel&fuels=B100&fuels=Petrol";
    const newUrl = addParamsToUrl(baseUrl, params);
    expect(newUrl).toBe(expectedUrl);
  });
});

describe("decodeUrlParams", () => {
  it("should decode parameters from the URL", () => {
    const url =
      "https://localhost:3000/pdf?fromDate=2023-01-04&toDate=2024-02-29";
    const expectedParams = {
      fromDate: "2023-01-04",
      toDate: "2024-02-29",
    };
    const decodedParams = decodeUrlParams(url);
    expect(decodedParams).toEqual(expectedParams);
  });

  it("should handle parameters with arrays", () => {
    const url =
      "https://localhost:3000/pdf?fromDate=2023-01-04&toDate=2024-02-29&fuels=HvoDiesel&fuels=B100&fuels=Petrol&accountsIds=9127047c-4ee1-4a1f-a083-f58d55fddcb5&accountsIds=a9f532d9-3f05-41de-89c4-701ce7487cd5&accountsIds=9be57e7b-0618-4ffb-ae65-f3f686a2025e";
    const expectedParams = {
      fromDate: "2023-01-04",
      toDate: "2024-02-29",
      fuels: ["HvoDiesel", "B100", "Petrol"],
      accountsIds: [
        "9127047c-4ee1-4a1f-a083-f58d55fddcb5",
        "a9f532d9-3f05-41de-89c4-701ce7487cd5",
        "9be57e7b-0618-4ffb-ae65-f3f686a2025e",
      ],
    };
    const decodedParams = decodeUrlParams(url);
    expect(decodedParams).toEqual(expectedParams);
  });

  it("should handle missing parameters", () => {
    const url = "https://example.com/api";
    const decodedParams = decodeUrlParams(url);
    expect(decodedParams).toEqual({});
  });
});
