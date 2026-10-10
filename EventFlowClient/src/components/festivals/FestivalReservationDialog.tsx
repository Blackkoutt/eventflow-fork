import { forwardRef, useEffect, useState } from "react";
import { toast } from "react-toastify";
import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  EventEntity,
  EventPass,
  Festival,
  Hall,
  PaymentType,
  Reservation,
  SeatType,
  Ticket,
} from "../../models/response_models";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { SelectOption } from "../../helpers/SelectOptions";
import { HTTPStatusCode } from "../../helpers/enums/HTTPStatusCode";
import { useAuth } from "../../context/AuthContext";
import Dialog from "../common/Dialog";
import {
  ReservationRequest,
  ReservationSchema,
} from "../../models/create_schemas/ReservationSchema";
import FestivalEventSeats from "./FestivalEventSeats";
import SeatTypesLegend from "../reservations/SeatTypesLegend";
import ReservationTypeSelects from "../reservations/ReservationTypeSelects";
import ReservationDialogFooter from "../reservations/ReservationDialogFooter";
import ReservationSummary from "../reservations/ReservationSummary";
import usePaymentTypeSelectOptions from "../../hooks/usePaymentTypeSelectOptions";
import LabelText from "../common/LabelText";
import { TabPanel, TabView } from "primereact/tabview";

interface FestivalReservationDialogProps {
  festival?: Festival;
  onDialogConfirm: () => void;
  onDialogClose: () => void;
}

type HallWithReservedSeats = {
  hall: Hall;
  seatNr: number[];
};
type EventReservedSeat = {
  event: EventEntity;
  seatNr?: number;
};

