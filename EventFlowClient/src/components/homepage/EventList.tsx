import { useEffect } from "react";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { EventEntity } from "../../models/response_models";
import EventCard from "../common/EventCard";

const EventList = () => {
  const { data: events, get: getEvents } = useApi<EventEntity>(ApiEndpoint.Event);

  useEffect(() => {
    const eventStartDateQueryParams = {
      sortBy: "StartDate",
      sortDirection: "ASC",
      status: "Active",
      pageNumber: 1,
      pageSize: 4,
    };
    getEvents({ id: undefined, queryParams: eventStartDateQueryParams });
  }, []);
  console.log(events);

  return (
    <>
      <div className="w-full grid grid-cols-1 xl:grid-cols-2 gap-6">
        {events.map((event) => (event ? <EventCard key={event.id} event={event} /> : null))}
      </div>
    </>
  );
};

export default EventList;
