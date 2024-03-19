import cookie from "cookie";
import { parseJwt } from "../parse-jwt";

enum CookieKeys {
  AccessToken = "accessToken",
  AccessTokenForRefresh = "accessTokenForRefresh",
  RefreshToken = "refreshToken",
}

// Access token is active for 3 hours
export const saveAccessTokenWithExpireDateToCookie = (accessToken: string) => {
  if (!accessToken) throw Error("Access token is null");
  const parsedToken = parseJwt(accessToken);
  const timestamp = parsedToken.exp ?? 0;
  // Explanation of why we multiply with 1000: https://stackoverflow.com/questions/62417014/set-cookie-expiration-time
  document.cookie = cookie.serialize(CookieKeys.AccessToken, accessToken, {
    expires: new Date(timestamp * 1000),
    path: "/",
  });
};

export const saveAccessTokenForRefreshToCookie = (accessToken: string) => {
  if (!accessToken) throw Error("Access token for refresh is null");
  document.cookie = cookie.serialize(
    CookieKeys.AccessTokenForRefresh,
    accessToken,
    {
      path: "/",
    },
  );
};

// Refresh token is active for 1 month
export const saveRefreshTokenToCookie = (refreshToken: string) => {
  if (!refreshToken) throw Error("Refresh token is null");
  document.cookie = cookie.serialize(CookieKeys.RefreshToken, refreshToken, {
    expires: new Date(new Date().setMonth(new Date().getMonth() + 1)),
    path: "/",
  });
};

export const loadTokensFromCookies = () => {
  const parsedCookie = cookie.parse(document.cookie);
  const accessToken = parsedCookie[CookieKeys.AccessToken];
  const refreshToken = parsedCookie[CookieKeys.RefreshToken];
  const accessTokenForRefresh = parsedCookie[CookieKeys.AccessTokenForRefresh];
  return { accessToken, refreshToken, accessTokenForRefresh };
};

export const clearAllCookies = () => {
  const cookies = document.cookie.split(";");
  console.log("Before: ", cookies);
  for (let i = 0; i < cookies.length; i++) {
    const cookie = cookies[i];
    console.log("In loop: ", cookie);
    const eqPos = cookie.indexOf("=");
    console.log("eqPos: ", eqPos);
    const name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
    console.log("name: ", name);
    document.cookie = name + "=; Path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT";
  }
  console.log("After: ", document.cookie);
};
