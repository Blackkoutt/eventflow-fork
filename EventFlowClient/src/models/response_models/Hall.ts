import { IModel } from "../abstract/IModel";
import { HallType } from "./HallType";
import { Seat } from "./Seat";

export type Hall = IModel & {
  hallNr: number;
  rentalPricePerHour: number;
  floor: number;
  seatsCount: number;
  totalLength: number;
  totalWidth: number;
  totalArea: number;
  stageLength?: number;
  stageWidth?: number;
  stageArea?: number;
  maxNumberOfSeatsRows: number;
  maxNumberOfSeatsColumns: number;
  numberOfSeats: number;
  maxNumberOfSeats: number;
  type?: HallType;
  seats: Seat[];
};
