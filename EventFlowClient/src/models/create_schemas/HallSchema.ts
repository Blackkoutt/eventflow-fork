import { z } from "zod";
import { SeatSchema } from "./SeatSchema";

export const HallSchema = z.object({
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

  floor: z.preprocess(
    (value) => {
      if (value === "" || value === null || value === undefined) {
        return NaN;
      }
      return Number(value);
    },
    z
      .number({
        required_error: "Nr piętra jest wymagany",
        invalid_type_error: "Nr piętra jest wymagany",
      })
      .refine((val) => val >= 1 && val <= 4, "Nr piętra musi być z przedziału (1-4)")
  ),

  totalLength: z.preprocess(
    (value) => {
      if (value === "" || value === null || value === undefined) {
        return NaN;
      }
      return Number(value);
    },
    z
      .number({
        required_error: "Długość sali jest wymagana",
        invalid_type_error: "Długość sali jest wymagana",
      })
      .refine((val) => val >= 4 && val < 100, "Długość sali musi być z przedziału (4-100) m")
  ),
  totalWidth: z.preprocess(
    (value) => {
      if (value === "" || value === null || value === undefined) {
        return NaN;
      }
      return Number(value);
    },
    z
      .number({
        required_error: "Szerokość sali jest wymagana",
        invalid_type_error: "Szerokość sali jest wymagana",
      })
      .refine((val) => val >= 4 && val < 100, "Szerokość sali musi być z przedziału (4-100) m")
  ),

  stageLength: z.preprocess(
    (value) => (value !== "" && value !== null ? Number(value) : 0),
    z
      .number()
      .optional()
      .nullable()
      .transform((val) => (val === 0 ? null : val))
      .refine((val) => {
        if (val !== 0 && val !== null && val != undefined) {
          return val >= 3 && val <= 30;
        }
        return true;
      }, "Długość sceny powinna być z zakresu (3-30).")
  ),

  stageWidth: z.preprocess(
    (value) => (value !== "" && value !== null ? Number(value) : 0),
    z
      .number()
      .optional()
      .nullable()
      .transform((val) => (val === 0 ? null : val))
      .refine((val) => {
        if (val !== 0 && val !== null && val != undefined) {
          return val >= 3 && val <= 30;
        }
        return true;
      }, "Szerokość sceny powinna być z zakresu (3-30).")
  ),

  maxNumberOfSeatsRows: z.preprocess(
    (value) => {
      if (value === "" || value === null || value === undefined) {
        return NaN;
      }
      return Number(value);
    },
    z
      .number({
        required_error: "Maksymalna ilość rzedów miejsc jest wymagana",
        invalid_type_error: "Maksymalna ilość rzedów miejsc jest wymagana",
      })
      .refine(
        (val) => val >= 1 && val <= 20,
        "Maksymalna ilość rzedów miejsc musi być z przedziału (1-20)"
      )
  ),
  maxNumberOfSeatsColumns: z.preprocess(
    (value) => {
      if (value === "" || value === null || value === undefined) {
        return NaN;
      }
      return Number(value);
    },
    z
      .number({
        required_error: "Maksymalna ilość kolumn miejsc jest wymagana",
        invalid_type_error: "Maksymalna ilość kolumn miejsc jest wymagana",
      })
      .refine(
        (val) => val >= 1 && val <= 20,
        "Maksymalna ilość kolumn miejsc musi być z przedziału (1-20)"
      )
  ),

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

export type HallRequest = z.infer<typeof HallSchema>;
