import { IModel } from "../abstract/IModel";
import { EventEntity } from "./EventEntity";
import { MediaPatron } from "./MediaPatron";
import { Organizer } from "./Organizer";
import { Sponsor } from "./Sponsor";
import { Ticket } from "./Ticket";

export type Festival = IModel & {
  title: string;
  shortDescription: string;
  festivalStatus: string;
  start: string;
  end: string;
  duration: string;
  photoName: string;
  photoEndpoint: string;
  longDescription?: string;
  events: EventEntity[];
  mediaPatrons: MediaPatron[];
  organizers: Organizer[];
  sponsors: Sponsor[];
  tickets: Ticket[];
};