const FestivalReservationDialog = forwardRef<HTMLDialogElement, FestivalReservationDialogProps>(
  ({ festival, onDialogClose, onDialogConfirm }: FestivalReservationDialogProps, ref) => {
    const { data: hallWithDetails, get: getHallWithDetails } = useApi<Hall>(ApiEndpoint.Hall);
    const { data: seatTypes, get: getSeatTypes } = useApi<SeatType>(ApiEndpoint.SeatType);
    const {
      data: reservations,
      get: getReservations,
      post: postReservation,
    } = useApi<Reservation, ReservationRequest>(ApiEndpoint.Reservation);
    const { data: eventPasses, get: getEventPasses } = useApi<EventPass>(ApiEndpoint.EventPass);
    const { data: paymentTypes, get: getPaymentTypes } = useApi<PaymentType>(
      ApiEndpoint.PaymentType
    );

    //console.log(eventEntity);
    const [ticketTypeSelectOptions, setTicketTypeSelectOptions] = useState<SelectOption[]>([]);
    //const [hallSeats, setHallSeats] = useState<Seat[]>([]);
    //const [reservedSeatNumbers, setReservedSeatNumbers] = useState<number[]>([]);
    const [userReservedSeats, setUserReservedSeats] = useState<EventReservedSeat[]>([]);
    const { authenticated } = useAuth();
    const paymentTypeSelectOptions = usePaymentTypeSelectOptions(paymentTypes, eventPasses);

    useEffect(() => {
      const onInit = async () => {
        if (!festival) return;

        // Pobranie danych dla wszystkich wydarzeń w festiwalu
        for (const e of festival.events) {
          await getHallWithDetails({ id: e.hall?.id, queryParams: undefined });
          await getReservations({ id: undefined, queryParams: { eventId: e?.id, getAll: true } });
        }

        // Pobranie typów miejsc i płatności
        await Promise.all([
          getSeatTypes({ id: undefined, queryParams: undefined }),
          getPaymentTypes({ id: undefined, queryParams: undefined }),
        ]);

        // Pobranie aktywnych event passów, jeśli użytkownik jest zalogowany
        if (authenticated) {
          await getEventPasses({ id: undefined, queryParams: { status: "Active" } });
        }
      };
      onInit();
    }, [festival]);

    const [hallReservations, setHallReservations] = useState<HallWithReservedSeats[]>([]);
    const [halls, setHalls] = useState<Hall[]>([]);

    // Set Hall Seats
    useEffect(() => {
      if (hallWithDetails !== undefined && hallWithDetails.length !== 0) {
        setHalls((prev) => {
          const prevHalls = prev || [];
          const exists = prevHalls.some((h) => h.id === hallWithDetails[0].id);
          if (exists) return prevHalls;
          return [...prevHalls, hallWithDetails[0]];
        });
        setHallReservations((prev) => {
          const prevHalls = prev || [];
          const exists = prevHalls.some((h) => h.hall.id === hallWithDetails[0]?.id);
          if (exists) return prevHalls;
          return [...prevHalls, { hall: hallWithDetails[0], seatNr: [] } as HallWithReservedSeats];
        });
      }
    }, [hallWithDetails]);

    // Set Reserved Seats
    useEffect(() => {
      console.log(reservations);
      if (!reservations.length || !hallWithDetails.length) return;

      const hallWithAnyRes = halls.find((h) => h.id == reservations[0]?.ticket?.event?.hall?.id);

      const seats = reservations
        .filter((r) => r.reservationStatus == "Active")
        .flatMap((r) => r.seats.map((seat) => seat.seatNr));

      setHallReservations((prev) => {
        const prevReservations = prev || [];

        const hallIndex = prevReservations.findIndex((h) => h.hall.id === hallWithAnyRes?.id);

        if (hallIndex !== -1) {
          const updatedReservations = [...prevReservations];
          updatedReservations[hallIndex] = {
            ...updatedReservations[hallIndex],
            seatNr: seats,
          };
          return updatedReservations;
        }

        return [
          ...prevReservations,
          { hall: hallWithAnyRes, seatNr: seats } as HallWithReservedSeats,
        ];
      });
      //   setHallReservations((prev) => {
      //     const prevReservations = prev || [];
      //     const exists = prevReservations.some((h) => h.hall.id === hallWithAnyRes?.id);
      //     if (exists) return prevReservations;
      //     return [
      //       ...prevReservations,
      //       { hall: hallWithAnyRes, seatNr: seats } as HallWithReservedSeats,
      //     ];
      //   });

      //setReservedSeatNumbers(seats);
    }, [reservations]);

    // Ticket Type
    useEffect(() => {
      if (festival) {
        const uniqueTicketTypes = new Set<string>(); // Używamy Set, aby przechować unikalne nazwy
        const selectOptions: SelectOption[] = festival.tickets
          .filter((t: Ticket) => t.isFestival) // Filtrujemy tylko bilety festiwalowe
          .filter((t: Ticket) => {
            // Sprawdzamy, czy nazwa biletu już była dodana
            if (uniqueTicketTypes.has(t.ticketType.name)) {
              return false; // Pomijamy powtórzenia
            }
            uniqueTicketTypes.add(t.ticketType.name); // Dodajemy nową nazwę do Set
            return true;
          })
          .map((t: Ticket) => ({
            value: t.id,
            option: `${t.ticketType.name} (${t.price} zł)`,
          }));

        setTicketTypeSelectOptions(selectOptions);
      }
    }, [festival]);

    const toggleUserReservedSeat = (eventEntity: EventEntity, seatNr: number) => {
      setUserReservedSeats((prev) => {
        const eventReservedSeat = prev.find((es) => es.event.id === eventEntity.id);
        if (!eventReservedSeat) {
          return [...prev, { event: eventEntity, seatNr }];
        }

        return prev.map((es) =>
          es.event.id === eventEntity.id
            ? { ...es, seatNr: es.seatNr === seatNr ? undefined : seatNr }
            : es
        );
      });
    };

    const methods = useForm<ReservationRequest>({
      resolver: zodResolver(ReservationSchema),
    });
    const { handleSubmit, formState, watch } = methods;
    const { errors, isSubmitting } = formState;

    const selectedTicketTypeId = watch().ticketId;
    const [selectedTicketType, setSelectedTicketType] = useState<Ticket>();
    const ticketPrice = selectedTicketType?.price ?? 0;

    useEffect(() => {
      const selectedTicket = festival?.tickets.filter((t) => t.id == selectedTicketTypeId)[0];
      setSelectedTicketType(selectedTicket);
    }, [selectedTicketTypeId]);

    const onSubmit: SubmitHandler<ReservationRequest> = async (data) => {
      if (festival != null && festival != undefined) {
        if (authenticated) {
          const reservedSeatsNr = userReservedSeats.map((es) => es.seatNr);
          const hasUndefined = reservedSeatsNr.some((seat) => seat === undefined);
          if (hasUndefined || reservedSeatsNr.length < 0) {
            toast.error("Wybierz conajmniej jedno miejsce na każde wydarzenie");
            return;
          }

          const reservedSeatsDetails = userReservedSeats
            .map((es) => {
              if (es.event.hall) {
                return halls
                  .filter((h) => h.id === es.event.hall?.id)
                  .flatMap((hall) => hall.seats.filter((seat) => seat.seatNr === es.seatNr));
              }
              return [];
            })
            .flat();

          data.isReservationForFestival = true;
          data.seatsIds = reservedSeatsDetails.map((s) => s.id);

          const request = postReservation({ body: data });
          await toast.promise(request, {
            pending: "Trwa przetwarzanie rezerwacji",
            error: "Wystąpił błąd podczas kupna biletu",
          });
          if ((await request) == HTTPStatusCode.Ok) {
            toast.success("Bilet został zakupiony pomyślnie");
            onDialogConfirm();
          }
        } else {
          toast.error("Zaloguj się aby dokonać rezerwacji");
        }
      }
    };

    return (
      <div className="h-full">
        {festival && (
          <Dialog ref={ref} minWidth={750} maxWidth={1200} top={220}>
            <FormProvider {...methods}>
              <form
                className="flex flex-row justify-center items-start gap-3 py-3"
                onSubmit={handleSubmit(onSubmit)}
              >
                <div className="flex flex-col justify-center items-center px-5 pb-2 gap-6">
                  <div className="flex flex-col justify-center items-center gap-2">
                    <h2>Rezerwacja miejsc - "{festival.title}"</h2>
                    <p className="text-textPrimary text-base text-center">
                      Wybierz poniżej rezerwowane miejsca oraz typ biletu
                    </p>
                  </div>
                  <div className="flex flex-col justify-center items-center gap-3">
                    {/* <div>stage</div> */}
                    <div className="flex flex-col gap-2">
                      <TabView>
                        {festival.events.map((e: EventEntity, index: number) => (
                          <TabPanel key={index} header={e.title}>
                            <FestivalEventSeats
                              hallReservation={hallReservations.find(
                                (h) => h.hall.id === e.hall?.id
                              )}
                              selectedSeatNumber={
                                userReservedSeats.find((es) => es.event.id === e.id)?.seatNr
                              }
                              onSeatClick={(seatNr) => toggleUserReservedSeat(e, seatNr)}
                            />
                          </TabPanel>
                        ))}
                      </TabView>
                    </div>
                  </div>
                </div>
                <div className="self-stretch min-w-[1px] bg-black"></div>
                <div className="pl-3 flex flex-col justify-centers items-start gap-1">
                  <SeatTypesLegend seatTypes={seatTypes} />
                  <ReservationTypeSelects
                    paymentTypeSelectOptions={paymentTypeSelectOptions}
                    ticketTypeSelectOptions={ticketTypeSelectOptions}
                    errors={errors}
                  />
                  <ReservationSummary ticketPrice={ticketPrice} totalCost={ticketPrice}>
                    {festival.events.map((e) => (
                      <LabelText
                        key={e.id}
                        emptyMessage=""
                        label={`\"${e.title}\" nr miejsca:`}
                        text={userReservedSeats.find((es) => es.event.id == e.id)?.seatNr}
                      />
                    ))}
                  </ReservationSummary>
                  <ReservationDialogFooter
                    isSubmitting={isSubmitting}
                    onDialogClose={onDialogClose}
                  />
                </div>
              </form>
            </FormProvider>
          </Dialog>
        )}
      </div>
    );
  }
);
export default FestivalReservationDialog;
