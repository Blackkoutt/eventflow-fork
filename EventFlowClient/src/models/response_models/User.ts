import { Roles } from "../../helpers/enums/UserRoleEnum";
import { UserData } from "./UserData";

export type User = {
  id: string;
  name: string;
  surname: string;
  emailAddress: string;
  dateOfBirth: string;
  registeredDate: string;
  photoEndpoint?: string;
  allReservationsCount: number;
  allHallRentsCount: number;
  isActiveEventPass: boolean;
  userData?: UserData;
  userRoles: string[];
};

export type AuthUser = Pick<User, "id" | "name" | "surname" | "emailAddress" | "dateOfBirth"> & {
  userRoles: string | string[];
};

export const isUserInRole = (user: Pick<AuthUser, "userRoles"> | null | undefined, role: Roles) => {
  if (user === null || user === undefined || user.userRoles === undefined) return false;
  if (!Array.isArray(user.userRoles)) return user.userRoles === role;
  return user.userRoles.includes(role);
};
