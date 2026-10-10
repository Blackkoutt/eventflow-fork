import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { EventPass } from "../../../models/response_models";
import DateFormatter from "../../../helpers/DateFormatter";
import { DateFormat } from "../../../helpers/enums/DateFormatEnum";
import Button, { ButtonStyle } from "../../buttons/Button";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import { HTTPStatusCode } from "../../../helpers/enums/HTTPStatusCode";
import { faCheck, faXmark } from "@fortawesome/free-solid-svg-icons";
import LabelText from "../../common/LabelText";
import { toast } from "react-toastify";

interface CancelEventPassDialogProps {
  eventPass?: EventPass;
  onDialogConfirm: () => void;
  onDialogClose: () => void;
}

const CancelEventPassDialog = forwardRef<HTMLDialogElement, CancelEventPassDialogProps>(
  ({ eventPass, onDialogClose, onDialogConfirm }: CancelEventPassDialogProps, ref) => {
    const { del: cancelEventPass, statusCode: statusCode } = useApi<EventPass>(
      ApiEndpoint.EventPass
    );
    const [actionPerformed, setActionPerformed] = useState(false);
    const [promisePending, setPromisePending] = useState(false);

    const onCancelEventPass = async () => {
      if (eventPass !== undefined) {
        setPromisePending(true);
        await toast.promise(cancelEventPass({ id: eventPass.id }), {
          pending: "Wykonywanie żądania",
          success: "Karnet został anulowany pomyślnie",
          error: "Wystąpił błąd podczas anulowania karnetu",
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
        {eventPass && (
          <Dialog ref={ref}>
            <article className="flex flex-col justify-center items-center gap-5 max-w-[550px]">
              <div className="flex flex-col justify-center items-center gap-2">
                <h2>Anulowanie karnetu</h2>{" "}
                <p className="text-textPrimary text-base text-center">
                  Czy na pewno chcesz anulować ten karnet ?
                </p>
              </div>
              <div className="flex flex-col justify-start items-start gap-2">
                <LabelText label="ID:" labelWidth={140} text={eventPass.id} gap={10} />
                <LabelText
                  label="Użytkownik:"
                  labelWidth={150}
                  text={`${eventPass.user?.name} ${eventPass.user?.surname}`}
                  gap={10}
                />
                <LabelText
                  label="E-mail:"
                  labelWidth={150}
                  text={eventPass.user?.emailAddress}
                  gap={10}
                />
                <LabelText
                  labelWidth={150}
                  label="Data zakupu:"
                  text={DateFormatter.FormatDate(eventPass.startDate, DateFormat.Date)}
                  gap={10}
                />
                <LabelText
                  labelWidth={150}
                  label="Data przedłużenia:"
                  text={DateFormatter.FormatDate(eventPass.renewalDate, DateFormat.Date)}
                  gap={10}
                />
                <LabelText
                  label="Data zakończenia:"
                  labelWidth={150}
                  text={DateFormatter.FormatDate(eventPass.endDate, DateFormat.Date)}
                  gap={10}
                />
                <LabelText
                  label="Typ karnetu:"
                  labelWidth={150}
                  text={eventPass.passType?.name}
                  gap={10}
                />
                <LabelText
                  label="Kwota całkowita:"
                  labelWidth={150}
                  text={`${eventPass.paymentAmount} zł`}
                  gap={10}
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
                  action={onCancelEventPass}
                ></Button>
              </div>
            </article>
          </Dialog>
        )}
      </div>
    );
  }
);
export default CancelEventPassDialog;
