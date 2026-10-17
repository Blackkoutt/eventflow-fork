import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { FAQ } from "../../../models/response_models";
import Button, { ButtonStyle } from "../../buttons/Button";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import { HTTPStatusCode } from "../../../helpers/enums/HTTPStatusCode";
import { faCheck, faXmark } from "@fortawesome/free-solid-svg-icons";
import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import FormButton from "../../common/forms/FormButton";
import { toast } from "react-toastify";
import { FAQRequest, FAQSchema } from "../../../models/create_schemas/FAQSchema";
import TextArea from "../../common/forms/TextArea";

interface CreateFAQDialogProps {
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogSuccess: () => void;
  onDialogClose: () => void;
}

const CreateFAQDialog = forwardRef<HTMLDialogElement, CreateFAQDialogProps>(
  ({ maxWidth, minWidth, onDialogClose, paddingX, onDialogSuccess }: CreateFAQDialogProps, ref) => {
    const { statusCode: statusCode, post: postItem } = useApi<FAQ, FAQRequest>(ApiEndpoint.FAQ);

    const [actionPerformed, setActionPerformed] = useState(false);

    const methods = useForm<FAQRequest>({
      resolver: zodResolver(FAQSchema),
    });

    const { handleSubmit, formState, reset } = methods;
    const { errors, isSubmitting } = formState;

    const onSubmit: SubmitHandler<FAQRequest> = async (data) => {
      console.log(data);
      await toast.promise(postItem({ body: data }), {
        pending: "Wykonywanie żądania",
        success: "Pozycja FAQ została pomyślnie utworzona",
        error: "Wystąpił błąd podczas tworzenia pozycji FAQ",
      });
      setActionPerformed(true);
    };

    useEffect(() => {
      if (actionPerformed) {
        console.log(statusCode);
        if (statusCode == HTTPStatusCode.Created) {
          reset();
          onDialogSuccess();
        }
        setActionPerformed(false);
      }
    }, [actionPerformed]);

    return (
      <Dialog
        ref={ref}
        maxWidth={maxWidth}
        paddingLeft={paddingX}
        paddingRight={paddingX}
        minWidth={minWidth}
        onClose={() => {
          onDialogClose();
          reset();
        }}
      >
        <div className="flex flex-col justify-center items-center pt-2 pb-1">
          <h3 className="text-center font-semibold text-[24px]">Tworzenie pozycji FAQ</h3>
        </div>
        <FormProvider {...methods}>
          <form
            className="flex flex-col justify-center items-center gap-3 w-full mt-4"
            onSubmit={handleSubmit(onSubmit)}
          >
            <div className="flex flex-col justify-center items-center gap-2 w-full">
              <TextArea
                label="Pytanie"
                name="question"
                maxLength={100}
                error={errors.question}
                isFirstLetterUpperCase={true}
                errorHeight={15}
              />
              <TextArea
                label="Odpowiedź"
                name="answer"
                maxLength={1000}
                error={errors.answer}
                isFirstLetterUpperCase={true}
                errorHeight={15}
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
                action={() => {
                  onDialogClose();
                  reset();
                }}
              ></Button>
              <FormButton
                text="Zatwierdź"
                width={145}
                height={45}
                icon={faCheck}
                iconSize={18}
                isSubmitting={isSubmitting}
                rounded={999}
              />
            </div>
          </form>
        </FormProvider>
      </Dialog>
    );
  }
);
export default CreateFAQDialog;
