import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { Sponsor } from "../../../models/response_models";
import Button, { ButtonStyle } from "../../buttons/Button";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import { HTTPStatusCode } from "../../../helpers/enums/HTTPStatusCode";
import { faCheck, faXmark } from "@fortawesome/free-solid-svg-icons";
import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import FormButton from "../../common/forms/FormButton";
import Input from "../../common/forms/Input";
import { toast } from "react-toastify";
import { SponsorRequest, SponsorSchema } from "../../../models/create_schemas/SponsorSchema";

interface CreateSponsorDialogProps {
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogSuccess: () => void;
  onDialogClose: () => void;
}

const CreateSponsorDialog = forwardRef<HTMLDialogElement, CreateSponsorDialogProps>(
  (
    { maxWidth, minWidth, onDialogClose, paddingX, onDialogSuccess }: CreateSponsorDialogProps,
    ref
  ) => {
    const { statusCode: statusCode, post: postItem } = useApi<Sponsor, FormData>(
      ApiEndpoint.Sponsor
    );
    const [actionPerformed, setActionPerformed] = useState(false);

    const methods = useForm<SponsorRequest>({
      resolver: zodResolver(SponsorSchema),
    });
    const { handleSubmit, formState, reset } = methods;
    const { errors, isSubmitting } = formState;

    const onSubmit: SubmitHandler<SponsorRequest> = async (data) => {
      console.log(data);
      const formData = new FormData();
      formData.append("Kind", "Sponsor");
      formData.append("Name", data.name);
      formData.append("PartnerPhoto", data.sponsorPhoto);
      await toast.promise(postItem({ body: formData }), {
        pending: "Wykonywanie żądania",
        success: "Sponsor został pomyślnie utworzony",
        error: "Wystąpił błąd podczas tworzenia sponsora",
      });
      setActionPerformed(true);
    };

    useEffect(() => {
      if (actionPerformed) {
        if (statusCode == HTTPStatusCode.Created) {
          onDialogSuccess();
          reset();
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
        onClose={onDialogClose}
      >
        <div className="flex flex-col justify-center items-center pt-2 pb-1">
          <h3 className="text-center font-semibold text-[24px]">Tworzenie sponsora</h3>
        </div>
        <FormProvider {...methods}>
          <form
            className="flex flex-col justify-center items-center gap-3 w-full mt-4"
            onSubmit={handleSubmit(onSubmit)}
          >
            <div className="flex flex-col justify-center items-center gap-2 w-full">
              <Input
                label="Nazwa"
                type="text"
                name="name"
                maxLength={50}
                error={errors.name}
                isFirstLetterUpperCase={true}
                errorHeight={15}
              />
              <Input
                label="Zdjęcie sponsora"
                type="file"
                name="sponsorPhoto"
                error={errors.sponsorPhoto}
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
export default CreateSponsorDialog;
