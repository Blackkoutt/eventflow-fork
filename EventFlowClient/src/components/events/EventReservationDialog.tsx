import { forwardRef, useEffect, useState } from "react";
import { toast } from "react-toastify";
import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  EventEntity,
  EventPass,
  Hall,
  PaymentType,
  Reservation,
  Seat,
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
import SeatsGrid from "../reservations/SeatsGrid";
import SeatTypesLegend from "../reservations/SeatTypesLegend";
import ReservationTypeSelects from "../reservations/ReservationTypeSelects";
import ReservationDialogFooter from "../reservations/ReservationDialogFooter";
import ReservationSummary from "../reservations/ReservationSummary";
import usePaymentTypeSelectOptions from "../../hooks/usePaymentTypeSelectOptions";
import LabelText from "../common/LabelText";

interface EventReservationDialogProps {
  eventEntity?: EventEntity;
  onDialogConfirm: () => void;
  onDialogClose: () => void;
}

const EventReservationDialog = forwardRef<HTMLDialogElement, EventReservationDialogProps>(
  ({ eventEntity, onDialogClose, onDialogConfirm }: EventReservationDialogProps, ref) => {
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
    const [hallSeats, setHallSeats] = useState<Seat[]>([]);
    const [reservedSeatNumbers, setReservedSeatNumbers] = useState<number[]>([]);
    const [userReservedSeats, setUserReservedSeats] = useState<number[]>([]);
    const { authenticated } = useAuth();
    const paymentTypeSelectOptions = usePaymentTypeSelectOptions(paymentTypes, eventPasses);

    useEffect(() => {
      if (eventEntity !== undefined) {
        getHallWithDetails({ id: eventEntity.hall?.id, queryParams: undefined });
        getSeatTypes({ id: undefined, queryParams: undefined });
        getReservations({ id: undefined, queryParams: { eventId: eventEntity?.id, getAll: true } });
        getPaymentTypes({ id: undefined, queryParams: undefined });
        if (authenticated) {
          getEventPasses({ id: undefined, queryParams: { status: "Active" } });
        }
      }
    }, [eventEntity]);

    // Ticket Type
    useEffect(() => {
      if (eventEntity) {
        console.log(eventEntity.tickets);
        const selectOptions: SelectOption[] = eventEntity.tickets
          .filter((t: Ticket) => !t.isFestival)
          .map(
            (t: Ticket) =>
              ({
                value: t.id,
                option: `${t.ticketType.name} (${t.price} zł)`,
              } as SelectOption)
          );
        setTicketTypeSelectOptions(selectOptions);
      }
    }, [eventEntity]);

    // Set Reserved Seats
    useEffect(() => {
      const seats = reservations
        .filter((r) => r.reservationStatus == "Active")
        .flatMap((r) => r.seats.map((seat) => seat.seatNr));
      console.log(seats);
      setReservedSeatNumbers(seats);
    }, [reservations]);

    // Set Hall Seats
    useEffect(() => {
      if (hallWithDetails !== undefined && hallWithDetails.length !== 0) {
        setHallSeats(hallWithDetails[0].seats);
      }
    }, [hallWithDetails]);

    const hallDetails = hallWithDetails[0]?.hallDetails;

    const toggleUserReservedSeat = (seatNr: number) => {
      setUserReservedSeats((prev) =>
        prev.includes(seatNr) ? prev.filter((s) => s != seatNr) : [...prev, seatNr]
      );
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
      const selectedTicket = eventEntity?.tickets.filter((t) => t.id == selectedTicketTypeId)[0];
      setSelectedTicketType(selectedTicket);
    }, [selectedTicketTypeId]);

    const [totalCost, setTotalCost] = useState(0);
    const [additionalPaymentCost, setAdditionalPaymentCost] = useState(0);
    const [totalAdditionalPaymentPercentage, setTotalAdditionalPaymentPercentage] = useState(0);
    useEffect(() => {
      if (selectedTicketType) {
        const reservedSeats = hallSeats.filter((s) => userReservedSeats.includes(s.seatNr));
        let paymentAmount = 0;
        let totalAdditionalPayment = 0;
        reservedSeats.forEach((s) => {
          if (s.seatType) {
            let additionalPayment =
              selectedTicketType?.price * (s.seatType?.addtionalPaymentPercentage / 100);
            totalAdditionalPayment += additionalPayment;
            paymentAmount += selectedTicketType?.price + additionalPayment;
          }
        });
        setTotalCost(parseFloat(paymentAmount.toFixed(2)));
        setAdditionalPaymentCost(parseFloat(totalAdditionalPayment.toFixed(2)));
        setTotalAdditionalPaymentPercentage(
          parseFloat(((totalAdditionalPayment / selectedTicketType?.price) * 100).toFixed(2))
        );
      }
    }, [selectedTicketType, userReservedSeats]);

    const onSubmit: SubmitHandler<ReservationRequest> = async (data) => {
      if (eventEntity != null && eventEntity != undefined) {
        if (authenticated) {
          const reservedSeats = hallSeats.filter((s) => userReservedSeats.includes(s.seatNr));
          if (reservedSeats.length > 0) {
            data.isReservationForFestival = false;
            data.seatsIds = reservedSeats.map((s) => s.id);
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
            toast.error("Wybierz conajmniej jedno miejsce");
          }
        } else {
          toast.error("Zaloguj się aby dokonać rezerwacji");
        }
      }
    };

    return (
      <div className="h-full">
        {eventEntity && (
          <Dialog ref={ref} minWidth={750} maxWidth={1200} top={220}>
            <FormProvider {...methods}>
              <form
                className="flex flex-row justify-center items-start gap-3 py-3"
                onSubmit={handleSubmit(onSubmit)}
              >
                <div className="flex flex-col justify-center items-center px-5 pb-2 gap-6">
                  <div className="flex flex-col justify-center items-center gap-2">
                    <h2>Rezerwacja miejsc - "{eventEntity.title}"</h2>
                    <p className="text-textPrimary text-base text-center">
                      Wybierz poniżej rezerwowane miejsca oraz typ biletu
                    </p>
                  </div>
                  <div className="flex flex-col justify-center items-center gap-3">
                    {/* <div>stage</div> */}
                    <div className="flex flex-col gap-2">
                      {hallDetails?.maxNumberOfSeatsRows !== undefined &&
                        hallDetails?.maxNumberOfSeatsColumns !== undefined && (
                          <SeatsGrid
                            maxNumberOfSeatsRows={hallDetails.maxNumberOfSeatsRows}
                            maxNumberOfSeatsColumns={hallDetails.maxNumberOfSeatsColumns}
                            seats={hallSeats}
                            reservedSeatNumbers={reservedSeatNumbers}
                            selectedSeatNumbers={userReservedSeats}
                            onSeatClick={toggleUserReservedSeat}
                          />
                        )}
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
                  <ReservationSummary
                    ticketPrice={ticketPrice}
                    totalCost={totalCost}
                    additionalPayment={{
                      percentage: totalAdditionalPaymentPercentage,
                      cost: additionalPaymentCost,
                    }}
                  >
                    <LabelText label="Wybrane nr miejsc:" text={userReservedSeats.join(", ")} />
                    <LabelText label="Ilość miejsc:" text={userReservedSeats.length} />
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
export default EventReservationDialog;
