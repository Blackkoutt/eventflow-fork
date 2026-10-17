import { Roles } from "../../helpers/enums/UserRoleEnum";

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
  street?: string;
  houseNumber?: number;
  flatNumber?: number;
  city?: string;
  zipCode?: string;
  phoneNumber?: string;
  userRoles: string[];
};

export const isUserInRole = (user: User | null | undefined, role: Roles) => {
  if (user === null || user === undefined || user.userRoles === undefined) return false;
  if (!Array.isArray(user.userRoles)) return user.userRoles === role;
  return user.userRoles.includes(role);
};
