import { useEffect, useState } from "react";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { EventEntity } from "../../models/response_models";
import { ScheduleXCalendar, useCalendarApp } from "@schedule-x/react";
import { createViewWeek, createViewMonthGrid } from "@schedule-x/calendar";
import "@schedule-x/theme-default/dist/calendar.css";
import { createEventsServicePlugin } from "@schedule-x/events-service";
import DateFormatter from "../../helpers/DateFormatter";
import Button, { ButtonStyle } from "../../components/buttons/Button";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import CreateEventDialog from "../../components/management/events/CreateEventDialog";
import { useDialogs } from "../../hooks/useDialogs";
import EventClickDialog from "../../components/management/events/EventClickDialog";

const EventsManagement = () => {
  const { data: items, get: getItems } = useApi<EventEntity>(ApiEndpoint.Event);

  const [events, setEvents] = useState<EventEntity[]>([]);

  useEffect(() => {
    getItems({ id: undefined, queryParams: undefined });
  }, []);

  useEffect(() => {
    setEvents(items.filter((item) => item.eventStatus === "Active"));
  }, [items]);

  const calendar = useCalendarApp({
    views: [createViewMonthGrid(), createViewWeek()],
    selectedDate: DateFormatter.FormatDateForCalendar(new Date()).split(" ")[0],
    callbacks: {
      onEventClick(calendarEvent) {
        onModify(calendarEvent as EventEntity);
        //console.log("onEventClick", calendarEvent as EventEntity);
      },
    },
    plugins: [createEventsServicePlugin()],
    locale: "pl-PL",
  });

  useEffect(() => {
    if (events && events.length > 0) {
      const formattedEvents = events.map((e) => ({
        ...e,
        start: DateFormatter.FormatDateForCalendar(e.start),
        end: DateFormatter.FormatDateForCalendar(e.end),
        location: `Sala nr ${e.hall?.hallNr}`,
      }));
      console.log(events);
      calendar.eventsService.set(formattedEvents);
      console.log(calendar.eventsService.getAll());
    }
  }, [events]);

  const {
    createDialog,
    detailsDialog,
    modifyDialog,
    itemToDelete,
    itemToDetails,
    itemToModify,
    onDialogClose,
    onDelete,
    onModify,
    onCreate,
    onDetails,
    closeDialogsAndSetValuesToDefault,
  } = useDialogs<EventEntity>();

  const reloadItemsAfterSuccessDialog = () => {
    console.log("reload");
    closeDialogsAndSetValuesToDefault();
    getItems({ id: undefined, queryParams: undefined });
  };

  return (
    items && (
      <div className="w-full px-5 flex flex-col justify-center items-center gap-4">
        <CreateEventDialog
          ref={createDialog}
          onDialogClose={onDialogClose}
          onDialogSuccess={reloadItemsAfterSuccessDialog}
        />
        <EventClickDialog
          minWidth={700}
          ref={modifyDialog}
          item={itemToModify}
          onDialogSuccess={reloadItemsAfterSuccessDialog}
          onDialogClose={onDialogClose}
        />

        <div className="flex flex-row justify-between items-center w-full px-6">
          <h2 className="text-textPrimary">Wydarzenia</h2>
          <Button
            text="Dodaj"
            width={110}
            icon={faPlus}
            iconSize={18}
            height={45}
            style={ButtonStyle.Primary}
            rounded="rounded-md"
            action={onCreate}
          />
        </div>
        <div className="w-full">
          <ScheduleXCalendar calendarApp={calendar} />
        </div>
      </div>
    )
  );
};
export default EventsManagement;
