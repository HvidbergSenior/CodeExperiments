import { HttpResponse, delay, http } from "msw";
import { UploadIncomingDeclarationCommandResponse } from "../api/api";
import { formatDate } from "../util/formatters/formatters";

export const InterceptDeclarationUpload = http.post(
  "https://localhost:7084/api/incomingdeclarations/upload",
  async () => {
    const today = new Date();
    const laterDay = new Date();
    laterDay.setDate(new Date().getDate() + 4);

    const response: UploadIncomingDeclarationCommandResponse = {
      incomingDeclarationParseResponses: [
        { errorMessage: "", posNumber: "12345", rowNumber: 12, success: true },
      ],
      incomingDeclarationUploadId: { id: "123" },
      newestEntry: formatDate(laterDay),
      oldestEntry: formatDate(today),
    };
    await delay(500);
    return HttpResponse.json(response);
  },
);

export const InterceptDeclarationUploadApprove = http.post(
  "https://localhost:7084/api/incomingdeclarations/approve",
  async () => {
    await delay(500);
    /*
    return HttpResponse.json(null, {
      status: 404,
      statusText: "Out Of Apples",
    });
    */
    return HttpResponse.json(null, {
      status: 200,
      statusText: "OK",
    });
  },
);
