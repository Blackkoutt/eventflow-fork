import Input from "../components/common/forms/Input";
import { useAuth } from "../context/AuthContext";
import { faEnvelope, faKey } from "@fortawesome/free-solid-svg-icons";
import { UserLoginRequest, userLoginSchema } from "../models/create_schemas/auth/UserLoginSchema";
import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import loginHero from "../assets/loginHero.png";
import Checkbox from "../components/common/Checkbox";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import FormButton from "../components/common/forms/FormButton";
import { toast } from "react-toastify";

const LoginPage = () => {
  const {
    authenticated,
    handleLogin,
    handleLogout,
  } = useAuth();

  const navigate = useNavigate();

  const methods = useForm<UserLoginRequest>({
    resolver: zodResolver(userLoginSchema),
  });
  const { handleSubmit, formState, watch } = methods;
  const { errors, isSubmitting } = formState;

  // Login direct in application
  const onSubmit: SubmitHandler<UserLoginRequest> = async (data) => {
    await handleLogin(data);
  };

  // Redirect after authentication to main page
  useEffect(() => {
    if (authenticated) {
      navigate("/");
    }
  }, [authenticated]);

  return (
    <div className="flex flex-row justify-center items-center gap-20 py-16 w-full">
      <div className="flex flex-col justify-center items-start">
        <h2 className="tracking-wide">
          <span className="text-[#2F2F2F] text-4xl font-semibold">Witaj w </span>
          <span className="text-primaryPurple font-extrabold text-5xl">EventFlow!</span>
        </h2>
        <img
          src={loginHero}
          alt="Obrazek powitalny logowania"
          className="object-contain w-[460px] h-[460px]"
        />
      </div>
      <div className="bg-white min-w-[580px] rounded-md drop-shadow-xl p-10 flex flex-col justify-center items-center gap-10">
        <h3 className="text-black font-bold text-4xl">Zaloguj się</h3>
        <div className="flex flex-col justify-center items-center gap-6 w-full">
          <FormProvider {...methods}>
            <form
              className="flex flex-col justify-center items-center gap-2 w-full"
              onSubmit={handleSubmit(onSubmit)}
            >
              <Input
                icon={faEnvelope}
                label="Email"
                type="email"
                maxLength={255}
                name="email"
                error={errors.email}
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

              <div className="flex flex-row justify-between items-center w-full pb-4">
                <Checkbox
                  color="#7B2CBF"
                  name="remind"
                  textColor="#2F2F2F"
                  fontSize={16}
                  text="Zapamiętaj mnie"
                />
              </div>
              <FormButton isSubmitting={isSubmitting} text="Zaloguj się" />
              {/* {errors.root && <div className="text-red-500">{errors.email.message}</div>} */}
              <div className="flex flex-row justify-center items-center gap-1 pt-4">
                <p className="text-base text-[#2f2f2f]">Nie masz jeszcze konta? </p>
                <Link to="/sign-up" className="text-[#6358DC] text-base">
                  Zarejestruj się
                </Link>
              </div>
            </form>
          </FormProvider>
        </div>
      </div>
    </div>
  );
};
export default LoginPage;
