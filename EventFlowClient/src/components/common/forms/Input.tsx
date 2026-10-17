import { IconDefinition } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { FormEvent, useEffect, useState } from "react";
import { faEye, faEyeSlash } from "@fortawesome/free-solid-svg-icons";
import { FieldError, FieldErrorsImpl, Merge, useFormContext } from "react-hook-form";

export interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label: string;
  name: string;
  error?: FieldError | Merge<FieldError, FieldErrorsImpl<any>> | undefined;
  isFirstLetterUpperCase?: boolean;
  icon?: IconDefinition;
  iconwidth?: number;
  onInput?: (e: FormEvent<HTMLInputElement>) => void;
  min?: number;
  minLength?: number;
  pattern?: string;
  max?: number;
  onlyInt?: boolean;
  errorHeight?: number;
  maxLength?: number;
  step?: number;
  iconHeight?: number;
}

const Input = ({
  label,
  icon,
  error,
  isFirstLetterUpperCase = false,
  name,
  min,
  max,
  step = 1,
  pattern,
  maxLength,
  minLength,
  onlyInt = false,
  onInput,
  iconwidth = 24,
  iconHeight = 24,
  errorHeight,
  onChange,
  type = "text",
  ...props
}: InputProps) => {
  const [isFocused, setIsFocused] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const { register, getValues, setFocus, setValue } = useFormContext();

  useEffect(() => {
    if (isFocused) setFocus(name);
  }, [isFocused]);

  const capitalizeFirstLetter = (e: FormEvent<HTMLInputElement>) => {
    const input = e.target as HTMLInputElement;
    if (input.value && /^[a-z]/i.test(input.value)) {
      input.value = input.value.charAt(0).toUpperCase() + input.value.slice(1);
    }
  };

  const phoneNumberOnInput = (e: FormEvent<HTMLInputElement>) => {
    const input = e.target as HTMLInputElement;
    input.value = input.value.replace(/[^0-9\s\+\-\(\)]/g, "");
  };

  const validateEmail = (e: FormEvent<HTMLInputElement>) => {
    const input = e.target as HTMLInputElement;

    // remove all not allowed chars
    input.value = input.value.replace(/[^a-zA-Z0-9à-ÿÀ-ßąćęłńóśźżĄĆĘŁŃÓŚŹŻ._@-]/g, "");

    // remove " " "." "'" "-" from start
    input.value = input.value.replace(/^[ .'-]+/, "");

    // remove repetition of values ( '..', '--', '@@')
    input.value = input.value.replace(/([. '-@])\1+/g, "$1");

    // only one @
    const atCount = (input.value.match(/@/g) || []).length;
    if (atCount > 1) input.value = input.value.replace(/@([^@]*)$/, "");
  };

  const zipCodeOnInput = (e: FormEvent<HTMLInputElement>) => {
    const input = e.target as HTMLInputElement;
    let inputValue = input.value.replace(/\D/g, "");
    if (inputValue.length > 2) {
      inputValue = inputValue.slice(0, 2) + "-" + inputValue.slice(2);
    }
    setValue(name, inputValue);
  };

  const numberOnlyIntOnInput = (e: FormEvent<HTMLInputElement>) => {
    const input = e.target as HTMLInputElement;
    let inputValue = input.value.replace(/\D/g, "");
    if (inputValue.startsWith("0") && inputValue.length > 1) {
      inputValue = inputValue.substring(1);
    }
    setValue(name, inputValue);
  };

  const errorMessage = error ? (error as FieldError)?.message : undefined;

  return (
    <div className="w-full">
      <div
        className={`w-full bg-[#ECECEC] rounded-md px-3
            cursor-text
            flex flex-row justify-start items-center gap-3 ${
              isFocused ? "outline outline-primaryPurple outline-2" : "outline-none"
            }`}
        style={{ cursor: type === "color" ? "pointer" : "text" }}
        tabIndex={1}
        onFocus={() => setIsFocused(true)}
      >
        {icon ? (
          <FontAwesomeIcon
            icon={icon}
            style={{ color: `black`, width: iconwidth, height: iconHeight }}
          />
        ) : null}
        <div className="w-full relative pt-6 pb-2 ">
          <label
            htmlFor={name}
            className={`absolute transition-all text-[#2F2F2F] duration-300 cursor-text ${
              isFocused || getValues(name) || type == "file" || type == "color"
                ? "-translate-y-4 text-[12px] text-black"
                : "-translate-y-[10px] text-lg"
            }`}
          >
            {label}
          </label>

          <input
            {...register(name)}
            name={name}
            id={name}
            min={min}
            max={max}
            step={step}
            maxLength={(() => {
              switch (type) {
                case "tel":
                  return 15;
                case "zipCode":
                  return 6;
                default:
                  return maxLength;
              }
            })()}
            minLength={minLength}
            pattern={pattern}
            type={(type === "password" && showPassword) || type === "zipCode" ? "text" : type}
            className={`w-full inputRef text-[#2F2F2F] font-semibold bg-[#ECECEC] ${
              type === "color" ? "h-[30px]" : ""
            }`}
            style={{ outline: "none", cursor: type === "color" ? "pointer" : "text" }}
            onInput={(e) => {
              switch (type) {
                case "zipCode":
                  zipCodeOnInput(e);
                  break;
                case "tel":
                  phoneNumberOnInput(e);
                  break;
                case "email":
                  validateEmail(e);
                  break;
                case "number":
                  if (onlyInt) numberOnlyIntOnInput(e);
                  break;
                default:
                  if (isFirstLetterUpperCase) capitalizeFirstLetter(e);
                  else onInput ? onInput(e) : undefined;
                  break;
              }
            }}
            {...props}
            onFocus={() => setIsFocused(true)}
            onBlur={() => setIsFocused(false)}
          />
        </div>
        {type === "password" ? (
          <FontAwesomeIcon
            icon={showPassword ? faEye : faEyeSlash}
            style={{ color: `black`, width: iconwidth, height: iconHeight, cursor: "pointer" }}
            onClick={() => setShowPassword(!showPassword)}
          />
        ) : null}
      </div>
      <div style={{ height: errorHeight !== undefined ? `${errorHeight}px` : "auto" }}>
        {errorMessage && <div className="text-red-500 text-[14.5px]">{errorMessage}</div>}
      </div>
    </div>
  );
};

export default Input;
