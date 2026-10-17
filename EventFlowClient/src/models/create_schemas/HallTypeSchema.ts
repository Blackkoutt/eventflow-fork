import { z } from "zod";
import { MaxFileSizeAndTypeValidatorNotRequired } from "../validators/MaxFileSizeValidatorNotRequired";

export const HallTypeSchema = z.object({
  name: z
    .string({
      required_error: "Nazwa jest wymagana",
      invalid_type_error: "Niepoprawny typ wartości",
    })
    .min(2, "Nazwa powinna zawierać przynajmniej 2 znaki.")
    .max(30, "Nazwa powinna zawierać maksymalnie 30 znaków."),

  description: z
    .string()
    .optional()
    .nullable()
    .transform((val) => (val === "" ? null : val))
    .refine((val) => {
      if (val !== undefined && val !== null) {
        return val.length >= 2 && val.length < 600;
      }
      return true;
    }, "Opis zawierać od 2 do 600 znaków."),

  equipmentIds: z.array(
    z.number({
      required_error: "Wyposażenie jest wymagane",
      invalid_type_error: "Niepoprawny typ wartości",
    })
  ),

  hallTypePhoto: MaxFileSizeAndTypeValidatorNotRequired(10, ["image/jpeg"]).nullish(),
});

export type HallTypeRequest = z.infer<typeof HallTypeSchema>;
