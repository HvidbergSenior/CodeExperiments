import { jwtDecode, JwtPayload } from "jwt-decode";

interface CustomJwtPayload extends JwtPayload {
  access: string[] | string;
  role: string;
  unique_name: string;
}

export function parseJwt(accessToken: string): CustomJwtPayload {
  const decoded = jwtDecode<CustomJwtPayload>(accessToken);
  return decoded;
}
