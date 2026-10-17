import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { EventCategory, EventEntity, Hall } from "../../../models/response_models";
import Button, { ButtonStyle } from "../../buttons/Button";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import { HTTPStatusCode } from "../../../helpers/enums/HTTPStatusCode";
import { faCheck, faXmark } from "@fortawesome/free-solid-svg-icons";
import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import FormButton from "../../common/forms/FormButton";
import Input from "../../common/forms/Input";
import Select from "../../common/forms/Select";
import { SelectOption } from "../../../helpers/SelectOptions";
import { EventRequest, EventSchema } from "../../../models/create_schemas/EventSchema";
import TextArea from "../../common/forms/TextArea";
import DatePicker from "../../common/forms/DatePicker";
import { EventFestivalTicketRequest } from "../../../models/create_schemas/EventFestivalTicketSchema";
import AddEventTicket from "../tickets/AddEventTicket";
import { EventTicketContext } from "../../../context/EventTicketContext";
import { toast } from "react-toastify";
import DateFormatter from "../../../helpers/DateFormatter";

interface CreateEventDialogProps {
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogSuccess: () => void;
  onDialogClose: () => void;
}

const CreateEventDialog = forwardRef<HTMLDialogElement, CreateEventDialogProps>(
  (
    { maxWidth, minWidth, onDialogClose, paddingX, onDialogSuccess }: CreateEventDialogProps,
    ref
  ) => {
    const { statusCode: statusCode, post: postItem } = useApi<EventEntity, FormData>(
      ApiEndpoint.Event
    );
    const { data: categories, get: getCategories } = useApi<EventCategory>(
      ApiEndpoint.EventCategory
    );
    const { data: halls, get: getHalls } = useApi<Hall>(ApiEndpoint.Hall);

    const [categorySelectOptions, setCategorySelectOptions] = useState<SelectOption[]>([]);
    const [hallSelectOptions, setHallSelectOptions] = useState<SelectOption[]>([]);
    const [eventTickets, setEventTickets] = useState<EventFestivalTicketRequest[]>([]);

    useEffect(() => {
      getCategories({ id: undefined, queryParams: undefined });
      getHalls({ id: undefined, queryParams: undefined });
    }, []);

    useEffect(() => {
      const selectOptions: SelectOption[] = categories.map(
        (cat) =>
          ({
            value: cat.id,
            option: cat.name,
          } as SelectOption)
      );
      setCategorySelectOptions(selectOptions);
    }, [categories]);

    useEffect(() => {
      const selectOptions: SelectOption[] = halls.map(
        (hall) =>
          ({
            value: hall.id,
            option: `Sala nr ${hall.hallNr}`,
          } as SelectOption)
      );
      setHallSelectOptions(selectOptions);
    }, [halls]);

    const [actionPerformed, setActionPerformed] = useState(false);

    const methods = useForm<EventRequest>({
      resolver: zodResolver(EventSchema),
    });
    const { handleSubmit, formState, reset } = methods;
    const { errors, isSubmitting } = formState;

    const onSubmit: SubmitHandler<EventRequest> = async (data) => {
      if (eventTickets && eventTickets.length > 0) {
        console.log(data);
        data.eventTickets = eventTickets;
        const formData = new FormData();
        formData.append("Name", data.name);
        formData.append("StartDate", DateFormatter.ToLocalISOString(data.startDate));
        formData.append("EndDate", DateFormatter.ToLocalISOString(data.endDate));
        formData.append("ShortDescription", data.shortDescription);
        formData.append("LongDescription", data.longDescription || "");
        formData.append("CategoryId", data.categoryId.toString());
        formData.append("HallId", data.hallId.toString());
        formData.append("EventPhoto", data.eventPhoto);
        data.eventTickets.forEach((ticket, index) => {
          formData.append(`EventTickets[${index}].price`, ticket.price.toString());
          formData.append(`EventTickets[${index}].ticketTypeId`, ticket.ticketTypeId.toString());
        });
        console.log(formData);

        await toast.promise(postItem({ body: formData }), {
          pending: "Wykonywanie żądania",
          success: "Wydarzenie zostało pomyślnie utworzone",
          error: "Wystąpił błąd podczas tworzenia wydarzenia",
        });
        setActionPerformed(true);
      } else {
        toast.error("Nie ustalono biletów na dane wydarzenie");
      }
    };

    useEffect(() => {
      if (actionPerformed) {
        if (statusCode == HTTPStatusCode.Created) {
          onDialogSuccess();
          setEventTickets([]);
          reset();
        }
        setActionPerformed(false);
      }
    }, [actionPerformed]);

    return (
      <Dialog
        ref={ref}
        maxWidth={maxWidth}
        paddingLeft={paddingX}
        paddingRight={paddingX}
        minWidth={minWidth}
        onClose={onDialogClose}
      >
        <div className="flex flex-col justify-center items-center pt-2 pb-1">
          <h3 className="text-center font-semibold text-[24px]">Tworzenie wydarzenia</h3>
        </div>
        <div className="flex flex-row justify-center items-start gap-6 w-full">
          <FormProvider {...methods}>
            <form
              id="createEventForm"
              className="flex flex-col justify-center items-center gap-3 w-full mt-4"
              onSubmit={handleSubmit(onSubmit)}
            >
              <div className="flex flex-row justify-center items-start gap-6 w-full">
                <div className="flex flex-col justify-center items-center gap-2 w-full">
                  <Input
                    label="Nazwa"
                    type="text"
                    name="name"
                    maxLength={60}
                    error={errors.name}
                    isFirstLetterUpperCase={true}
                    errorHeight={15}
                  />
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
                  <Input
                    label="Zdjęcie wydarzenia"
                    type="file"
                    name="eventPhoto"
                    error={errors.eventPhoto}
                    errorHeight={15}
                  />
                </div>
                <div className="flex flex-col justify-center items-center gap-2 w-full">
                  <TextArea
                    label="Krótki opis"
                    name="shortDescription"
                    maxLength={300}
                    errorHeight={15}
                    isFirstLetterUpperCase={true}
                    error={errors.shortDescription}
                  />
                  <TextArea
                    label="Długi opis"
                    name="longDescription"
                    maxLength={600}
                    errorHeight={15}
                    isFirstLetterUpperCase={true}
                    error={errors.longDescription}
                  />
                  <Select
                    label="Kategoria"
                    name="categoryId"
                    optionValues={categorySelectOptions}
                    error={errors.categoryId}
                    errorHeight={15}
                  />
                  <Select
                    label="Sala"
                    name="hallId"
                    optionValues={hallSelectOptions}
                    error={errors.hallId}
                    errorHeight={15}
                  />
                </div>
              </div>
            </form>
          </FormProvider>
          <div className="w-[1px] mt-4 bg-textPrimary h-[300px]"></div>
          <div className="w-[50%] mt-4">
            <EventTicketContext.Provider value={{ eventTickets, setEventTickets }}>
              <AddEventTicket />
            </EventTicketContext.Provider>
          </div>
        </div>
        <div className="flex flex-row justify-center items-center gap-2 mt-2">
          <Button
            text="Anuluj"
            width={145}
            height={45}
            icon={faXmark}
            iconSize={18}
            style={ButtonStyle.DefaultGray}
            action={() => {
              onDialogClose();
              reset();
            }}
          ></Button>
          <FormButton
            text="Zatwierdź"
            width={145}
            height={45}
            form="createEventForm"
            icon={faCheck}
            iconSize={18}
            isSubmitting={isSubmitting}
            rounded={999}
          />
        </div>
      </Dialog>
    );
  }
);
export default CreateEventDialog;
