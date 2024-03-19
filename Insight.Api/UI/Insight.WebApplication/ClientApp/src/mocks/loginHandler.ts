import { HttpResponse, delay, http } from "msw";
import { AuthenticatedResponse } from "../api/api";

export const InterceptLogin = http.post(
  "https://localhost:7084/api/users/login",
  async () => {
    const response: AuthenticatedResponse = {
      accessToken:
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluQGJpb2Z1ZWwtZXhwcmVzcy5jb20iLCJqdGkiOiI1NjlmODNiYy01YzZmLTQ3YzMtYTU3Yy1lN2ExMmY5YzRhOTkiLCJyb2xlIjoiQWRtaW4iLCJuYmYiOjE3MDQyODIzMDAsImV4cCI6MTcwNDI5MzEwMCwiaWF0IjoxNzA0MjgyMzAwLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDg0IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA4NCJ9.lsIr6pVNMofBTZ_Bkc0a5l545aMOICmSDWcnhNXBDyA",
      refreshToken: "YDmWj3abIX7t3fc9nUOAwPfLjaO%2BjNp1gYKwIulRd%2B8%3D",
    };
    await delay(500);
    return HttpResponse.json(response);
  },
);

export const InterceptRefresh = http.post(
  "https://localhost:7084/api/users/refresh",
  async () => {
    const response: AuthenticatedResponse = {
      accessToken:
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluQGJpb2Z1ZWwtZXhwcmVzcy5jb20iLCJqdGkiOiI1NjlmODNiYy01YzZmLTQ3YzMtYTU3Yy1lN2ExMmY5YzRhOTkiLCJyb2xlIjoiQWRtaW4iLCJuYmYiOjE3MDQyODIzMDAsImV4cCI6MTcwNDI5MzEwMCwiaWF0IjoxNzA0MjgyMzAwLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDg0IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA4NCJ9.lsIr6pVNMofBTZ_Bkc0a5l545aMOICmSDWcnhNXBDyA",
      refreshToken: "YDmWj3abIX7t3fc9nUOAwPfLjaO%2BjNp1gYKwIulRd%2B8%3D",
    };
    await delay(500);
    return HttpResponse.json(response);
  },
);
