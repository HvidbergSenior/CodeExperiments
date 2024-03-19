import { HttpResponse, delay, http } from "msw";

export const InterceptCreateStock = http.post(
  "https://localhost:7084/api/stocktransactions",
  async () => {
    await delay(500);
    return HttpResponse.json(null, {
      status: 200,
      statusText: "Success",
    });
  },
);
