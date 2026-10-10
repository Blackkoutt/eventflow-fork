import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { Hall, HallRent } from "../../../models/response_models";
import DateFormatter from "../../../helpers/DateFormatter";
import { DateFormat } from "../../../helpers/enums/DateFormatEnum";
import Button, { ButtonStyle } from "../../buttons/Button";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import { HTTPStatusCode } from "../../../helpers/enums/HTTPStatusCode";
import { faCheck, faXmark } from "@fortawesome/free-solid-svg-icons";
import LabelText from "../../common/LabelText";
import { toast } from "react-toastify";
import { MessageType } from "../../../helpers/enums/MessageTypeEnum";
import MessageText from "../../common/MessageText";

interface CancelHallRentDialogProps {
  hallRent?: HallRent;
  onDialogConfirm: () => void;
  onDialogClose: () => void;
}

const CancelHallRentDialog = forwardRef<HTMLDialogElement, CancelHallRentDialogProps>(
  ({ hallRent, onDialogClose, onDialogConfirm }: CancelHallRentDialogProps, ref) => {
    const { del: cancelHallRent, statusCode: statusCode } = useApi<HallRent>(ApiEndpoint.HallRent);
    const { data: hallWidthDetails, get: getHallWithDetails } = useApi<Hall>(ApiEndpoint.Hall);

    useEffect(() => {
      getHallWithDetails({ id: hallRent?.hall?.id, queryParams: undefined });
    }, []);

    const [actionPerformed, setActionPerformed] = useState(false);
    const [promisePending, setPromisePending] = useState(false);

    const onCancelHallRent = async () => {
      if (hallRent !== undefined) {
        setPromisePending(true);
        await toast.promise(cancelHallRent({ id: hallRent.id }), {
          pending: "Wykonywanie żądania",
          success: "Rezerwacja sali została anulowana pomyślnie",
          error: "Wystąpił błąd podczas anulowania rezerwacji sali",
        });
        setPromisePending(false);
        setActionPerformed(true);
      }
    };

    useEffect(() => {
      if (actionPerformed) {
        if (statusCode == HTTPStatusCode.NoContent) {
          onDialogConfirm();
        }
        setActionPerformed(false);
      }
    }, [actionPerformed]);

    return (
      <div>
        {hallRent && hallWidthDetails.length !== 0 && (
          <Dialog ref={ref}>
            <article className="flex flex-col justify-center items-center gap-5 max-w-[750px]">
              <div className="flex flex-col justify-center items-center gap-2">
                <h2>Anulowanie rezerwacji sali</h2>{" "}
                <p className="text-textPrimary text-base text-center">
                  Czy na pewno chcesz anulować tę rezerwację sali ?
                </p>
              </div>
              <div className="flex flex-row justify-center items-center gap-14">
                <div className="flex flex-col justify-start items-start gap-2">
                  <LabelText label="ID:" labelWidth={150} text={hallRent.id} gap={10} />
                  <LabelText
                    labelWidth={150}
                    label="Data rozpoczęcia:"
                    text={DateFormatter.FormatDate(hallRent.startDate, DateFormat.DateTime)}
                    gap={10}
                  />
                  <LabelText
                    labelWidth={150}
                    label="Data zakończenia:"
                    text={DateFormatter.FormatDate(hallRent.endDate, DateFormat.DateTime)}
                    gap={10}
                  />
                  <LabelText
                    labelWidth={150}
                    label="Data płatności:"
                    text={DateFormatter.FormatDate(hallRent.paymentDate, DateFormat.DateTime)}
                    gap={10}
                  />
                  <LabelText
                    label="Typ płatności:"
                    labelWidth={150}
                    text={hallRent.paymentType?.name}
                    gap={10}
                  />
                </div>
                <div className="flex flex-col justify-start items-start gap-2">
                  <LabelText
                    labelWidth={185}
                    label="Nr sali:"
                    text={hallRent.hall?.hallNr}
                    gap={10}
                  />
                  <LabelText
                    labelWidth={185}
                    label="Typ sali:"
                    text={hallRent.hall?.type?.name}
                    gap={10}
                  />
                  <LabelText
                    labelWidth={185}
                    label="Ilość miejsc:"
                    text={hallWidthDetails[0].seats.length}
                    gap={10}
                  />

                  <LabelText
                    labelWidth={185}
                    label="Cena za 1 h:"
                    text={`${hallRent.hall?.rentalPricePerHour} zł`}
                    gap={10}
                  />
                  <LabelText
                    labelWidth={185}
                    label="Cena całkowita:"
                    text={`${hallRent.paymentAmount} zł`}
                    gap={10}
                  />
                </div>
              </div>
              <div className="flex flex-col justify-center items-center gap-2">
                <MessageText
                  messageType={MessageType.Info}
                  text={`Po anulowaniu rezerwacji sali środki wysokości ${hallRent.paymentAmount} zł zostaną zwrócone na
                  twoje konto w przeciągu kilku następnych dni roboczych.`}
                />
              </div>
              <div className="flex flex-row justify-center items-center gap-2">
                <Button
                  text="Anuluj"
                  width={145}
                  height={45}
                  icon={faXmark}
                  iconSize={18}
                  style={ButtonStyle.DefaultGray}
                  action={onDialogClose}
                ></Button>
                <Button
                  text={promisePending ? "Ładowanie..." : "Zatwierdź"}
                  width={145}
                  height={45}
                  icon={faCheck}
                  iconSize={18}
                  style={ButtonStyle.Primary}
                  action={onCancelHallRent}
                ></Button>
              </div>
            </article>
          </Dialog>
        )}
      </div>
    );
  }
);
export default CancelHallRentDialog;
