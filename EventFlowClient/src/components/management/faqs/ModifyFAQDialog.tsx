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
import { FAQUpdateRequest, FAQUpdateSchema } from "../../../models/update_schemas/FAQUpdateSchema";
import TextArea from "../../common/forms/TextArea";

interface ModifyFAQDialogProps {
  item?: FAQ;
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogSuccess: () => void;
  onDialogClose: () => void;
}

const ModifyFAQDialog = forwardRef<HTMLDialogElement, ModifyFAQDialogProps>(
  (
    { item, maxWidth, minWidth, onDialogClose, paddingX, onDialogSuccess }: ModifyFAQDialogProps,
    ref
  ) => {
    const { statusCode: statusCode, put: putItem } = useApi<FAQ, undefined, FAQUpdateRequest>(
      ApiEndpoint.FAQ
    );

    const [actionPerformed, setActionPerformed] = useState(false);

    console.log(item);
    const methods = useForm<FAQUpdateRequest>({
      resolver: zodResolver(FAQUpdateSchema),
      defaultValues: {
        question: item?.question,
        answer: item?.answer,
      },
    });
    const { handleSubmit, formState, reset } = methods;
    const { errors, isSubmitting } = formState;

    useEffect(() => {
      if (item) {
        reset({
          question: item?.question,
          answer: item?.answer,
        });
      }
    }, [item, reset]);

    //console.log(item);

    const onSubmit: SubmitHandler<FAQUpdateRequest> = async (data) => {
      console.log(data);
      await toast.promise(putItem({ id: item?.id, body: data }), {
        pending: "Wykonywanie żądania",
        success: "Pozycja FAQ została pomyślnie zmodyfikowana",
        error: "Wystąpił błąd podczas modyfikacji pozycji FAQ",
      });
      setActionPerformed(true);
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
      item && (
        <Dialog
          ref={ref}
          maxWidth={maxWidth}
          paddingLeft={paddingX}
          paddingRight={paddingX}
          minWidth={minWidth}
          onClose={onDialogClose}
        >
          <div className="flex flex-col justify-center items-center gap-2 pt-2 pb-1">
            <h3 className="text-center font-semibold text-[24px]">Modyfikacja pozycji FAQ</h3>
            <p className="text-textPrimary text-base text-center">ID: {item.id}</p>
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
                  action={onDialogClose}
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
      )
    );
  }
);
export default ModifyFAQDialog;
