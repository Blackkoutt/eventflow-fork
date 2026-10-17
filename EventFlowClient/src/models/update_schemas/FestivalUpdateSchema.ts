import { z } from "zod";
import { EventFestivalTicketSchema } from "../create_schemas/EventFestivalTicketSchema";
import { MaxFileSizeAndTypeValidatorNotRequired } from "../validators/MaxFileSizeValidatorNotRequired";
export const FestivalUpdateSchema = z.object({
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

  // events: z.record(
  //   z.preprocess(
  //     (value) => (value !== "" && value !== null ? Number(value) : 0),
  //     z
  //       .number({
  //         required_error: "Wydarzenie jest wymagane",
  //         invalid_type_error: "Niepoprawny typ wartości",
  //       })
  //       .transform((val) => (val === 0 ? null : val))
  //       .refine((val) => {
  //         if (val !== 0 && val != null && val != undefined) {
  //           return val >= 1 && val < Number.MAX_VALUE;
  //         }
  //         return true;
  //       }, "Id wydarzenia musi być z zakresu (1-MAX_BVAL).")
  //   ),
  //   FestivalUpdate_EventSchema.optional()
  // ),

  mediaPatronIds: z.array(z.number()),

  organizerIds: z.array(z.number()),

  sponsorIds: z.array(z.number()),
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
  //details: z.object(FestivalDetailsSchema.shape).optional().nullable(),

  festivalPhoto: MaxFileSizeAndTypeValidatorNotRequired(10, ["image/jpeg"]).nullish(),

  festivalTickets: z.array(EventFestivalTicketSchema).optional(),
});
export type FestivalUpdateRequest = z.infer<typeof FestivalUpdateSchema>;
