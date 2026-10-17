import { z } from "zod";
import { MaxFileSizeAndTypeValidator } from "../validators/MaxFileSizeValidator";

export const UserInfoSchema = z.object({
  street: z
    .string()
    .optional()
    .nullable()
    .transform((val) => (val === "" ? null : val))
    .refine((val) => {
      if (val !== undefined && val !== null) {
        return val.length >= 2 && val.length <= 50;
      }
      return true;
    }, "Ulica powinna zawierać od 2 do 50 znaków.")
    .refine((val) => {
      if (val !== undefined && val !== null) {
        return /^[A-ZĄĆĘŁŃÓŚŹŻ0-9][A-Za-ząćęłńóśźż0-9' \-]*$/.test(val);
      }
      return true;
    }, "Ulica powinna zawierać tylko litery lub cyfry."),

  houseNumber: z.preprocess(
    (value) => (value !== "" && value !== null ? Number(value) : 0),
    z
      .number()
      .optional()
      .nullable()
      .transform((val) => (val === 0 ? null : val))
      .refine((val) => {
        if (val !== 0 && val !== null && val != undefined) {
          return val >= 1 && val <= 999;
        }
        return true;
      }, "Nr domu powienien być z zakresu (1-999).")
  ),

  flatNumber: z.preprocess(
    (value) => (value !== "" && value !== null ? Number(value) : 0),
    z
      .number()
      .optional()
      .nullable()
      .transform((val) => (val === 0 ? null : val))
      .refine((val) => {
        if (val !== 0 && val !== null && val != undefined) {
          return val >= 1 && val <= 999;
        }
        return true;
      }, "Nr mieszkania powienien być z zakresu (1-999).")
  ),

  city: z
    .string()
    .optional()
    .nullable()
    .transform((val) => (val === "" ? null : val))
    .refine((val) => {
      if (val !== undefined && val !== null) {
        return val.length >= 2 && val.length <= 50;
      }
      return true;
    }, "Miasto powinno zawierać od 2 do 50 znaków.")
    .refine((val) => {
      if (val !== undefined && val !== null) {
        return /^[A-ZĄĆĘŁŃÓŚŹŻ][a-zA-Ząćęłńóśźżà-ÿÀ-ß' \-]*$/.test(val);
      }
      return true;
    }, "Miasto powinno zawierać tylko litery."),

  zipCode: z
    .string()
    .optional()
    .nullable()
    .transform((val) => (val === "" ? null : val))
    .refine((val) => {
      if (val !== undefined && val !== null) {
        return val.length === 6;
      }
      return true;
    }, "Kod pocztowy powinien zawierać 6 znaków.")
    .refine((val) => {
      if (val !== undefined && val !== null) {
        return /^\d{2}-\d{3}$/.test(val);
      }
      return true;
    }, "Format kodu pocztowego to '12-345'."),

  phoneNumber: z
    .string()
    .optional()
    .nullable()
    .transform((val) => (val === "" ? null : val))
    .refine((val) => {
      if (val !== undefined && val !== null) {
        return val.length >= 5 && val.length <= 15;
      }
      return true;
    }, "Nr telefonu powinien zawierać od 5 do 15 znaków.")
    .refine((val) => {
      if (val !== undefined && val !== null) {
        return /^(\+?\d{1,4}[\s-]?)?(\(?\d{1,5}\)?[\s-]?)?[\d\s-]{5,15}$/.test(val);
      }
      return true;
    }, "Nr telefonu jest nieprawidłowy."),

  userPhoto: MaxFileSizeAndTypeValidator(10, ["image/jpeg"]).nullish(),
});

export type UserInfoRequest = z.infer<typeof UserInfoSchema>;
