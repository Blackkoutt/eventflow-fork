import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { EventPassType } from "../../../models/response_models";
import Button, { ButtonStyle } from "../../buttons/Button";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import { HTTPStatusCode } from "../../../helpers/enums/HTTPStatusCode";
import { faCheck, faXmark } from "@fortawesome/free-solid-svg-icons";
import LabelText from "../../common/LabelText";
import { toast } from "react-toastify";
import MessageText from "../../common/MessageText";
import { MessageType } from "../../../helpers/enums/MessageTypeEnum";

interface DeleteEventPassTypeDialogProps {
  item?: EventPassType;
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogSuccess: () => void;
  onDialogClose: () => void;
}

const DeleteEventPassTypeDialog = forwardRef<HTMLDialogElement, DeleteEventPassTypeDialogProps>(
  (
    {
      item,
      maxWidth = 750,
      minWidth,
      onDialogClose,
      paddingX,
      onDialogSuccess,
    }: DeleteEventPassTypeDialogProps,
    ref
  ) => {
    const { del: deleteItem, statusCode: statusCode } = useApi<EventPassType>(
      ApiEndpoint.EventPassType
    );
    const [actionPerformed, setActionPerformed] = useState(false);
    const [promisePending, setPromisePending] = useState(false);

    const onDelete = async () => {
      if (item !== undefined) {
        setPromisePending(true);
        await toast.promise(deleteItem({ id: item.id }), {
          pending: "Wykonywanie żądania",
          success: "Typ karnetu został pomyślnie usunięty",
          error: "Wystąpił błąd podczas usuwania typu karnetu",
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
          <Dialog
            ref={ref}
            maxWidth={maxWidth}
            paddingLeft={paddingX}
            paddingRight={paddingX}
            minWidth={minWidth}
            onClose={onDialogClose}
          >
            <article className="flex flex-col justify-center items-center gap-5">
              <div className="flex flex-col justify-center items-center gap-2">
                <h2>Usunięcie typu karnetu</h2>{" "}
                <p className="text-textPrimary text-base text-center">
                  Czy na pewno chcesz usunąć ten typ karnetu ?
                </p>
              </div>
              <div className="flex flex-col justify-center items-center gap-2 -translate-x-[46px]">
                <LabelText labelWidth={210} label="ID:" text={item.id} gap={10} />
                <LabelText labelWidth={210} label="Nazwa:" text={item.name} gap={10} />
                <LabelText
                  labelWidth={210}
                  label="Długość karnetu:"
                  text={`${item.validityPeriodInMonths} mies`}
                  gap={10}
                />
                <LabelText
                  labelWidth={210}
                  label="Zniżka przy przedłużeniu:"
                  text={`${item.renewalDiscountPercentage} %`}
                  gap={10}
                />
                <LabelText labelWidth={210} label="Cena:" text={`${item.price} zł`} gap={10} />
              </div>
              <div className="flex flex-col justify-center items-center gap-2">
                <MessageText
                  messageType={MessageType.Info}
                  text={`Usunięcie typu karnetu sprawi, że nie będzie można już go wybrać przy zakupie lub przedłużeniu karnetu. Pamiętaj, że typ karnetu może wciąż występować w aktywnych i przeszłych karnetach.`}
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
          </Dialog>
        )}
      </div>
    );
  }
);
export default DeleteEventPassTypeDialog;
