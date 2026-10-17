import { z } from "zod";
import { EventFestivalTicketSchema } from "../create_schemas/EventFestivalTicketSchema";
import DateFormatter from "../../helpers/DateFormatter";
import { MaxFileSizeAndTypeValidatorNotRequired } from "../validators/MaxFileSizeValidatorNotRequired";

export const EventSchema = z
  .object({
    name: z
      .string({
        required_error: "Nazwa jest wymagana",
        invalid_type_error: "Niepoprawny typ wartości",
      })
      .min(2, "Nazwa powinna zawierać przynajmniej 2 znaki")
      .max(60, "Nazwa powinna zawierać maksymalnie 60 znaków"),

    shortDescription: z
      .string({
        required_error: "Krótki opis jest wymagany",
        invalid_type_error: "Niepoprawny typ wartości",
      })
      .min(2, "Krótki opis powinien zawierać przynajmniej 2 znaki")
      .max(300, "Krótki opis powinien zawierać maksymalnie 300 znaków"),

    longDescription: z
      .string()
      .optional()
      .nullable()
      .transform((val) => (val === "" ? null : val))
      .refine((val) => {
        if (val !== undefined && val !== null) {
          return val.length >= 2 && val.length <= 2000;
        }
        return true;
      }, "Długi opis powinien zawierać od 2 do 2000 znaków."),

    categoryId: z.preprocess(
      (value) => {
        if (value === "" || value === null || value === undefined) {
          return NaN;
        }
        return Number(value);
      },
      z
        .number({
          required_error: "Kategoria jest wymagana",
          invalid_type_error: "Kategoria jest wymagana",
        })
        .refine((val) => val >= 1 && val < Number.MAX_VALUE, "Id kategorii musi być większe od 0.")
    ),

    hallId: z.preprocess(
      (value) => {
        if (value === "" || value === null || value === undefined) {
          return NaN;
        }
        return Number(value);
      },
      z
        .number({
          required_error: "Sala jest wymagana",
          invalid_type_error: "Sala jest wymagana",
        })
        .refine((val) => val >= 1 && val < Number.MAX_VALUE, "Id sali musi być większe od 0.")
    ),

    eventPhoto: MaxFileSizeAndTypeValidatorNotRequired(10, ["image/jpeg"]).nullish(),

    eventTickets: z.array(EventFestivalTicketSchema).optional(),

    startDate: z
      .preprocess((input) => DateFormatter.ParseDate(input as string), z.date())
      .refine(
        (date) => {
          return date >= new Date();
        },
        { message: "Data początkowa musi być w przyszłości" }
      ),
    endDate: z
      .preprocess((input) => DateFormatter.ParseDate(input as string), z.date())
      .refine(
        (date) => {
          return date >= new Date();
        },
        { message: "Data zakończenia musi być w przyszłości" }
      ),
  })
  .refine(
    (data) =>
      new Date(data.endDate).getTime() - new Date(data.startDate).getTime() >= 5 * 60 * 1000,
    {
      message: "Zakończenie musi być co najmniej 5 minut po rozpoczęciu",
    }
  )
  .refine(
    (data) =>
      new Date(data.endDate).getTime() - new Date(data.startDate).getTime() <= 24 * 60 * 60 * 1000,
    {
      message: `Wydarzenie może trwać max ${24} h`,
    }
  );

export type EventRequest = z.infer<typeof EventSchema>;
