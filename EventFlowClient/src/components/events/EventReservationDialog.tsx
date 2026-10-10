import { forwardRef, useEffect, useState } from "react";
import { faCouch, faShoppingCart, faXmark } from "@fortawesome/free-solid-svg-icons";
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
    const [paymentTypeSelectOptions, setPaymentTypeSelectOptions] = useState<SelectOption[]>([]);
    const [hallSeats, setHallSeats] = useState<Seat[]>([]);
    const [reservedSeatNumbers, setReservedSeatNumbers] = useState<number[]>([]);
    const [userReservedSeats, setUserReservedSeats] = useState<number[]>([]);
    const { authenticated } = useAuth();

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

    // Print Seats
    const getSeats = (maxNumberOfSeatsRows?: number, maxNumberOfSeatsColumns?: number) => {
      if (maxNumberOfSeatsRows !== undefined && maxNumberOfSeatsColumns !== undefined) {
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
                        const seat = hallSeats.find((s) => s.seatNr == seatNumber);
                        const reservedSeat = reservedSeatNumbers.find((nr) => nr == seatNumber);
                        const userReservedSeat = userReservedSeats.find((nr) => nr == seat?.seatNr);
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
                                if (userReservedSeats?.find((s) => s == seat.seatNr)) {
                                  setUserReservedSeats((prev) =>
                                    prev?.filter((s) => s != seat.seatNr)
                                  );
                                } else {
                                  setUserReservedSeats((prev) => [...prev, seat.seatNr]);
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
    };

    const methods = useForm<ReservationRequest>({
      resolver: zodResolver(ReservationSchema),
    });
    const { handleSubmit, formState, watch } = methods;
    const { errors, isSubmitting } = formState;

    const selectedTicketTypeId = watch().ticketId;
    const [selectedTicketType, setSelectedTicketType] = useState<Ticket>();

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
                      {getSeats(
                        hallWithDetails[0]?.hallDetails?.maxNumberOfSeatsRows,
                        hallWithDetails[0]?.hallDetails?.maxNumberOfSeatsColumns
                      )}
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
                    <LabelText label="Wybrane nr miejsc:" text={userReservedSeats.join(", ")} />
                    <LabelText label="Ilość miejsc:" text={userReservedSeats.length} />
                    <LabelText
                      label="Cena biletu:"
                      text={`${
                        selectedTicketType?.price != undefined ? selectedTicketType?.price : 0
                      } zł`}
                    />
                    <LabelText
                      label={`Dodatkowe opłaty (+${totalAdditionalPaymentPercentage}%):`}
                      text={`${additionalPaymentCost} zł`}
                    />
                    <LabelText label="Całkowity koszt:" text={`${totalCost} zł`} />
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
export default EventReservationDialog;
