import { z } from "zod";
import DateFormatter from "../../helpers/DateFormatter";

export const HallRentSchema = z
  .object({
    // hallId: z.preprocess(
    //   (value) => {
    //     if (value === "" || value === null || value === undefined) {
    //       return NaN;
    //     }
    //     return Number(value);
    //   },
    //   z
    //     .number({
    //       required_error: "Sala jest wymagana",
    //       invalid_type_error: "Sala jest wymagana",
    //     })
    //     .refine((val) => val >= 1 && val < Number.MAX_VALUE, "Id sali musi być większe od 0.")
    // ),
    hallId: z.number().optional(),

    additionalServicesIds: z.array(
      z.number({
        required_error: "Dodatkowe usługi są wymagane",
        invalid_type_error: "Niepoprawny typ wartości",
      })
    ),

    startDate: z
      .preprocess((input) => DateFormatter.ParseDate(input as string), z.date())
      .refine(
        (date) => {
          return date >= new Date();
        },
        { message: `Data początkowa jest wcześniejsza niż obcena data` }
      ),
    endDate: z
      .preprocess((input) => DateFormatter.ParseDate(input as string), z.date())
      .refine(
        (date) => {
          return date >= new Date();
        },
        { message: `Data końcowa jest wcześniejsza niż obcena data` }
      ),
  })
  .refine(
    (data) =>
      new Date(data.endDate).getTime() - new Date(data.startDate).getTime() >= 5 * 60 * 1000,
    {
      message: "Data zakończenia musi być co najmniej 5 minut po dacie początkowej",
    }
  );

export type HallRentRequest = z.infer<typeof HallRentSchema>;
