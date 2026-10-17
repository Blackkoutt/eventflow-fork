import { FormProvider, useForm } from "react-hook-form";
import {
  EventFestivalTicketRequest,
  EventFestivalTicketSchema,
} from "../../../models/create_schemas/EventFestivalTicketSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import Input from "../../common/forms/Input";
import { TicketType } from "../../../models/response_models";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import useApi from "../../../hooks/useApi";
import { useContext, useEffect, useState } from "react";
import { SelectOption } from "../../../helpers/SelectOptions";
import Select from "../../common/forms/Select";
import { faPlus, faXmark } from "@fortawesome/free-solid-svg-icons";
import { EventTicketContext } from "../../../context/EventTicketContext";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import LabelText from "../../common/LabelText";
import { toast } from "react-toastify";

const AddEventTicket = () => {
  const { data: ticketTypes, get: getTicketType } = useApi<TicketType>(ApiEndpoint.TicketType);
  const [ticketTypesSelectOptions, setTicketTypesSelectOptions] = useState<SelectOption[]>([]);

  const context = useContext(EventTicketContext);

  if (context === undefined) {
    throw new Error("EventTicketContext must be used within an EventTicketProvider");
  }

  const { eventTickets, setEventTickets } = context;

  useEffect(() => {
    getTicketType({ id: undefined, queryParams: undefined });
  }, []);

  useEffect(() => {
    const selectOptions: SelectOption[] = ticketTypes.map(
      (type) =>
        ({
          value: type.id,
          option: type.name,
        } as SelectOption)
    );
    setTicketTypesSelectOptions(selectOptions);
  }, [ticketTypes]);

  const methods = useForm<EventFestivalTicketRequest>({
    resolver: zodResolver(EventFestivalTicketSchema),
  });
  const { formState, watch, setError, clearErrors, reset } = methods;
  const { errors } = formState;

  useEffect(() => {
    reset();
  }, []);

  const onSubmit = () => {
    const formData = watch(); // Pobierz wartości pól
    console.log("Form Data:", formData);
    const validationResult = EventFestivalTicketSchema.safeParse(formData);

    if (!validationResult.success) {
      validationResult.error.format().price?._errors.forEach((err) => {
        setError("price", {
          type: "manual",
          message: err,
        });
      });
      validationResult.error.format().ticketTypeId?._errors.forEach((err) => {
        setError("ticketTypeId", {
          type: "manual",
          message: err,
        });
      });
      return;
    }
    clearErrors("price");
    clearErrors("ticketTypeId");
    const eventTicket = eventTickets.find((et) => et.ticketTypeId == formData.ticketTypeId);
    if (eventTicket !== undefined)
      toast.error(
        `Wydarzenie posiada już bilet typu ${
          ticketTypes.find((tt) => tt.id == eventTicket.ticketTypeId)?.name
        }`
      );
    else {
      setEventTickets((prev) => [...prev, formData]);
    }
  };

  const deleteTicketType = (id: number) => {
    setEventTickets((prev) => prev.filter((elem) => elem.ticketTypeId !== id));
  };

  useEffect(() => {
    console.log(eventTickets);
  }, [eventTickets]);

  return (
    <div className="flex flex-col justify-start items-start w-full gap-2">
      <FormProvider {...methods}>
        <form className="flex flex-col justify-center items-center gap-3 w-full">
          <div className="flex flex-col justify-center items-center gap-2 w-full">
            <Input
              label="Cena biletu"
              type="number"
              step={0.01}
              name="price"
              errorHeight={15}
              error={errors.price}
            />
            <Select
              label="Typ biletu"
              name="ticketTypeId"
              optionValues={ticketTypesSelectOptions}
              error={errors.ticketTypeId}
              errorHeight={15}
            />
          </div>
          <div className="flex flex-row justify-center items-start self-start gap-2 -translate-y-2">
            <button
              type="button"
              onClick={onSubmit}
              className="p-[6px] border-textPrimary border-[1px] rounded-md"
            >
              <div className="flex flex-row justify-center items-center gap-2">
                <FontAwesomeIcon icon={faPlus} color="#22c55e" fontSize={13}></FontAwesomeIcon>
                <p className="text-[13px]">Dodaj typ biletu</p>
              </div>
            </button>
          </div>
        </form>
      </FormProvider>
      {eventTickets.map((et, index) => (
        <div key={index} className="flex flex-row w-full justify-between">
          <LabelText
            labelWidth={125}
            isTextLeft={true}
            textWidth={90}
            label={`${ticketTypes.find((tt) => tt.id == et.ticketTypeId)?.name}:`}
            text={`${et.price} PLN`}
          />
          <FontAwesomeIcon
            icon={faXmark}
            className="hover:cursor-pointer"
            onClick={() => deleteTicketType(et.ticketTypeId)}
            color="#4C4C4C"
            title="Usuń typ biletu"
          ></FontAwesomeIcon>
        </div>
      ))}
    </div>
  );
};
export default AddEventTicket;
