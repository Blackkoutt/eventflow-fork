import { useParams } from "react-router-dom";
import { EventEntity } from "../models/response_models";
import useApi from "../hooks/useApi";
import { ApiEndpoint } from "../helpers/enums/ApiEndpointEnum";
import { useEffect, useRef, useState } from "react";
import ApiClient from "../services/api/ApiClientService";
import DateFormatter from "../helpers/DateFormatter";
import { DateFormat } from "../helpers/enums/DateFormatEnum";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faLocationDot, faTicket } from "@fortawesome/free-solid-svg-icons";
import Button, { ButtonStyle } from "../components/buttons/Button";
import EventReservationDialog from "../components/events/EventReservationDialog";

const EventPage = () => {
  const { data: items, get: getItems } = useApi<EventEntity>(ApiEndpoint.Event);
  const [eventEntity, setEventEntity] = useState<EventEntity>();
  const reservationDialog = useRef<HTMLDialogElement>(null);
  const { eventId } = useParams();

  useEffect(() => {
    getItems({ id: eventId, queryParams: undefined });
  }, [eventId]);

  useEffect(() => {
    if (items && items.length > 0) {
      setEventEntity(items[0]);
    }
  }, [items]);

  const FormatEventDate = (): string[] => {
    if (eventEntity != undefined) {
      const date = DateFormatter.FormatDate(eventEntity.start, DateFormat.DayDateTime).split(" ");
      return date;
    }
    return [];
  };
  const [dayOfWeek, date, timeLabel, time] = FormatEventDate();

  return (
    eventEntity && (
      <div className="flex flex-col w-[80%] justify-start items-start my-10 pb-6 rounded-md shadow-xl overflow-hidden">
        <EventReservationDialog
          ref={reservationDialog}
          eventEntity={eventEntity}
          onDialogClose={() => reservationDialog.current?.close()}
          onDialogConfirm={() => reservationDialog.current?.close()}
        />
        <div className="relative w-full">
          <img
            className="object-cover w-full max-h-[300px] shadow-md"
            src={ApiClient.GetPhotoEndpoint(eventEntity.photoEndpoint)}
            alt={`Zdjęcie wydarzenia ${eventEntity.title}`}
          />
          <div
            className="absolute w-[205px] h-[121px] left-6 -bottom-12 shadow-sm flex flex-col justify-center items-center"
            style={{
              background: `linear-gradient(to bottom, #987EFE, ${eventEntity.category?.color})`,
            }}
          >
            <div className="px-2 py-2 flex flex-col justify-center items-center gap-1">
              <p className="text-shadow text-white font-semibold">{dayOfWeek}</p>
              <p className="text-shadow text-white font-semibold text-2xl">{date}</p>
              <div className="bg-white h-[2px] shadow-md w-full"></div>
              <p className="text-shadow text-white font-semibold text-2xl">
                {timeLabel} {time}
              </p>
            </div>
          </div>
        </div>

        <div className="flex flex-row justify-between items-start w-full pr-3 pt-1">
          <article className="flex flex-col justify-start items-start gap-2 max-w-[76%]">
            <div className="flex flex-col justify-start items-start gap-1 pl-[250px] pt-2">
              <div className="flex flex-row justify-start items-center gap-1">
                <i
                  className={eventEntity.category?.icon}
                  style={{ color: eventEntity.category?.color, fontSize: "24px" }}
                ></i>
                <p
                  className="text-[16px] font-semibold"
                  style={{ color: eventEntity.category?.color }}
                >
                  {eventEntity.category?.name.toLocaleUpperCase()}
                </p>
              </div>
              <h3 className="text-[24px] font-semibold ">{eventEntity.title}</h3>
            </div>
            <p className="pl-6 w-full text-textPrimary">{eventEntity.longDescription}</p>
          </article>

          <div className="flex flex-col justify-center items-center gap-[14px] min-w-[180px] py-4">
            <div className="flex flex-row justify-center items-center gap-3">
              <FontAwesomeIcon
                icon={faLocationDot}
                style={{ color: `${eventEntity.category?.color}`, width: "22px", height: "22px" }}
              />
              <p className="font-semibold text-[16px] text-center text-textPrimary">
                Sala: nr {eventEntity.hall?.hallNr}
              </p>
            </div>
            {new Date(eventEntity.end).getTime() > Date.now() && (
              <div className="flex flex-row justify-center items-center gap-3">
                <FontAwesomeIcon
                  icon={faTicket}
                  style={{ color: `${eventEntity.category?.color}`, width: "22px", height: "22px" }}
                />
                <p className="font-semibold text-[16px] text-center text-textPrimary">
                  Cena od: {Math.min(...eventEntity.tickets.map((ticket) => ticket.price))} zł
                </p>
              </div>
            )}
            {new Date(eventEntity.end).getTime() > Date.now() && (
              <div>
                <Button
                  text="Kup bilet"
                  width={130}
                  height={43}
                  fontSize={17}
                  isFontSemibold={true}
                  style={ButtonStyle.Default}
                  action={() => reservationDialog.current?.showModal()}
                />
              </div>
            )}
          </div>
        </div>
      </div>
    )
  );
};
export default EventPage;
