import { z } from "zod";
import { SeatSchema } from "../create_schemas/SeatSchema";

export const HallUpdateSchema = z.object({
  hallNr: z.preprocess(
    (value) => {
      if (value === "" || value === null || value === undefined) {
        return NaN;
      }
      return Number(value);
    },
    z
      .number({
        required_error: "Nr sali jest wymagany",
        invalid_type_error: "Nr sali jest wymagany",
      })
      .refine((val) => val >= 1 && val < Number.MAX_VALUE, "Nr sali musi być większy od 0")
  ),

  rentalPricePerHour: z.preprocess(
    (value) => {
      if (value === "" || value === null || value === undefined) {
        return NaN;
      }
      return Number(value);
    },
    z
      .number({
        required_error: "Cena wynajmu za godzinę jest wymagana",
        invalid_type_error: "Cena wynajmu za godzinę jest wymagana",
      })
      .refine((val) => val >= 0 && val < 1000, "Cena sali musi być z przedziału (0-1000) zł")
  ),

  // floor: z.preprocess(
  //   (value) => {
  //     if (value === "" || value === null || value === undefined) {
  //       return NaN;
  //     }
  //     return Number(value);
  //   },
  //   z
  //     .number({
  //       required_error: "Nr piętra jest wymagany",
  //       invalid_type_error: "Nr piętra jest wymagany",
  //     })
  //     .refine((val) => val >= 1 && val <= 4, "Nr piętra musi być z przedziału (1-4)")
  // ),

  // hallDetails: z.object(HallDetailsSchema.shape),

  hallTypeId: z.preprocess(
    (value) => {
      if (value === "" || value === null || value === undefined) {
        return NaN;
      }
      return Number(value);
    },
    z
      .number({
        required_error: "Typ sali jest wymagany",
        invalid_type_error: "Typ sali jest wymagany",
      })
      .refine((val) => val >= 1 && val < Number.MAX_VALUE, "Id typu sali musi być większe od 0")
  ),

  seats: z.array(SeatSchema).optional(),
});

export type HallUpdateRequest = z.infer<typeof HallUpdateSchema>;
