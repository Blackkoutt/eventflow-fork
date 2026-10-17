import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { EventCategory } from "../../../models/response_models";
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
import {
  EventCategoryUpdateRequest,
  EventCategoryUpdateSchema,
} from "../../../models/update_schemas/EventCategoryUpdateSchema";
import Select from "../../common/forms/Select";
import * as solidIcons from "@fortawesome/free-solid-svg-icons";
import { SelectOption } from "../../../helpers/SelectOptions";

interface ModifyEventCategoryDialogProps {
  item?: EventCategory;
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogSuccess: () => void;
  onDialogClose: () => void;
}

const ModifyEventCategoryDialog = forwardRef<HTMLDialogElement, ModifyEventCategoryDialogProps>(
  (
    {
      item,
      maxWidth,
      minWidth,
      onDialogClose,
      paddingX,
      onDialogSuccess,
    }: ModifyEventCategoryDialogProps,
    ref
  ) => {
    const { statusCode: statusCode, put: putItem } = useApi<
      EventCategory,
      undefined,
      EventCategoryUpdateRequest
    >(ApiEndpoint.EventCategory);

    const [actionPerformed, setActionPerformed] = useState(false);

    console.log(item);
    const methods = useForm<EventCategoryUpdateRequest>({
      resolver: zodResolver(EventCategoryUpdateSchema),
      defaultValues: {
        name: item?.name,
        icon: item?.icon,
        color: item?.color,
      },
    });
    const { handleSubmit, formState, reset } = methods;
    const { errors, isSubmitting } = formState;

    const iconClassNames: SelectOption[] = Array.from(
      new Map(
        Object.keys(solidIcons)
          .filter((key) => key.startsWith("fa"))
          .map((key) => {
            const iconClass = `fa-solid ${key.replace(/([A-Z0-9])/g, "-$1").toLowerCase()}`;
            return [iconClass, { value: iconClass, option: iconClass }];
          })
      ).values()
    );

    useEffect(() => {
      if (item) {
        reset({
          name: item?.name,
          icon: item?.icon,
          color: item?.color,
        });
      }
    }, [item, reset]);

    //console.log(item);

    const onSubmit: SubmitHandler<EventCategoryUpdateRequest> = async (data) => {
      console.log(data);
      await toast.promise(putItem({ id: item?.id, body: data }), {
        pending: "Wykonywanie żądania",
        success: "Kategoria wydarzenia została pomyślnie zmodyfikowana",
        error: "Wystąpił błąd podczas modyfikacji kategorii wydarzenia",
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
            <h3 className="text-center font-semibold text-[24px]">
              Modyfikacja kategorii wydarzenia
            </h3>
            <p className="text-textPrimary text-base text-center">
              ID: {item.id}, Nazwa: {item.name}
            </p>
          </div>
          <FormProvider {...methods}>
            <form
              className="flex flex-col justify-center items-center gap-3 w-full mt-4"
              onSubmit={handleSubmit(onSubmit)}
            >
              <div className="flex flex-col justify-center items-center gap-3 w-full px-10">
                <Input
                  label="Nazwa"
                  type="text"
                  name="name"
                  maxLength={30}
                  error={errors.name}
                  isFirstLetterUpperCase={true}
                  errorHeight={15}
                />
                <Select
                  label="Ikona"
                  name="icon"
                  isIcons={true}
                  optionValues={iconClassNames}
                  error={errors.icon}
                  errorHeight={15}
                />
                <Input
                  label="Kolor"
                  type="color"
                  name="color"
                  error={errors.color}
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
export default ModifyEventCategoryDialog;
