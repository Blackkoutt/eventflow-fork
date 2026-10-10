import { forwardRef, useEffect, useState } from "react";
import { faCheck, faXmark } from "@fortawesome/free-solid-svg-icons";
import { toast } from "react-toastify";
import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { AdditionalServices, EventEntity, Hall, HallRent } from "../../models/response_models";
import { HallRentRequest, HallRentSchema } from "../../models/create_schemas/HallRentSchema";
import MultiSelect from "../common/forms/MultiSelect";
import DatePicker from "../common/forms/DatePicker";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { SelectOption } from "../../helpers/SelectOptions";
import { HTTPStatusCode } from "../../helpers/enums/HTTPStatusCode";
import LabelText from "../common/LabelText";
import DateFormatter from "../../helpers/DateFormatter";
import { DateFormat } from "../../helpers/enums/DateFormatEnum";
import MessageText from "../common/MessageText";
import { MessageType } from "../../helpers/enums/MessageTypeEnum";
import Button, { ButtonStyle } from "../buttons/Button";
import FormButton from "../common/forms/FormButton";
import { useAuth } from "../../context/AuthContext";
import { ScheduleXCalendar, useCalendarApp } from "@schedule-x/react";
import { createViewMonthGrid, createViewWeek } from "@schedule-x/calendar";
import { createEventsServicePlugin } from "@schedule-x/events-service";
import { differenceInHours } from "date-fns";
import Dialog from "../common/Dialog";

interface RentHallDialogProps {
  hall?: Hall;
  onDialogConfirm: () => void;
  onDialogClose: () => void;
}

type HallRentDto = {
  hallId: number;
  additionalServicesIds: number[];
  startDate: string;
  endDate: string;
};

