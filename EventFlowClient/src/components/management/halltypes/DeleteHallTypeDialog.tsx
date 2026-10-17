import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { HallType } from "../../../models/response_models";
import Button, { ButtonStyle } from "../../buttons/Button";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import { HTTPStatusCode } from "../../../helpers/enums/HTTPStatusCode";
import { faCheck, faXmark } from "@fortawesome/free-solid-svg-icons";
import LabelText from "../../common/LabelText";
import { toast } from "react-toastify";
import MessageText from "../../common/MessageText";
import { MessageType } from "../../../helpers/enums/MessageTypeEnum";
import ApiClient from "../../../services/api/ApiClientService";

interface DeleteHallTypeDialogProps {
  item?: HallType;
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogSuccess: () => void;
  onDialogClose: () => void;
}

const DeleteHallTypeDialog = forwardRef<HTMLDialogElement, DeleteHallTypeDialogProps>(
  (
    {
      item,
      maxWidth = 750,
      minWidth,
      onDialogClose,
      paddingX,
      onDialogSuccess,
    }: DeleteHallTypeDialogProps,
    ref
  ) => {
    const { del: deleteItem, statusCode: statusCode } = useApi<HallType>(ApiEndpoint.HallType);
    const [actionPerformed, setActionPerformed] = useState(false);
    const [promisePending, setPromisePending] = useState(false);

    const onDelete = async () => {
      if (item !== undefined) {
        setPromisePending(true);
        await toast.promise(deleteItem({ id: item.id }), {
          pending: "Wykonywanie żądania",
          success: "Typ sali został pomyślnie usunięty",
          error: "Wystąpił błąd podczas usuwania typu sali",
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
                <h2>Usunięcie typu sali</h2>{" "}
                <p className="text-textPrimary text-base text-center">
                  Czy na pewno chcesz usunąć ten typ sali ?
                </p>
              </div>
              <div className="flex flex-col justify-center items-center gap-2">
                <LabelText labelWidth={133} label="ID:" text={item.id} gap={10} />
                <LabelText labelWidth={133} label="Nazwa:" text={item.name} gap={10} />
                <LabelText labelWidth={133} label="Opis:" text={item.description} gap={10} />
                <LabelText
                  labelWidth={133}
                  label="Wyposażenie:"
                  text={item.equipments.map((eq) => eq.name).join(", ")}
                  gap={10}
                />
                <div className="flex flex-row self-start items-center gap-2">
                  <p
                    style={{ fontSize: 16, width: 133 }}
                    className={`font-bold text-end text-textPrimary hover:cursor-default`}
                  >
                    Zdjęcie:
                  </p>
                  <img
                    src={`${ApiClient.GetPhotoEndpoint(item.photoEndpoint)}`}
                    alt={`Zdjęcie typu sali o id ${item.id}`}
                    className="w-[100px] h-[100px] object-contain"
                  />
                </div>
              </div>
              <div className="flex flex-col justify-center items-center gap-2">
                <MessageText
                  messageType={MessageType.Info}
                  text={`Usunięcie typu sali sprawi, że nie będzie on możliwy do wybrania przy tworzeniu nowych sal oraz modyfikacji. Pamiętaj, że te typ sali może jednak wciąż występować w aktualnie istniejącyh salach.`}
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
export default DeleteHallTypeDialog;
