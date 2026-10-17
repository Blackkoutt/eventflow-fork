import { z } from "zod";
import { EventFestivalTicketSchema } from "./EventFestivalTicketSchema";
import { MaxFileSizeAndTypeValidatorNotRequired } from "../validators/MaxFileSizeValidatorNotRequired";

export const FestivalSchema = z.object({
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
  eventIds: z
    .array(
      z.number({
        required_error: "Wydarzenia są wymagane",
        invalid_type_error: "Niepoprawny typ wartości",
      })
    )
    .refine((arr) => arr.length > 0, "Lista wydarzeń nie może być pusta"),
  mediaPatronIds: z.array(
    z.number({
      required_error: "Patroni medialni są wymagani",
      invalid_type_error: "Niepoprawny typ wartości",
    })
  ),
  organizerIds: z.array(
    z.number({
      required_error: "Organizatorzy są wymagani",
      invalid_type_error: "Niepoprawny typ wartości",
    })
  ),
  sponsorIds: z.array(
    z.number({
      required_error: "Sponsorzy są wymagani",
      invalid_type_error: "Niepoprawny typ wartości",
    })
  ),
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
  festivalPhoto: MaxFileSizeAndTypeValidatorNotRequired(10, ["image/jpeg"]).nullish(),
  festivalTickets: z.array(EventFestivalTicketSchema).optional(),
});

export type FestivalRequest = z.infer<typeof FestivalSchema>;
