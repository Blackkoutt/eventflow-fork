import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { EventPass, Hall } from "../../../models/response_models";
import DateFormatter from "../../../helpers/DateFormatter";
import { DateFormat } from "../../../helpers/enums/DateFormatEnum";
import Button, { ButtonStyle } from "../../buttons/Button";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import { HTTPStatusCode } from "../../../helpers/enums/HTTPStatusCode";
import { faCheck, faXmark } from "@fortawesome/free-solid-svg-icons";
import LabelText from "../../common/LabelText";
import { toast } from "react-toastify";
import MessageText from "../../common/MessageText";
import { MessageType } from "../../../helpers/enums/MessageTypeEnum";

interface DeleteHallDialogProps {
  hall?: Hall;
  onDialogConfirm: () => void;
  onDialogClose: () => void;
}

const DeleteHallDialog = forwardRef<HTMLDialogElement, DeleteHallDialogProps>(
  ({ hall, onDialogClose, onDialogConfirm }: DeleteHallDialogProps, ref) => {
    const { del: deleteHall, statusCode: statusCode } = useApi<Hall>(ApiEndpoint.Hall);
    const [actionPerformed, setActionPerformed] = useState(false);
    const [promisePending, setPromisePending] = useState(false);

    const onDeleteHall = async () => {
      if (hall !== undefined) {
        setPromisePending(true);
        await toast.promise(deleteHall({ id: hall.id }), {
          pending: "Wykonywanie żądania",
          success: "Sala została usunięta pomyślnie",
          error: "Wystąpił błąd podczas usuwania sali",
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
        {hall && (
          <Dialog ref={ref}>
            <article className="flex flex-col justify-center items-center gap-5 max-w-[550px]">
              <div className="flex flex-col justify-center items-center gap-2">
                <h2>Usuwanie sali</h2>{" "}
                <p className="text-textPrimary text-base text-center">
                  Czy na pewno chcesz usunąć tą salę ?
                </p>
              </div>
              <div className="flex flex-col justify-start items-start gap-2">
                <LabelText labelWidth={100} label="ID:" text={hall.id} gap={10} />
                <LabelText labelWidth={100} label="Nr sali:" text={hall.hallNr} gap={10} />
                <LabelText labelWidth={100} label="Piętro:" text={hall.floor} gap={10} />
                <LabelText labelWidth={100} label="Ilość miejsc:" text={hall.seatsCount} gap={10} />
                <LabelText labelWidth={100} label="Typ:" text={hall.type?.name} gap={10} />
              </div>
              <div className="flex flex-col justify-center items-center gap-2">
                <MessageText
                  messageType={MessageType.Error}
                  text={`[UWAGA] Tej operacji nie da się cofnąć. Usunięcie tej sali spowoduje odwołanie wszytskich aktywnych rezerwacji sal oraz wydarzeń powiązanych z tą salą. Operacja ta może chwilę potrwać.`}
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
                  action={onDeleteHall}
                ></Button>
              </div>
            </article>
          </Dialog>
        )}
      </div>
    );
  }
);
export default DeleteHallDialog;
