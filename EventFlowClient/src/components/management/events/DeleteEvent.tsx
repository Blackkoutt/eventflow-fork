import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { EventCategory, EventEntity } from "../../../models/response_models";
import Button, { ButtonStyle } from "../../buttons/Button";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import { HTTPStatusCode } from "../../../helpers/enums/HTTPStatusCode";
import { faCheck, faXmark } from "@fortawesome/free-solid-svg-icons";
import LabelText from "../../common/LabelText";
import { toast } from "react-toastify";
import MessageText from "../../common/MessageText";
import { MessageType } from "../../../helpers/enums/MessageTypeEnum";
import DateFormatter from "../../../helpers/DateFormatter";

interface DeleteEventProps {
  item?: EventEntity;
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogSuccess: () => void;
  onDialogClose: () => void;
}

const DeleteEvent = forwardRef<HTMLDialogElement, DeleteEventProps>(
  (
    { item, maxWidth = 750, minWidth, onDialogClose, paddingX, onDialogSuccess }: DeleteEventProps,
    ref
  ) => {
    const { del: deleteItem, statusCode: statusCode } = useApi<EventEntity>(ApiEndpoint.Event);
    const [actionPerformed, setActionPerformed] = useState(false);
    const [promisePending, setPromisePending] = useState(false);

    const onDelete = async () => {
      if (item !== undefined) {
        setPromisePending(true);
        await toast.promise(deleteItem({ id: item.id }), {
          pending: "Wykonywanie żądania",
          success: "Wydarzenie zostało pomyślnie anulowane",
          error: "Wystąpił błąd podczas anulowania wydarzenia",
        });
        setPromisePending(false);
        setActionPerformed(true);
      }
    };

    useEffect(() => {
      if (actionPerformed) {
        if (statusCode == HTTPStatusCode.NoContent) {
          onDialogSuccess();
        }
        setActionPerformed(false);
      }
    }, [actionPerformed]);

    return (
      <div>
        {item && (
          <article className="flex flex-col justify-center items-center gap-5">
            <div className="flex flex-col justify-center items-center gap-2">
              <h2>Anulowanie wydarzenia</h2>{" "}
              <p className="text-textPrimary text-base text-center">
                Czy na pewno chcesz usunąć te wydarzenie ?
              </p>
            </div>
            <div className="flex flex-col justify-center items-center gap-2">
              <LabelText labelWidth={145} isTextLeft={true} label="ID:" text={item?.id} gap={10} />
              <LabelText
                labelWidth={145}
                isTextLeft={true}
                label="Nazwa:"
                text={item?.title}
                gap={10}
              />
              <LabelText
                labelWidth={145}
                isTextLeft={true}
                label="Data rozpoczęcia:"
                text={DateFormatter.FormatDateFromCalendar(item?.start)}
                gap={10}
              />
              <LabelText
                labelWidth={145}
                isTextLeft={true}
                label="Data zakończenia:"
                text={DateFormatter.FormatDateFromCalendar(item?.end)}
                gap={10}
              />
              <LabelText
                labelWidth={145}
                isTextLeft={true}
                label="Kategoria:"
                text={item?.category?.name}
                gap={10}
              />
              <LabelText
                labelWidth={145}
                isTextLeft={true}
                label="Nr sali:"
                text={item?.hall?.hallNr}
                gap={10}
              />
            </div>
            <div className="flex flex-col justify-center items-center gap-2">
              <MessageText
                messageType={MessageType.Error}
                maxWidth={600}
                text={`Anulowanie wydarzenia sprawi, że wszystkie rezerwacje na to wydarzenie zostaną anulowane. Konieczny będzie zwrot kosztów wszystkich rezerwacji.`}
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
                action={onDelete}
              ></Button>
            </div>
          </article>
        )}
      </div>
    );
  }
);
export default DeleteEvent;
