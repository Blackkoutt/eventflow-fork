import { forwardRef, useEffect, useState } from "react";
import { User } from "../../models/response_models";
import ApiClient from "../../services/api/ApiClientService";
import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import { UserInfoRequest, UserInfoSchema } from "../../models/create_schemas/UserInfoSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import Input from "../common/forms/Input";
import FormButton from "../common/forms/FormButton";
import { MaxFileSizeAndTypeValidator } from "../../models/validators/MaxFileSizeValidator";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { HTTPStatusCode } from "../../helpers/enums/HTTPStatusCode";
import Dialog from "../common/Dialog";

interface ChangePhotoDialogProps {
  user: User;
  reloadComponent: () => void;
}

const ChangePhotoDialog = forwardRef<HTMLDialogElement, ChangePhotoDialogProps>(
  ({ user, reloadComponent }: ChangePhotoDialogProps, ref) => {
    const [photo, setPhoto] = useState(ApiClient.GetPhotoEndpoint(user.photoEndpoint));

    const { statusCode: statusCode, put: putInfo } = useApi<User, undefined, FormData>(
      ApiEndpoint.UserInfo
    );

    const [actionPerformed, setActionPerformed] = useState(false);

    const methods = useForm<UserInfoRequest>({
      resolver: zodResolver(UserInfoSchema),
      defaultValues: {
        street: user.street ?? null,
        houseNumber: (user.houseNumber === 0 ? null : user.houseNumber) ?? null,
        flatNumber: (user.flatNumber === 0 ? null : user.flatNumber) ?? null,
        city: user.city ?? null,
        zipCode: user.zipCode ?? null,
        phoneNumber: user.phoneNumber ?? null,
      },
    });
    const { handleSubmit, formState, watch, setValue } = methods;
    const { errors, isSubmitting } = formState;

    useEffect(() => {
      setValue("street", user.street ?? null);
      setValue(
        "houseNumber",
        (user.houseNumber === 0 ? null : user.houseNumber) ?? null
      );
      setValue(
        "flatNumber",
        (user.flatNumber === 0 ? null : user.flatNumber) ?? null
      );
      setValue("city", user.city ?? null);
      setValue("zipCode", user.zipCode ?? null);
      setValue("phoneNumber", user.phoneNumber ?? null);
    }, [user.street, user.houseNumber, user.flatNumber, user.city, user.zipCode, user.phoneNumber]);

    const file = watch().userPhoto;
    const validator = MaxFileSizeAndTypeValidator(10, ["image/jpeg", "image/png"]);

    useEffect(() => {
      if (file != undefined && file.length != 0 && validator.safeParse(file).success) {
        const url = URL.createObjectURL(file[0]);
        setPhoto(url);
      } else if (photo != ApiClient.GetPhotoEndpoint(user.photoEndpoint)) {
        console.log(ApiClient.GetPhotoEndpoint(user.photoEndpoint));
        setPhoto(ApiClient.GetPhotoEndpoint(user.photoEndpoint));
      }
    }, [file]);

    const onSubmit: SubmitHandler<UserInfoRequest> = async (data) => {
      console.log(data);
      const formData = new FormData();
      formData.append("Street", data.street || "");
      formData.append("HouseNumber", data.houseNumber?.toString() || "");
      formData.append("FlatNumber", data.flatNumber?.toString() || "");
      formData.append("City", data.city || "");
      formData.append("ZipCode", data.zipCode || "");
      formData.append("PhoneNumber", data.phoneNumber || "");
      formData.append("UserPhoto", data.userPhoto);
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
      photo && (
        <Dialog ref={ref}>
          <div className="flex flex-col justify-center items-center">
            <div className="bg-gray-200 rounded-lg flex flex-row justify-center items-end p-6 gap-6">
              <div className="flex flex-col justify-center items-center">
                <img
                  src={photo}
                  alt={`Zdjęcie użytkownika ${user.name} ${user.surname} 256px x 256px`}
                  className="w-[256px] h-[256px] object-cover"
                />
                <p className="mt-2">256px x 256px</p>
              </div>
              <div className="flex flex-col justify-center items-center">
                <img
                  src={photo}
                  alt={`Zdjęcie użytkownika ${user.name} ${user.surname} 128px x 128px`}
                  className="w-[128px] h-[128px] object-cover"
                />
                <p className="mt-2">128px x 128px</p>
              </div>
              <div className="flex flex-col justify-center items-center">
                <img
                  src={photo}
                  alt={`Zdjęcie użytkownika ${user.name} ${user.surname} 64px x 64px`}
                  className="w-[64px] h-[64px] object-cover"
                />
                <p className="mt-2">64px x 64px</p>
              </div>
            </div>
            <FormProvider {...methods}>
              <form
                className="flex flex-col justify-center items-center gap-4 w-full mt-4"
                onSubmit={handleSubmit(onSubmit)}
              >
                <Input
                  label="Zdjęcie profilowe"
                  type="file"
                  name="userPhoto"
                  error={errors.userPhoto}
                />
                <div className="w-[200px]">
                  <FormButton isSubmitting={isSubmitting} text="Zmień zdjęcie" py={12} />
                </div>
              </form>
            </FormProvider>
          </div>
        </Dialog>
      )
    );
  }
);
export default ChangePhotoDialog;
