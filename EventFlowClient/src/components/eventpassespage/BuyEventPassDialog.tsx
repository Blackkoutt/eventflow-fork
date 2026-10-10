import { forwardRef, useEffect } from "react";
import { EventPassType } from "../../models/response_models";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import { EventPassRequest } from "../../models/create_schemas/EventPassSchema";
import { PaymentRedirectResponse } from "../../models/response_models/PaymentRedirectResponse";
import useApi from "../../hooks/useApi";
import { toast } from "react-toastify";
import { HTTPStatusCode } from "../../helpers/enums/HTTPStatusCode";
import Dialog from "../common/Dialog";
import LabelText from "../common/LabelText";
import MessageText from "../common/MessageText";
import { MessageType } from "../../helpers/enums/MessageTypeEnum";
import Button, { ButtonStyle } from "../buttons/Button";
import { faShoppingCart, faXmark } from "@fortawesome/free-solid-svg-icons";
import { format, addMonths } from "date-fns";

interface BuyEventPassDialogProps {
  eventPassType?: EventPassType;
  onDialogClose: () => void;
}

const BuyEventPassDialog = forwardRef<HTMLDialogElement, BuyEventPassDialogProps>(
  ({ eventPassType, onDialogClose }: BuyEventPassDialogProps, ref) => {
    const {
      data: paymentResponse,
      statusCode: createPaymentStatusCode,
      post: createBuyPassPaymentTransaction,
    } = useApi<PaymentRedirectResponse, EventPassRequest>(ApiEndpoint.EventPassCreateBuyTransaction);

    const onSubmit = async () => {
      if (eventPassType !== undefined) {
        const data: EventPassRequest = {
          passTypeId: eventPassType.id,
        };
        await toast.promise(createBuyPassPaymentTransaction({ id: undefined, body: data }), {
          pending: "Trwa przetwarzanie płatności",
          error: "Wystąpił błąd podczas przetwarzania płatności",
        });
      }
    };

    useEffect(() => {
      if (createPaymentStatusCode == HTTPStatusCode.Ok && paymentResponse.length == 1) {
        const redirectUri = paymentResponse[0].redirectUri;
        window.location.href = redirectUri;
      }
    }, [paymentResponse]);

    return (
      <div>
        {eventPassType && (
          <Dialog ref={ref} onClose={onDialogClose} top={350}>
            <article className="flex flex-col justify-center items-center gap-5 max-w-[750px]">
              <div className="flex flex-col justify-center items-center gap-2">
                <h2>Zakup karnetu</h2>
                <p className="text-textPrimary text-base text-center">
                  Informacje o dokonywanym zakupie karnetu:
                </p>
              </div>
              <div className="flex flex-col justify-center items-center gap-5">
                <div className="flex flex-row justify-center items-center gap-10">
                  <div className="flex flex-col justify-start items-start gap-2 min-w-[300px]">
                    <LabelText
                      labelWidth={150}
                      label="Data zakupu:"
                      text={format(new Date(), "dd.MM.yyyy")}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={150}
                      label="Data zakończenia:"
                      text={format(
                        addMonths(new Date(), eventPassType.validityPeriodInMonths),
                        "dd.MM.yyyy"
                      )}
                      gap={10}
                    />
                    <LabelText labelWidth={150} label="Typ płatności:" text="Płatność online" gap={10} />
                    <LabelText
                      label="Typ karnetu:"
                      labelWidth={150}
                      text={eventPassType.name}
                      gap={10}
                    />
                  </div>
                  <div className="flex flex-col justify-start items-start gap-2 min-w-[320px]">
                    <LabelText
                      labelWidth={210}
                      label="Długość karnetu (msc):"
                      text={eventPassType.validityPeriodInMonths}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={210}
                      label="Cena karnetu:"
                      text={`${eventPassType.price} zł`}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={210}
                      label="Zniżka przy przedłużeniu:"
                      text={`${eventPassType.renewalDiscountPercentage} %`}
                      gap={10}
                    />
                  </div>
                </div>
                <div className="flex flex-col justify-center items-center gap-2">
                  <MessageText
                    messageType={MessageType.Info}
                    text={`Po zakupie karnetu otrzymasz wiadomość email z karnetem w
                            formie pliku PDF wraz z potwierdzeniem płatności.`}
                  />
                </div>
                <div className="flex flex-col gap-3 w-full">
                  <div className="bg-textPrimary w-full h-[1px]"></div>
                  <div className="self-end flex flex-col gap-1">
                    <LabelText
                      fontSize={20}
                      label="Cena karnetu:"
                      text={`${eventPassType.price} PLN`}
                      between={true}
                      isLabelBold={false}
                      gap={10}
                      bgColor="#efefef"
                      px={10}
                      py={6}
                    />
                    <LabelText
                      between={true}
                      fontSize={20}
                      isTextBold={true}
                      label="Suma:"
                      text={`${eventPassType.price} PLN`}
                      px={10}
                      py={6}
                      gap={10}
                    />
                  </div>
                  <div className="bg-textPrimary w-full h-[1px]"></div>
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
                  <Button
                    text="Kup karnet"
                    icon={faShoppingCart}
                    width={180}
                    height={45}
                    rounded="rounded-full"
                    action={onSubmit}
                    style={ButtonStyle.Primary}
                  />
                </div>
              </div>
            </article>
          </Dialog>
        )}
      </div>
    );
  }
);
export default BuyEventPassDialog;
