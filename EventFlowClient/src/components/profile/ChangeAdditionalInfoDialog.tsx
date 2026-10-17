import { FormEvent, forwardRef, useEffect, useState } from "react";
import Dialog from "../common/Dialog";
import { User } from "../../models/response_models";
import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import { UserDataRequest, UserDataSchema } from "../../models/create_schemas/UserDataSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import useApi from "../../hooks/useApi";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import Input from "../common/forms/Input";
import FormButton from "../common/forms/FormButton";
import {
  faCity,
  faDoorClosed,
  faEnvelope,
  faHouse,
  faPhone,
  faSignsPost,
} from "@fortawesome/free-solid-svg-icons";
import { HTTPStatusCode } from "../../helpers/enums/HTTPStatusCode";

interface ChangeAdditionalInfoDialogProps {
  user: User;
  reloadComponent: () => void;
}

const ChangeAdditionalInfoDialog = forwardRef<HTMLDialogElement, ChangeAdditionalInfoDialogProps>(
  ({ user, reloadComponent }: ChangeAdditionalInfoDialogProps, ref) => {
    const { statusCode: statusCode, put: putInfo } = useApi<User, undefined, FormData>(
      ApiEndpoint.UserInfo
    );
    const [actionPerformed, setActionPerformed] = useState(false);

    const methods = useForm<UserDataRequest>({
      resolver: zodResolver(UserDataSchema),
      defaultValues: {
        street: user.userData?.street ?? null,
        houseNumber: (user.userData?.houseNumber === 0 ? null : user.userData?.houseNumber) ?? null,
        flatNumber: (user.userData?.flatNumber === 0 ? null : user.userData?.flatNumber) ?? null,
        city: user.userData?.city ?? null,
        zipCode: user.userData?.zipCode ?? null,
        phoneNumber: user.userData?.phoneNumber ?? null,
      },
    });
    const { handleSubmit, formState } = methods;
    const { errors, isSubmitting } = formState;

    const validateCityName = (e: FormEvent<HTMLInputElement>) => {
      const input = e.target as HTMLInputElement;

      input.value = input.value.replace(/[^a-zA-Zà-ÿÀ-ßąćęłńóśźżĄĆĘŁŃÓŚŹŻ' -]/g, "");

      // remove on start
      input.value = input.value.replace(/^[ '’-]+/, "");

      // remove repetitions
      input.value = input.value.replace(/([ '’-])\1+/g, "$1");

      if (input.value && /^[a-zà-ÿÀ-ßąćęłńóśźżĄĆĘŁŃÓŚŹŻ]/i.test(input.value)) {
        input.value = input.value.charAt(0).toUpperCase() + input.value.slice(1);
      }
    };

    const validateStreetName = (e: FormEvent<HTMLInputElement>) => {
      const input = e.target as HTMLInputElement;

      input.value = input.value.replace(/[^A-Za-ząćęłńóśźżĄĆĘŁŃÓŚŹŻ0-9' -]/g, "");

      // remove on start
      input.value = input.value.replace(/^[ '’-]+/, "");

      // remove repetitions
      input.value = input.value.replace(/([ '’-])\1+/g, "$1");

      if (input.value && /^[a-z0-9]/i.test(input.value)) {
        input.value = input.value.charAt(0).toUpperCase() + input.value.slice(1);
      }
    };

    const onSubmit: SubmitHandler<UserDataRequest> = async (data) => {
      console.log(data);
      const formData = new FormData();
      formData.append("Street", data.street || "");
      formData.append("HouseNumber", data.houseNumber?.toString() || "");
      formData.append("FlatNumber", data.flatNumber?.toString() || "");
      formData.append("City", data.city || "");
      formData.append("ZipCode", data.zipCode || "");
      formData.append("PhoneNumber", data.phoneNumber || "");
      formData.append("UserPhoto", "");
      await putInfo({ id: undefined, body: formData });
      setActionPerformed(true);
    };

    useEffect(() => {
      if (actionPerformed) {
        if (statusCode == HTTPStatusCode.NoContent) {
          reloadComponent();
        }
        setActionPerformed(false);
      }
    }, [actionPerformed]);

    return (
      <Dialog ref={ref}>
        <h3 className="text-center font-semibold text-[24px] py-2">
          Zmiana dodatkowych danych osobowych
        </h3>
        <FormProvider {...methods}>
          <form
            className="flex flex-col justify-center items-center gap-3 w-full mt-4"
            onSubmit={handleSubmit(onSubmit)}
          >
            <div className="flex flex-row justify-center items-center gap-6">
              <div className="flex flex-col justify-center items-center gap-3 w-[350px]">
                <Input
                  icon={faSignsPost}
                  label="Ulica"
                  type="text"
                  name="street"
                  maxLength={50}
                  onInput={validateStreetName}
                  error={errors.street}
                  errorHeight={20}
                />
                <Input
                  icon={faHouse}
                  label="Nr domu"
                  min={1}
                  max={999}
                  type="number"
                  onlyInt={true}
                  name="houseNumber"
                  error={errors.houseNumber}
                  errorHeight={20}
                />
                <Input
                  icon={faDoorClosed}
                  label="Nr mieszkania"
                  type="number"
                  onlyInt={true}
                  min={1}
                  max={999}
                  name="flatNumber"
                  error={errors.flatNumber}
                  errorHeight={20}
                />
              </div>
              <div className="flex flex-col justify-center items-center gap-3 w-[350px]">
                <Input
                  icon={faCity}
                  label="Miasto"
                  maxLength={50}
                  type="text"
                  name="city"
                  onInput={validateCityName}
                  error={errors.city}
                  errorHeight={20}
                />
                <Input
                  icon={faEnvelope}
                  label="Kod pocztowy"
                  type="zipCode"
                  name="zipCode"
                  error={errors.zipCode}
                  errorHeight={20}
                />
                <Input
                  label="Nr telefonu"
                  icon={faPhone}
                  type="tel"
                  name="phoneNumber"
                  error={errors.phoneNumber}
                  errorHeight={20}
                />
              </div>
            </div>

            <div className="w-[180px] mt-1">
              <FormButton isSubmitting={isSubmitting} text="Zmień dane" py={12} />
            </div>
          </form>
        </FormProvider>
      </Dialog>
    );
  }
);
export default ChangeAdditionalInfoDialog;
