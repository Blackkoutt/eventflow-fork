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
import {
  HallTypeUpdateRequest,
  HallTypeUpdateSchema,
} from "../../../models/update_schemas/HallTypeUpdateSchema";
import MultiSelect from "../../common/forms/MultiSelect";
import { SelectOption } from "../../../helpers/SelectOptions";

interface ModifyHallTypeDialogProps {
  item?: HallType;
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogSuccess: () => void;
  onDialogClose: () => void;
}

const ModifyHallTypeDialog = forwardRef<HTMLDialogElement, ModifyHallTypeDialogProps>(
  (
    {
      item,
      maxWidth,
      minWidth,
      onDialogClose,
      paddingX,
      onDialogSuccess,
    }: ModifyHallTypeDialogProps,
    ref
  ) => {
    const { statusCode: statusCode, put: putItem } = useApi<HallType, undefined, FormData>(
      ApiEndpoint.HallType
    );
    const { data: equipments, get: getEquipment } = useApi<Equipment>(ApiEndpoint.Equipment);

    const [actionPerformed, setActionPerformed] = useState(false);
    const [selectOptions, setSelectOptions] = useState<SelectOption[]>([]);
    const [selectedEquipment, setSelectedEquipment] = useState<SelectOption[]>([]);

    const methods = useForm<HallTypeUpdateRequest>({
      resolver: zodResolver(HallTypeUpdateSchema),
      defaultValues: {
        name: item?.name,
        description: item?.description ?? null,
        equipmentIds: item?.equipments.map((eq) => eq.id),
      },
    });
    const { handleSubmit, formState, reset } = methods;
    const { errors, isSubmitting } = formState;

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

    useEffect(() => {
      if (item != undefined) {
        const selectedEquipments = equipments.filter((eq) =>
          item?.equipments.some((itemEq) => itemEq.id === eq.id)
        );
        console.log(item);
        const selectedOptions: SelectOption[] = selectedEquipments.map(
          (eq) =>
            ({
              value: eq.id,
              option: eq.name,
            } as SelectOption)
        );
        setSelectedEquipment(selectedOptions);
      }
    }, [item]);

    useEffect(() => {
      console.log(selectedEquipment);
    }, [selectedEquipment]);

    useEffect(() => {
      if (item) {
        reset({
          name: item?.name,
          description: item?.description ?? null,
          equipmentIds: item?.equipments.map((eq) => eq.id),
        });
      }
    }, [item, reset]);

    //console.log(item);

    const onSubmit: SubmitHandler<HallTypeUpdateRequest> = async (data) => {
      console.log(data);
      const formData = new FormData();
      formData.append("Name", data.name);
      formData.append("Description", data.description || "");
      data.equipmentIds.forEach((id) => {
        formData.append("EquipmentIds[]", id.toString());
      });
      formData.append("HallTypePhoto", data.hallTypePhoto);
      await toast.promise(putItem({ id: item?.id, body: formData }), {
        pending: "Wykonywanie żądania",
        success: "Typ sali został pomyślnie zmodyfikowany",
        error: "Wystąpił błąd podczas modyfikacji typu sali",
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
            <h3 className="text-center font-semibold text-[24px]">Modyfikacja typu sali</h3>
            <p className="text-textPrimary text-base text-center">
              ID: {item.id}, Nazwa: {item.name}
            </p>
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
                  selectedOptions={selectedEquipment}
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
export default ModifyHallTypeDialog;
