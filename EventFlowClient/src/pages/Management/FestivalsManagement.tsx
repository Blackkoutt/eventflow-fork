import { useEffect, useState } from "react";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { Festival } from "../../models/response_models";
import { ScheduleXCalendar, useCalendarApp } from "@schedule-x/react";
import { createViewWeek, createViewMonthGrid } from "@schedule-x/calendar";
import "@schedule-x/theme-default/dist/calendar.css";
import { createEventsServicePlugin } from "@schedule-x/events-service";
import DateFormatter from "../../helpers/DateFormatter";
import { createDragAndDropPlugin } from "@schedule-x/drag-and-drop";
import Button, { ButtonStyle } from "../../components/buttons/Button";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import CreateFestivalDialog from "../../components/management/festivals/CreateFestivalDialog";
import { useDialogs } from "../../hooks/useDialogs";
import FestivalClickDialog from "../../components/management/festivals/FestivalClickDialog";

const FestivalsManagement = () => {
  const { data: items, get: getItems } = useApi<Festival>(ApiEndpoint.Festival);

  const [festivals, setFestivals] = useState<Festival[]>([]);

  useEffect(() => {
    getItems({ id: undefined, queryParams: undefined });
  }, []);
  useEffect(() => {
    console.log(items);
    setFestivals(items.filter((item) => item.festivalStatus === "Active"));
  }, [items]);

  const calendar = useCalendarApp({
    views: [createViewMonthGrid(), createViewWeek()],
    selectedDate: DateFormatter.FormatDateForCalendar(new Date()).split(" ")[0],
    callbacks: {
      onEventClick(calendarEvent) {
        onModify(calendarEvent as Festival);
      },
    },
    plugins: [createEventsServicePlugin(), createDragAndDropPlugin()],
    locale: "pl-PL",
  });

  useEffect(() => {
    console.log(festivals);
    if (festivals && festivals.length > 0) {
      const formattedFestivals = festivals.map((e) => ({
        ...e,
        start: DateFormatter.FormatDateForCalendar(e.start),
        end: DateFormatter.FormatDateForCalendar(e.end),
      }));
      console.log(festivals);
      calendar.eventsService.set(formattedFestivals);
      console.log(calendar.eventsService.getAll());
    }
  }, [festivals]);

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
  } = useDialogs<Festival>();

  const reloadItemsAfterSuccessDialog = () => {
    console.log("reload");
    closeDialogsAndSetValuesToDefault();
    getItems({ id: undefined, queryParams: undefined });
  };

  return (
    items && (
      <div className="w-full px-5 flex flex-col justify-center items-center gap-4">
        <CreateFestivalDialog
          minWidth={1000}
          maxWidth={1000}
          ref={createDialog}
          onDialogClose={onDialogClose}
          onDialogSuccess={reloadItemsAfterSuccessDialog}
        />
        <FestivalClickDialog
          minWidth={700}
          ref={modifyDialog}
          item={itemToModify}
          onDialogSuccess={reloadItemsAfterSuccessDialog}
          onDialogClose={onDialogClose}
        />
        <div className="flex flex-row justify-between items-center w-full px-6">
          <h2 className="text-textPrimary">Festiwale</h2>
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
export default FestivalsManagement;
