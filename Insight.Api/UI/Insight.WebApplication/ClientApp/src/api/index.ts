import * as api from "./api";
import { VITE_API_PATH } from "./paths";

const API_PATH: string =
  import.meta.env.MODE === "development" ? VITE_API_PATH : "";

const authorizedHttpClient = new api.Api({
  baseURL: API_PATH,
  headers: { "x-trace-id": "string" },
});

const unAuthorizedHttpClient = new api.Api({
  baseURL: API_PATH,
  headers: { "x-trace-id": "string" },
});

export { api, authorizedHttpClient, unAuthorizedHttpClient };
