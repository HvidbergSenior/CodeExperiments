import { HttpResponse, delay, http } from "msw";

export const InterceptForgotPassword = http.post(
  "https://localhost:7084/api/users/forgotpassword",
  async () => {
    await delay(500);

    return HttpResponse.json(null, {
      status: 200,
      statusText: "Success",
    });

    /*
    const errorResponse: api.Error = {
      detail: "These are the mocked details",
      instance: "instance mock",
      status: 404,
      title: "This is mock error title",
      type: "error mock",
      traceId: "mock-trace-id",
    }; 
      return HttpResponse.json(errorResponse, {
      status: 404,
      statusText: "Out Of Apples",
    });
    */
  },
);
