import Cookies from "js-cookie";
import { User } from "../../models/response_models";
import { jwtDecode } from "jwt-decode";

const jwtTokenCookieName = "EventFlowJWTCookie";

export const setJWTTokenCookie = (token: string, expirationDate?: Date): void => {
  const expires = expirationDate || new Date(new Date().getTime() + 7 * 24 * 60 * 60 * 1000);

  Cookies.set(jwtTokenCookieName, token, {
    expires: expires,
    secure: true,
    //httpOnly: true,
    sameSite: "Strict", // Zapobiega atakom CSRF
  });
};

export const isJWTTokenCookieExist = () => {
  return Cookies.get(jwtTokenCookieName) !== undefined;
};

export const getUserFormCookie = () => {
  const token = Cookies.get(jwtTokenCookieName);
  if (token === undefined) return null;
  const decodedToken = jwtDecode(token) as User;
  console.log(decodedToken);
  const user: User = {
    id: decodedToken.id,
    name: decodedToken.name,
    surname: decodedToken.surname,
    email: decodedToken.email,
    dateOfBirth: decodedToken.dateOfBirth,
    userRoles: decodedToken.userRoles,
  };
  return user;
};

export const removeJWTTokenCookie = (): void => {
  Cookies.remove(jwtTokenCookieName);
};
