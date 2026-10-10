import { forwardRef, useEffect, useState } from "react";
import { faCouch, faShoppingCart, faXmark } from "@fortawesome/free-solid-svg-icons";
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
  Seat,
  SeatType,
  Ticket,
} from "../../models/response_models";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { SelectOption } from "../../helpers/SelectOptions";
import { HTTPStatusCode } from "../../helpers/enums/HTTPStatusCode";
import MessageText from "../common/MessageText";
import { MessageType } from "../../helpers/enums/MessageTypeEnum";
import Button, { ButtonStyle } from "../buttons/Button";
import FormButton from "../common/forms/FormButton";
import { useAuth } from "../../context/AuthContext";
import Dialog from "../common/Dialog";
import {
  ReservationRequest,
  ReservationSchema,
} from "../../models/create_schemas/ReservationSchema";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Select from "../common/forms/Select";
import { ToRoman } from "../../helpers/NumberConverter";
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
    const [paymentTypeSelectOptions, setPaymentTypeSelectOptions] = useState<SelectOption[]>([]);
    //const [hallSeats, setHallSeats] = useState<Seat[]>([]);
    //const [reservedSeatNumbers, setReservedSeatNumbers] = useState<number[]>([]);
    const [userReservedSeats, setUserReservedSeats] = useState<EventReservedSeat[]>([]);
    const { authenticated } = useAuth();

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

    // PaymentType
    useEffect(() => {
      const selectOptions: SelectOption[] = paymentTypes.map(
        (pt: PaymentType) =>
          ({
            value: pt.id,
            option: pt.name,
          } as SelectOption)
      );
      setPaymentTypeSelectOptions(selectOptions);
    }, [paymentTypes]);

    // Filter PaymentType by EventPass
    useEffect(() => {
      if (eventPasses && eventPasses.length == 0) {
        setPaymentTypeSelectOptions((prev) => prev.filter((pt) => pt.option != "Karnet"));
      }
    }, [eventPasses]);

    // Print Seats
    const getSeats = (eventEntity: EventEntity, hallWithSeats: HallWithReservedSeats) => {
      if (hallWithSeats != undefined) {
        const maxNumberOfSeatsRows = hallWithSeats.hall.hallDetails?.maxNumberOfSeatsRows;
        const maxNumberOfSeatsColumns = hallWithSeats.hall.hallDetails?.maxNumberOfSeatsColumns;
        if (maxNumberOfSeatsRows != undefined && maxNumberOfSeatsColumns != undefined) {
          return (
            <div className="flex flex-col gap-[10px]">
              <div className="flex flex-row justify-center items-center gap-2">
                <div className="flex flex-col justify-center items-center gap-[24.5px] pt-[28px]">
                  {Array.from({ length: maxNumberOfSeatsRows }, (_, rowIndex) => {
                    return (
                      <div className="text-[14px] text-textPrimary">{ToRoman(rowIndex + 1)}</div>
                    );
                  })}
                </div>
                <div className="flex flex-col justify-start items-start gap-2">
                  <div
                    className="grid gap-[10px]"
                    style={{
                      gridTemplateColumns: `repeat(${maxNumberOfSeatsColumns}, 37px)`,
                    }}
                  >
                    {Array.from({ length: maxNumberOfSeatsColumns }, (_, colIndex) => {
                      return (
                        <div className="text-[14px] text-textPrimary text-center">
                          {ToRoman(colIndex + 1)}
                        </div>
                      );
                    })}
                  </div>
                  <div className="flex flex-col gap-[10px]">
                    {Array.from({ length: maxNumberOfSeatsRows }, (_, rowIndex) => (
                      <div
                        key={rowIndex}
                        className="grid gap-[10px]"
                        style={{
                          gridTemplateColumns: `repeat(${maxNumberOfSeatsColumns}, minmax(0, 1fr))`,
                        }}
                      >
                        {Array.from({ length: maxNumberOfSeatsColumns }, (_, colIndex) => {
                          const seatNumber = rowIndex * maxNumberOfSeatsColumns + colIndex + 1;
                          const seat = hallWithSeats.hall.seats.find((s) => s.seatNr == seatNumber);
                          const reservedSeat = hallWithSeats.seatNr.find((nr) => nr == seatNumber);
                          const userReservedSeat =
                            userReservedSeats.find((es) => es.event.id === eventEntity.id)
                              ?.seatNr === seat?.seatNr
                              ? seat?.seatNr
                              : undefined;

                          //const isAvailable = seatNumbers.includes(seatNumber);

                          return (
                            <div
                              key={`${rowIndex}-${colIndex}`}
                              className={`border px-[8px] py-[4px] text-center text-[12px] flex flex-col justify-center gap-[2px] items-center ${
                                seat
                                  ? reservedSeat
                                    ? "border-red-600 text-black bg-red-600"
                                    : "border-gray-300 text-gray-500 hover:cursor-pointer"
                                  : "border-gray-400 bg-gray-200 text-gray-500"
                              }`}
                              style={{
                                borderColor:
                                  reservedSeat == undefined ? seat?.seatType?.seatColor : undefined,
                                color:
                                  userReservedSeat != undefined
                                    ? "white"
                                    : reservedSeat != undefined
                                    ? "black"
                                    : seat?.seatType?.seatColor,
                                backgroundColor:
                                  userReservedSeat != undefined
                                    ? seat?.seatType?.seatColor
                                    : undefined,
                              }}
                              onClick={() => {
                                if (seat && !reservedSeat) {
                                  const eventReservedSeats = userReservedSeats?.find(
                                    (es) => es.event.id === eventEntity.id
                                  );

                                  // Jeśli rezerwacja dla wydarzenia istnieje i seatNr jest w rezerwacjach
                                  if (
                                    eventReservedSeats &&
                                    eventReservedSeats.seatNr === seat.seatNr
                                  ) {
                                    setUserReservedSeats(
                                      (prev) =>
                                        prev?.map((es) =>
                                          es.event.id === eventEntity.id
                                            ? {
                                                ...es,
                                                seatNr:
                                                  es.seatNr === seat.seatNr ? undefined : es.seatNr, // Usuwamy rezerwację, jeśli miejsce jest już zarezerwowane
                                              }
                                            : es
                                        ) || []
                                    );
                                  } else {
                                    setUserReservedSeats((prev) => {
                                      const updatedSeats = eventReservedSeats
                                        ? prev?.map((es) =>
                                            es.event.id === eventEntity.id
                                              ? { ...es, seatNr: seat.seatNr }
                                              : es
                                          )
                                        : [
                                            ...(prev || []),
                                            { event: eventEntity, seatNr: seat.seatNr }, // Dodajemy rezerwację, jeśli jeszcze jej nie ma
                                          ];

                                      return updatedSeats;
                                    });
                                  }
                                }
                              }}
                            >
                              <FontAwesomeIcon icon={faCouch} fontSize={12} />
                              <p className="hover:cursor-default text-[11px] font-semibold select-none">
                                {seatNumber}
                              </p>
                            </div>
                          );
                        })}
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            </div>
          );
        }
      }
    };

    const methods = useForm<ReservationRequest>({
      resolver: zodResolver(ReservationSchema),
    });
    const { handleSubmit, formState, watch } = methods;
    const { errors, isSubmitting } = formState;

    const selectedTicketTypeId = watch().ticketId;
    const [selectedTicketType, setSelectedTicketType] = useState<Ticket>();

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
                        {festival.events.map((e: EventEntity, index: number) => {
                          const hallReservation = hallReservations.find(
                            (h) => h.hall.id === e.hall?.id
                          );
                          return (
                            <TabPanel key={index} header={e.title}>
                              {hallReservation
                                ? getSeats(e, hallReservation)
                                : "Brak danych o sali"}
                            </TabPanel>
                          );
                        })}
                      </TabView>
                    </div>
                  </div>
                </div>
                <div className="self-stretch min-w-[1px] bg-black"></div>
                <div className="pl-3 flex flex-col justify-centers items-start gap-1">
                  <article className="flex flex-col justify-start items-start gap-1 w-full">
                    <h5 className="text-center font-semibold text-base w-full">Typy miejsc:</h5>
                    <div className="flex justify-start flex-col gap-2">
                      {seatTypes.map((type, index) => (
                        <div key={index} className="flex flex-row justify-start items-center gap-2">
                          <div
                            className="py-[2px] px-[6px] border-[1px]"
                            style={{ borderColor: type.seatColor, color: type.seatColor }}
                          >
                            <FontAwesomeIcon
                              fontSize={12}
                              icon={faCouch}
                              //color="white"
                            ></FontAwesomeIcon>
                          </div>
                          <p className="text-[14px]">
                            {type.name} {`(+ ${type.addtionalPaymentPercentage} % ceny biletu)`}
                          </p>
                        </div>
                      ))}
                      <div className="flex flex-row justify-start items-center gap-2">
                        <div className="py-[2px] px-[6px] border-gray-400 bg-gray-200">
                          <FontAwesomeIcon
                            fontSize={12}
                            icon={faCouch}
                            color="#6b7280"
                          ></FontAwesomeIcon>
                        </div>
                        <p className="text-[14px]">Miejsce nieaktywne</p>
                      </div>
                      <div className="flex flex-row justify-start items-center gap-2">
                        <div className="py-[2px] px-[6px] border-red-600 bg-red-600">
                          <FontAwesomeIcon
                            fontSize={12}
                            icon={faCouch}
                            color="#000"
                          ></FontAwesomeIcon>
                        </div>
                        <p className="text-[14px]">Miejsce zarezerwowane</p>
                      </div>
                    </div>
                  </article>
                  <article className="self-start pt-2 flex flex-col justify-center items-center gap-2 mt-2 w-full">
                    <h5 className="font-semibold text-base w-full text-center">
                      Wybierz typ płatności i typ biletu:
                    </h5>
                    <Select
                      label="Typ płatności"
                      name="paymentTypeId"
                      selectedOption={paymentTypeSelectOptions[0]}
                      optionValues={paymentTypeSelectOptions}
                      error={errors.paymentTypeId}
                      errorHeight={15}
                    />
                    <Select
                      label="Typ biletu"
                      name="ticketId"
                      selectedOption={ticketTypeSelectOptions[0]}
                      optionValues={ticketTypeSelectOptions}
                      error={errors.ticketId}
                      errorHeight={15}
                    />
                  </article>
                  <article className="self-start flex flex-col justify-center items-center gap-2 mt-2 w-full">
                    <h5 className="font-semibold text-base w-full text-center">Podsumowanie:</h5>
                    {festival.events.map((e) => (
                      <LabelText
                        key={e.id}
                        emptyMessage=""
                        label={`\"${e.title}\" nr miejsca:`}
                        text={userReservedSeats.find((es) => es.event.id == e.id)?.seatNr}
                      />
                    ))}

                    <LabelText
                      label="Cena biletu:"
                      text={`${
                        selectedTicketType?.price != undefined ? selectedTicketType?.price : 0
                      } zł`}
                    />
                    <LabelText
                      label="Całkowity koszt:"
                      text={`${
                        selectedTicketType?.price != undefined ? selectedTicketType?.price : 0
                      } zł`}
                    />
                  </article>
                  <article className="self-start flex flex-col justify-center items-center mt-4 w-full">
                    <MessageText
                      maxWidth={330}
                      messageType={MessageType.Info}
                      fontSize={14}
                      iconSize={14}
                      text={`Po dokonaniu transakcji otrzymasz wiadomość email z potwierdzeniem rezerwacji
                          w formie pliku PDF.`}
                    />
                  </article>
                  <div className="flex flex-row self-center justify-center items-center gap-2 pt-4">
                    <Button
                      text="Anuluj"
                      width={140}
                      height={45}
                      icon={faXmark}
                      iconSize={18}
                      style={ButtonStyle.DefaultGray}
                      action={onDialogClose}
                    />
                    <FormButton
                      isSubmitting={isSubmitting}
                      text="Kup bilet"
                      icon={faShoppingCart}
                      width={140}
                      height={45}
                      rounded={999}
                    />
                  </div>
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
