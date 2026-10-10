import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import {
  UserRegisterRequest,
  userRegisterSchema,
} from "../models/create_schemas/auth/UserRegisterSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import registerHero from "../assets/registerHero.png";
import Input from "../components/common/forms/Input";
import { faEnvelope, faKey, faUserCircle } from "@fortawesome/free-solid-svg-icons";
import { Link, useNavigate } from "react-router-dom";
import DatePicker from "../components/common/forms/DatePicker";
import FormButton from "../components/common/forms/FormButton";
import useApi from "../hooks/useApi";
import { ApiEndpoint } from "../helpers/enums/ApiEndpointEnum";
import { FormEvent, useEffect } from "react";
import { HTTPStatusCode } from "../helpers/enums/HTTPStatusCode";

const RegisterPage = () => {
  const methods = useForm<UserRegisterRequest>({
    resolver: zodResolver(userRegisterSchema),
  });
  const { register, handleSubmit, formState, watch } = methods;
  const { errors, isSubmitting } = formState;

  const {
    statusCode: registerStatusCode,
    post: registerUser,
    error: registerError,
  } = useApi<[], UserRegisterRequest>(ApiEndpoint.AuthRegister);

  const navigate = useNavigate();

  useEffect(() => {
    if (registerStatusCode === HTTPStatusCode.Ok) {
      navigate("/email-verification");
    }
  }, [registerStatusCode]);

  // Register user in application
  const onSubmit: SubmitHandler<UserRegisterRequest> = async (data) => {
    console.log(data);
    //await registerUser({ body: data });
  };

  const validateNameOrSurname = (e: FormEvent<HTMLInputElement>) => {
    const input = e.target as HTMLInputElement;

    input.value = input.value.replace(/[^a-zA-Zà-ÿÀ-ßąćęłńóśźżĄĆĘŁŃÓŚŹŻ' -]/g, "");

    // remove on start
    input.value = input.value.replace(/^[ '’-]+/, "");
    // remove repetitions
    input.value = input.value.replace(/([ '’-])\1+/g, "$1");

    if (input.value && /^[a-zà-ÿÀ-ßąćęłńóśźżĄĆĘŁŃÓŚŹŻ' -]/i.test(input.value)) {
      input.value = input.value.charAt(0).toUpperCase() + input.value.slice(1);
    }
  };

  return (
    <div className="flex flex-row justify-center items-start gap-20 py-16 w-full">
      <div className="flex flex-col justify-center items-start translate-y-[100px]">
        <h2 className="tracking-wide">
          <span className="text-[#2F2F2F] text-4xl font-semibold">Witaj w </span>
          <span className="text-primaryPurple font-extrabold text-5xl">EventFlow!</span>
        </h2>
        <img
          src={registerHero}
          alt="Obrazek powitalny logowania"
          className="object-contain w-[460px] h-[460px]"
        />
      </div>
      <div className="bg-white min-w-[580px] rounded-md drop-shadow-xl p-10 flex flex-col justify-center items-center gap-10">
        <h3 className="text-black font-bold text-4xl">Zarejestruj się</h3>
        <div className="flex flex-col justify-center items-center gap-6 w-full">
          <FormProvider {...methods}>
            <form
              className="flex flex-col justify-center items-center gap-[10px] w-full"
              onSubmit={handleSubmit(onSubmit)}
            >
              <Input
                icon={faUserCircle}
                label="Imię"
                onInput={validateNameOrSurname}
                maxLength={40}
                type="text"
                name="name"
                error={errors.name}
                errorHeight={15}
              />
              <Input
                icon={faUserCircle}
                label="Nazwisko"
                onInput={validateNameOrSurname}
                type="text"
                maxLength={40}
                name="surname"
                error={errors.surname}
                errorHeight={15}
              />

              <Input
                icon={faEnvelope}
                label="Email"
                type="email"
                maxLength={255}
                name="email"
                error={errors.email}
                errorHeight={15}
              />

              <DatePicker
                label="Data urodzenia"
                name="dateOfBirth"
                error={errors.dateOfBirth}
                errorHeight={15}
              />

              <Input
                icon={faKey}
                label="Hasło"
                type="password"
                name="password"
                error={errors.password}
                errorHeight={15}
              />

              <Input
                icon={faKey}
                label="Powtórz hasło"
                type="password"
                name="confirmPassword"
                error={errors.confirmPassword}
                errorHeight={15}
              />
              <FormButton isSubmitting={isSubmitting} text="Zarejestruj się" />
            </form>
          </FormProvider>
          <div className="flex flex-col justify-center items-center gap-6 w-full">
            <div className="flex flex-row justify-center items-center gap-1">
              <p className="text-base text-[#2f2f2f]">Posiadasz już konto? </p>
              <Link to="/sign-in" className="text-[#6358DC] text-base">
                Zaloguj się
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
export default RegisterPage;
