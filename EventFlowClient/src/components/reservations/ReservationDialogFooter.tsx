import { faShoppingCart, faXmark } from "@fortawesome/free-solid-svg-icons";
import MessageText from "../common/MessageText";
import { MessageType } from "../../helpers/enums/MessageTypeEnum";
import Button, { ButtonStyle } from "../buttons/Button";
import FormButton from "../common/forms/FormButton";

interface ReservationDialogFooterProps {
  isSubmitting: boolean;
  onDialogClose: () => void;
}

const ReservationDialogFooter = ({ isSubmitting, onDialogClose }: ReservationDialogFooterProps) => {
  return (
    <>
      <article className="self-start flex flex-col justify-center items-center mt-4 w-full">
        <MessageText
          maxWidth={330}
          messageType={MessageType.Info}
          fontSize={14}
          iconSize={14}
          text={`Po dokonaniu transakcji otrzymasz wiadomość email z potwierdzeniem rezerwacji
                          w formie pliku PDF.`}
        />
      </article>
      <div className="flex flex-row self-center justify-center items-center gap-2 pt-4">
        <Button
          text="Anuluj"
          width={140}
          height={45}
          icon={faXmark}
          iconSize={18}
          style={ButtonStyle.DefaultGray}
          action={onDialogClose}
        />
        <FormButton
          isSubmitting={isSubmitting}
          text="Kup bilet"
          icon={faShoppingCart}
          width={140}
          height={45}
          rounded={999}
        />
      </div>
    </>
  );
};

export default ReservationDialogFooter;
