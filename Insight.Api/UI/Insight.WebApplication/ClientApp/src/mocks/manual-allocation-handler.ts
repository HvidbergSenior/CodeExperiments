import { HttpResponse, delay, http } from "msw";

export const InterceptManualAllocation = http.post(
  "https://localhost:7084/api/allocations/manual",
  async () => {
    await delay(500);
    return HttpResponse.json(null, {
      status: 200,
      statusText: "Success",
    });
  },
);
