import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { Equipment, HallType } from "../../../models/response_models";
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
import TextArea from "../../common/forms/TextArea";
import { HallTypeRequest, HallTypeSchema } from "../../../models/create_schemas/HallTypeSchema";
import MultiSelect from "../../common/forms/MultiSelect";
import { SelectOption } from "../../../helpers/SelectOptions";

interface CreateHallTypeDialogProps {
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogSuccess: () => void;
  onDialogClose: () => void;
}

const CreateHallTypeDialog = forwardRef<HTMLDialogElement, CreateHallTypeDialogProps>(
  (
    { maxWidth, minWidth, onDialogClose, paddingX, onDialogSuccess }: CreateHallTypeDialogProps,
    ref
  ) => {
    const { statusCode: statusCode, post: postItem } = useApi<HallType, FormData>(
      ApiEndpoint.HallType
    );
    const { data: equipments, get: getEquipment } = useApi<Equipment>(ApiEndpoint.Equipment);

    const [selectOptions, setSelectOptions] = useState<SelectOption[]>([]);
    const [actionPerformed, setActionPerformed] = useState(false);

    useEffect(() => {
      getEquipment({ id: undefined, queryParams: undefined });
    }, []);

    useEffect(() => {
      const selectOptions: SelectOption[] = equipments.map(
        (eq) =>
          ({
            value: eq.id,
            option: eq.name,
          } as SelectOption)
      );
      setSelectOptions(selectOptions);
    }, [equipments]);

    const methods = useForm<HallTypeRequest>({
      resolver: zodResolver(HallTypeSchema),
    });
    const { handleSubmit, formState, reset } = methods;
    const { errors, isSubmitting } = formState;

    console.log(errors);

    const onSubmit: SubmitHandler<HallTypeRequest> = async (data) => {
      console.log(data);
      const formData = new FormData();
      formData.append("Name", data.name);
      formData.append("Description", data.description || "");
      data.equipmentIds.forEach((id) => {
        formData.append("EquipmentIds[]", id.toString());
      });
      formData.append("HallTypePhoto", data.hallTypePhoto);
      await toast.promise(postItem({ body: formData }), {
        pending: "Wykonywanie żądania",
        success: "Typ sali został pomyślnie utworzony",
        error: "Wystąpił błąd podczas tworzenia typu sali",
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
          <h3 className="text-center font-semibold text-[24px]">Tworzenie typu sali</h3>
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
                maxLength={30}
                error={errors.name}
                isFirstLetterUpperCase={true}
                errorHeight={15}
              />
              <TextArea
                label="Opis"
                name="description"
                maxLength={600}
                error={errors.description}
                isFirstLetterUpperCase={true}
                errorHeight={15}
              />
              <MultiSelect
                label="Wyposażenie"
                name="equipmentIds"
                optionValues={selectOptions}
                error={errors.equipmentIds}
                errorHeight={15}
              />
              <Input
                label="Zdjęcie typu sali"
                type="file"
                name="hallTypePhoto"
                error={errors.hallTypePhoto}
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
export default CreateHallTypeDialog;