const RentHallDialog = forwardRef<HTMLDialogElement, RentHallDialogProps>(
  ({ hall, onDialogClose, onDialogConfirm }: RentHallDialogProps, ref) => {
    const { post: rentHall } = useApi<HallRent, HallRentDto>(ApiEndpoint.HallRent);
    const { data: services, get: getServices } = useApi<AdditionalServices>(
      ApiEndpoint.AdditionalServices
    );
    const { data: allRents, get: getAllRents } = useApi<HallRent>(ApiEndpoint.HallRent);
    const { data: allEvents, get: getAllEvents } = useApi<EventEntity>(ApiEndpoint.Event);

    const calendar = useCalendarApp({
      views: [createViewMonthGrid(), createViewWeek()],
      selectedDate: DateFormatter.FormatDateForCalendar(new Date()),
      plugins: [createEventsServicePlugin()],
      locale: "pl-PL",
    });

    const { authenticated } = useAuth();

    const [servicesSelectOptions, setServicesSelectOptions] = useState<SelectOption[]>([]);
    const [servicesTotalPrice, setServicesTotalPrice] = useState<number>(0);
    const [totalPrice, setTotalPrice] = useState<number>(0);
    const [totalDurationInHours, setTotalDurationInHours] = useState<number>(0);
    //const [selectedServices, setSelectedServices] = useState<AdditionalServices[]>();

    const methods = useForm<HallRentRequest>({
      resolver: zodResolver(HallRentSchema),
    });
    const { handleSubmit, formState, watch, setValue } = methods;
    const { errors, isSubmitting } = formState;

    useEffect(() => {
      getServices({ id: undefined, queryParams: undefined });
      getAllRents({ id: undefined, queryParams: { getAll: true, hallNr: hall?.hallNr } });
      getAllEvents({ id: undefined, queryParams: { getAll: true, hallNr: hall?.hallNr } });
    }, []);

    useEffect(() => {
      if (allRents && allRents.length > 0) {
        const formattedRents = allRents.map((r) => ({
          ...r,
          title: "Rezerwacja",
          location: `Sala nr ${hall?.hallNr}`,
          start: DateFormatter.FormatDateForCalendar(r.startDate),
          end: DateFormatter.FormatDateForCalendar(r.endDate),
        }));

        const formattedEvents = allEvents.map((e) => ({
          ...e,
          title: e.title || "Wydarzenie",
          location: `Sala nr ${hall?.hallNr}`,
          start: DateFormatter.FormatDateForCalendar(e.start),
          end: DateFormatter.FormatDateForCalendar(e.end),
        }));

        const allFormattedEvents = [...formattedRents, ...formattedEvents];
        calendar.eventsService.set(allFormattedEvents);
        console.log(calendar.events.getAll());
      }
      console.log(allEvents);
    }, [allRents, allEvents]);

    useEffect(() => {
      const selectOptions: SelectOption[] = services.map(
        (s: AdditionalServices) =>
          ({
            value: s.id,
            option: `${s.name} (${s.price} zł)`,
          } as SelectOption)
      );
      setServicesSelectOptions(selectOptions);
    }, [services]);

    const selectedServicesIds = watch().additionalServicesIds;

    useEffect(() => {
      let servicesPrice: number = 0;
      services.forEach((s: AdditionalServices) => {
        if (selectedServicesIds.includes(s.id)) {
          servicesPrice += s.price;
        }
      });
      setServicesTotalPrice(Math.round(servicesPrice * 100) / 100);
    }, [selectedServicesIds]);

    const startDate = watch().startDate;
    const endDate = watch().endDate;

    useEffect(() => {
      if (startDate && endDate) {
        const startRentDate = DateFormatter.ParseDate(startDate.toString());
        const endRentDate = DateFormatter.ParseDate(endDate.toString());
        if (startRentDate && endRentDate) {
          const durationInHours = differenceInHours(endRentDate, startRentDate, {
            roundingMethod: "ceil",
          });
          setTotalDurationInHours(durationInHours);
        }
      }
    }, [startDate, endDate]);

    useEffect(() => {
      if (hall) {
        const totalPrice = hall?.rentalPricePerHour * totalDurationInHours + servicesTotalPrice;
        setTotalPrice(Math.round(totalPrice * 100) / 100);
      }
    }, [servicesTotalPrice, totalDurationInHours]);

    const onSubmit: SubmitHandler<HallRentRequest> = async (data) => {
      console.log(data);
      if (hall != null && hall != undefined) {
        if (authenticated) {
          const requestData: HallRentDto = {
            hallId: hall.id,
            additionalServicesIds: data.additionalServicesIds,
            startDate: DateFormatter.ToLocalISOString(data.startDate),
            endDate: DateFormatter.ToLocalISOString(data.endDate),
          };
          const request = rentHall({ body: requestData });
          await toast.promise(request, {
            pending: "Trwa przetwarzanie rezerwacji sali",
            error: "Wystąpił błąd podczas rezerwacji sali",
          });
          if ((await request) == HTTPStatusCode.Created) {
            toast.success("Sala została zarezerwowana pomyślnie");
            onDialogConfirm();
          }
        } else {
          toast.error("Zaloguj się aby wynająć tę salę");
        }
      }
    };

    return (
      <div className="h-full">
        {hall && (
          <Dialog ref={ref} minWidth={750} maxWidth={1200} top={15}>
            <article className="flex flex-col justify-center items-center gap-5 w-full">
              <h2>Wynajem sali nr {hall.hallNr}</h2>
              <FormProvider {...methods}>
                <form
                  className="flex flex-col justify-center items-center gap-3 w-full"
                  onSubmit={handleSubmit(onSubmit)}
                >
                  <div className="grid grid-cols-12 gap-10">
                    <div className="col-span-9">
                      <ScheduleXCalendar calendarApp={calendar} />
                    </div>
                    <div className=" col-span-3 flex flex-col justify-start items-start gap-3 w-full">
                      <div className="flex flex-col justify-center items-center gap-4 pt-1 w-full">
                        <p className="text-black font-semibold text-base text-center">
                          Wybierz daty rezerwacji oraz dodatkowe usługi:
                        </p>
                        <div className="flex flex-col justify-center items-center gap-2 w-full">
                          <DatePicker
                            label="Początkowa data"
                            name="startDate"
                            isTime={true}
                            error={errors.startDate}
                            errorHeight={15}
                          />
                          <DatePicker
                            label="Końcowa data"
                            name="endDate"
                            isTime={true}
                            error={errors.endDate}
                            errorHeight={15}
                          />
                          <MultiSelect
                            maxSelectedItemsHeight={40}
                            label="Dodatkowe usługi"
                            name="additionalServicesIds"
                            optionValues={servicesSelectOptions}
                            error={errors.additionalServicesIds}
                            errorHeight={15}
                          />
                        </div>
                      </div>
                      <div className="bg-[#4c4c4c] h-[1px] w-full"></div>
                      <div className="flex flex-col justify-center items-center gap-3 w-full">
                        <p className="text-black font-semibold text-base text-center">
                          Podsumowanie rezerwacji:
                        </p>
                        <div className="flex flex-col justify-center items-center gap-2 -translate-x-[30px]">
                          <LabelText labelWidth={163} label="Typ płatności:" text="Płatność online" gap={10} />
                          <LabelText
                            label="Początek rezerwacji:"
                            labelWidth={163}
                            text={startDate}
                            gap={10}
                          />
                          <LabelText
                            label="Koniec rezerwacji:"
                            labelWidth={163}
                            text={endDate}
                            gap={10}
                          />
                          <LabelText
                            label="Ilość godzin:"
                            labelWidth={163}
                            text={totalDurationInHours}
                            gap={10}
                          />
                          <LabelText
                            label="Cena za 1h:"
                            labelWidth={163}
                            text={`${hall.rentalPricePerHour} zł`}
                            gap={10}
                          />
                          <LabelText
                            label="Cena usług:"
                            labelWidth={163}
                            text={`${servicesTotalPrice} zł`}
                            gap={10}
                          />
                          <LabelText
                            labelWidth={163}
                            label="Całkowita cena:"
                            text={`${totalPrice} zł`}
                            gap={10}
                          />
                        </div>
                      </div>
                    </div>
                  </div>

                  <div className="flex flex-col justify-center items-center gap-3 w-full">
                    <div className="flex flex-col justify-center items-center gap-2 py-3">
                      <MessageText
                        messageType={MessageType.Info}
                        text={`Po dokonaniu transakcji otrzymasz wiadomość email z potwierdzeniem rezerwacji
                          w formie pliku PDF.`}
                      />
                    </div>
                    <div className="flex flex-row justify-center items-center gap-2">
                      <Button
                        text="Anuluj"
                        width={180}
                        height={45}
                        icon={faXmark}
                        iconSize={18}
                        style={ButtonStyle.DefaultGray}
                        action={onDialogClose}
                      />
                      <FormButton
                        isSubmitting={isSubmitting}
                        text="Wynajmij salę"
                        icon={faCheck}
                        width={180}
                        height={45}
                        rounded={999}
                      />
                    </div>
                  </div>
                </form>
              </FormProvider>
            </article>
          </Dialog>
        )}
      </div>
    );
  }
);
export default RentHallDialog;
