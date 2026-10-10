import { createContext, PropsWithChildren, useContext, useEffect, useState } from "react";
import { User } from "../models/response_models";
import { UserLogin } from "../models/response_models/UserLogin";
import { ApiEndpoint } from "../helpers/enums/ApiEndpointEnum";
import useApi from "../hooks/useApi";
import { UserLoginRequest } from "../models/create_schemas/auth/UserLoginSchema";
import { jwtDecode } from "jwt-decode";
import {
  getUserFormCookie,
  isJWTTokenCookieExist,
  removeJWTTokenCookie,
  setJWTTokenCookie,
} from "../helpers/cookies/JWTCookie";
import { APIError } from "../models/error/APIError";
import { useNavigate } from "react-router-dom";

type AuthContextType = {
  authenticated?: boolean | null;
  currentUser?: User | null;
  handleLogin: (loginRequest: UserLoginRequest) => Promise<void>;
  performAuthentication: (token?: string, errorMessages?: APIError[]) => void;
  handleLogout: () => void;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

type AuthProviderProps = PropsWithChildren;

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [authenticated, setAuthenticated] = useState<boolean | null>(isJWTTokenCookieExist());
  const [currentUser, setCurrentUser] = useState<User | null>(getUserFormCookie());

  // Login in application
  const { data: loginResponse, post: loginUser } = useApi<UserLogin, UserLoginRequest>(
    ApiEndpoint.AuthLogin
  );

  // Auth Validation
  const {
    data: validatedUser,
    get: validateUser,
    error: validateUserError,
  } = useApi<User>(ApiEndpoint.AuthValidate);

  // Auth Validation
  useEffect(() => {
    if (isJWTTokenCookieExist()) {
      validateUser({ id: undefined, queryParams: undefined });
    } else {
      setAuthenticated(false);
      setCurrentUser(null);
    }
  }, []);

  useEffect(() => {
    console.log(validatedUser[0]);
    if (validateUserError !== null) {
      setAuthenticated(false);
      setCurrentUser(null);
    } else if (validateUser.length !== 0 && validatedUser[0] !== undefined) {
      setAuthenticated(true);
      const user = validatedUser[0];
      setCurrentUser({
        id: user.id,
        name: user.name,
        surname: user.surname,
        emailAddress: user.emailAddress,
        dateOfBirth: user.dateOfBirth,
        userRoles: user.userRoles,
      });
    }
  }, [validatedUser, validateUserError]);

  // Set Cookie with JWT token if user is Authenticated
  useEffect(() => {
    const token = loginResponse.length > 0 ? loginResponse[0].token : undefined;
    console.log("token", token);

    performAuthentication(token);
  }, [loginResponse]);

  const performAuthentication = (token?: string) => {
    if (token !== undefined) {
      const decodedToken = jwtDecode(token);
      console.log(decodedToken);
      const userDecodedToken = decodedToken as User;
      let tokenExpirationDate: Date | undefined = undefined;
      if (decodedToken.exp !== undefined) {
        tokenExpirationDate = new Date(decodedToken.exp * 1000);
      }
      setJWTTokenCookie(token, tokenExpirationDate);
      setAuthenticated(true);
      setCurrentUser({
        id: userDecodedToken.id,
        name: userDecodedToken.name,
        surname: userDecodedToken.surname,
        emailAddress: userDecodedToken.emailAddress,
        dateOfBirth: userDecodedToken.dateOfBirth,
        userRoles: userDecodedToken.userRoles,
      });
    }
  };

  const handleLogin = async (loginRequest: UserLoginRequest) => {
    await loginUser({ body: loginRequest });
  };

  const handleLogout = () => {
    setAuthenticated(false);
    setCurrentUser(null);
    removeJWTTokenCookie();
  };

  return (
    <AuthContext.Provider
      value={{
        authenticated,
        currentUser,
        handleLogin,
        performAuthentication,
        handleLogout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);

  if (context === undefined) {
    throw Error("Use auth must be used in AuthProvider");
  }

  return context;
};
