import { IModel } from "../abstract/IModel";
import { EventCategory } from "./EventCategory";
import { Hall } from "./Hall";
import { Ticket } from "./Ticket";

export type EventEntity = IModel & {
  title: string;
  shortDescription: string;
  eventStatus: string;
  start: string;
  end: string;
  duration: string;
  photoName: string;
  photoEndpoint: string;
  category?: EventCategory;
  longDescription?: string;
  hall?: Hall;
  tickets: Ticket[];
};
