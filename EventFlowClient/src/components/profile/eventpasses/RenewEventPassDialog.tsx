import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { EventPass, EventPassType } from "../../../models/response_models";
import DateFormatter from "../../../helpers/DateFormatter";
import { DateFormat } from "../../../helpers/enums/DateFormatEnum";
import Button, { ButtonStyle } from "../../buttons/Button";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import { HTTPStatusCode } from "../../../helpers/enums/HTTPStatusCode";
import { faCheck, faXmark } from "@fortawesome/free-solid-svg-icons";
import LabelText from "../../common/LabelText";
import { toast } from "react-toastify";
import Select from "../../common/forms/Select";
import { SelectOption } from "../../../helpers/SelectOptions";
import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import {
  EventPassUpdateRequest,
  EventPassUpdateSchema,
} from "../../../models/update_schemas/EventPassUpdateSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import FormButton from "../../common/forms/FormButton";
import MessageText from "../../common/MessageText";
import { MessageType } from "../../../helpers/enums/MessageTypeEnum";

interface RenewEventPassDialogProps {
  eventPass?: EventPass;
  onDialogConfirm: () => void;
  onDialogClose: () => void;
}

const RenewEventPassDialog = forwardRef<HTMLDialogElement, RenewEventPassDialogProps>(
  ({ eventPass, onDialogClose, onDialogConfirm }: RenewEventPassDialogProps, ref) => {
    // const { put: renewEventPass, statusCode: statusCode } = useApi<EventPass>(
    //   ApiEndpoint.EventPass
    // );
    const { put: renewEventPass } = useApi<EventPass, undefined, EventPassUpdateRequest>(
      ApiEndpoint.EventPass
    );

    const { data: eventPassTypes, get: getEventPassTypes } = useApi<EventPassType>(
      ApiEndpoint.EventPassType
    );

    const [passTypesSelectOptions, setPassTypesSelectOptions] = useState<SelectOption[]>([]);

    const [selectedPassType, setSelectedPassType] = useState<EventPassType>();

    const methods = useForm<EventPassUpdateRequest>({
      resolver: zodResolver(EventPassUpdateSchema),
    });
    const { handleSubmit, formState, watch, setValue } = methods;
    const { errors, isSubmitting } = formState;

    const passTypeId = watch().passTypeId;

    useEffect(() => {
      const type = eventPassTypes.find((pt) => pt.id === passTypeId);
      if (type) {
        setSelectedPassType(type);
      }
    }, [passTypeId]);

    useEffect(() => {
      getEventPassTypes({ id: undefined, queryParams: undefined });
    }, []);

    useEffect(() => {
      const selectOptions: SelectOption[] = eventPassTypes.map(
        (passType) =>
          ({
            value: passType.id,
            option: passType.name,
          } as SelectOption)
      );
      setPassTypesSelectOptions(selectOptions);
      if (eventPassTypes.length > 0) setSelectedPassType(eventPassTypes[0]);
    }, [eventPassTypes]);

    const addMonthsToEndDate = (dateString: string, months: number | undefined): string => {
      if (!dateString || months === undefined) return "";
      const date = new Date(dateString);
      date.setMonth(date.getMonth() + months);
      return date.toISOString();
    };

    const onSubmit: SubmitHandler<EventPassUpdateRequest> = async (data) => {
      console.log(data);
      if (eventPass !== undefined) {
        //setPromisePending(true);
        const request = renewEventPass({ id: eventPass.id, body: data });
        await toast.promise(request, {
          pending: "Trwa przetwarzanie przedłużenia karnetu",
          error: "Wystąpił błąd podczas przedłużania karnetu",
        });
        if ((await request) == HTTPStatusCode.NoContent) {
          toast.success("Karnet został przedłużony pomyślnie");
          onDialogConfirm();
        }
        //setPromisePending(false);
        //setActionPerformed(true);
      }
    };

    // useEffect(() => {
    //   if (actionPerformed) {
    //     if (createPaymentStatusCode == HTTPStatusCode.Ok) {
    //       onDialogConfirm();
    //     }
    //     setActionPerformed(false);
    //   }
    // }, [actionPerformed]);

    const calculatePrice = () => {
      if (selectedPassType?.price && eventPass?.passType?.renewalDiscountPercentage) {
        return (
          selectedPassType.price -
          (selectedPassType.price * eventPass?.passType?.renewalDiscountPercentage) / 100
        ).toFixed(2);
      }
      return "";
    };

    return (
      <div>
        {eventPass && (
          <Dialog ref={ref}>
            <article className="flex flex-col justify-center items-center gap-5 max-w-[750px]">
              <h2>Przedłużanie karnetu</h2>
              <div className="flex flex-col justify-center items-center gap-3">
                <div className="flex flex-col justify-center items-center gap-2">
                  <p className="text-textPrimary text-base text-center">
                    Aktualne informacje o karnecie:
                  </p>
                  <div className="flex flex-row justify-center items-center gap-14">
                    <div className="flex flex-col justify-start items-start gap-2">
                      <LabelText label="ID:" labelWidth={150} text={eventPass.id} gap={10} />
                      <LabelText
                        labelWidth={150}
                        label="Data zakupu:"
                        text={DateFormatter.FormatDate(eventPass.startDate, DateFormat.Date)}
                        gap={10}
                      />
                      <LabelText
                        labelWidth={150}
                        label="Data zakończenia:"
                        text={DateFormatter.FormatDate(eventPass.endDate, DateFormat.Date)}
                        gap={10}
                      />
                    </div>
                    <div className="flex flex-col justify-start items-start gap-2">
                      <LabelText
                        label="Całkowita cena:"
                        labelWidth={190}
                        text={`${eventPass.paymentAmount} zł`}
                        gap={10}
                      />
                      <LabelText
                        label="Długość karnetu (msc):"
                        labelWidth={190}
                        text={eventPass.passType?.validityPeriodInMonths}
                        gap={10}
                      />
                      <LabelText
                        label="Typ karnetu:"
                        labelWidth={190}
                        text={eventPass.passType?.name}
                        gap={10}
                      />
                    </div>
                  </div>
                </div>
                <div className="bg-[#4c4c4c] h-[1px] w-full"></div>
                <div>
                  <FormProvider {...methods}>
                    <form
                      className="flex flex-col justify-center items-center gap-3 w-full"
                      onSubmit={handleSubmit(onSubmit)}
                    >
                      <div className="flex flex-col justify-center items-center gap-2 w-full">
                        <p className="text-black font-semibold text-base text-center">
                          Wybierz typ karnetu i typ płatności:
                        </p>
                        <Select
                          label="Typ karnetu"
                          name="passTypeId"
                          optionValues={passTypesSelectOptions}
                          error={errors.passTypeId}
                        />
                      </div>
                      <div className="bg-[#4c4c4c] h-[1px] w-full"></div>
                      <div className="flex flex-col justify-center items-center gap-2">
                        <p className="text-textPrimary text-base text-center">
                          Nowy karnet po przedłużeniu:
                        </p>
                        <div className="flex flex-row justify-center items-center gap-14">
                          <div className="flex flex-col justify-start items-start gap-2 min-w-[300px]">
                            <LabelText label="ID:" labelWidth={150} text={eventPass.id} gap={10} />
                            <LabelText
                              labelWidth={150}
                              label="Data zakupu:"
                              text={DateFormatter.FormatDate(eventPass.startDate, DateFormat.Date)}
                              gap={10}
                            />
                            <LabelText
                              labelWidth={150}
                              label="Data przedłużenia:"
                              text={DateFormatter.FormatDate(Date.now(), DateFormat.Date)}
                              gap={10}
                            />
                            <LabelText
                              labelWidth={150}
                              label="Data zakończenia:"
                              text={DateFormatter.FormatDate(
                                addMonthsToEndDate(
                                  eventPass.endDate,
                                  selectedPassType?.validityPeriodInMonths
                                ),
                                DateFormat.Date
                              )}
                              gap={10}
                            />
                            <LabelText
                              labelWidth={150}
                              label="Typ płatności:"
                              text="Płatność online"
                              gap={10}
                            />
                          </div>
                          <div className="flex flex-col justify-start items-start gap-2 min-w-[350px]">
                            <LabelText
                              label="Typ karnetu:"
                              labelWidth={190}
                              text={selectedPassType?.name}
                              gap={10}
                            />
                            <LabelText
                              labelWidth={190}
                              label="Długość karnetu (msc):"
                              text={selectedPassType?.validityPeriodInMonths}
                              gap={10}
                            />
                            <LabelText
                              labelWidth={190}
                              label="Cena karnetu:"
                              text={`${selectedPassType?.price} zł`}
                              gap={10}
                            />
                            <LabelText
                              labelWidth={190}
                              label="Zniżka:"
                              text={`${eventPass.passType?.renewalDiscountPercentage} %`}
                              gap={10}
                            />
                            <LabelText
                              labelWidth={190}
                              label="Całkowita cena:"
                              text={`${calculatePrice()} zł`}
                              gap={10}
                            />
                          </div>
                        </div>
                      </div>
                      <div className="bg-[#4c4c4c] h-[1px] w-full"></div>

                      <div className="flex flex-col justify-center items-center gap-2">
                        <MessageText
                          messageType={MessageType.Info}
                          text={`Po przedłużeniu karnetu otrzymasz wiadomość email z karnetem w
                          formie pliku PDF wraz z potwierdzeniem płatności.`}
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
                          text="Przedłuż karnet"
                          icon={faCheck}
                          width={180}
                          height={45}
                          rounded={999}
                        />
                      </div>
                    </form>
                  </FormProvider>
                </div>
              </div>
            </article>
          </Dialog>
        )}
      </div>
    );
  }
);
export default RenewEventPassDialog;
