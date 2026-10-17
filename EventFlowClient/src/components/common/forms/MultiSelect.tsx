import { useEffect, useState } from "react";
import { FieldError, FieldErrorsImpl, Merge, useFormContext } from "react-hook-form";
import { SelectOption } from "../../../helpers/SelectOptions";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronUp } from "@fortawesome/free-solid-svg-icons";

export interface MultiSelectProps extends React.SelectHTMLAttributes<HTMLSelectElement> {
  label: string;
  name: string;
  errorHeight?: number;
  isIcons?: boolean;
  maxHeight?: number;
  maxSelectedItemsHeight?: number;
  optionValues: SelectOption[];
  selectedOptions?: SelectOption[];
  error:
    | FieldError
    | Merge<FieldError, (FieldError | undefined)[]>
    | Merge<FieldError, FieldErrorsImpl<any>>
    | undefined;
}

const MultiSelect = ({
  label,
  error,
  isIcons = false,
  name,
  maxSelectedItemsHeight = 100,
  errorHeight,
  maxHeight = 250,
  optionValues,
  selectedOptions = [],
  ...props
}: MultiSelectProps) => {
  const [isFocused, setIsFocused] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
  const [selectedValues, setSelectedValues] = useState<SelectOption[]>(selectedOptions);
  const { register, setValue, setFocus } = useFormContext();

  useEffect(() => {
    if (selectedOptions.length > 0) {
      setValue(
        name,
        selectedOptions.map((opt) => opt.value)
      );
      setSelectedValues(selectedOptions);
    }
  }, [selectedOptions]);

  useEffect(() => {
    if (isFocused) setFocus(name);
  }, [isFocused]);

  const handleSelect = (option: SelectOption) => {
    const updatedSelection = selectedValues.some((item) => item.value === option.value)
      ? selectedValues.filter((item) => item.value !== option.value) // Remove if already selected
      : [...selectedValues, option]; // Add new option

    setSelectedValues(updatedSelection);
    setValue(
      name,
      updatedSelection.map((s) => s.value)
    );
  };

  const handleRemove = (value: string | number) => {
    const updatedSelection = selectedValues.filter((item) => item.value !== value);
    setSelectedValues(updatedSelection);
    setValue(
      name,
      updatedSelection.map((s) => s.value)
    );
  };

  const errorMessage = error ? (error as FieldError)?.message : undefined;

  return (
    <div className="w-full">
      <div
        className={`w-full relative bg-[#ECECEC] h-auto rounded-md px-3 py-2
        ${isFocused ? "outline outline-primaryPurple outline-2" : "outline-none"}`}
        tabIndex={1}
        onFocus={() => setIsFocused(true)}
        onBlur={() => {
          setIsFocused(false);
          setIsOpen(false);
        }}
        onClick={() => setIsOpen(!isOpen)}
      >
        <div className="w-full relative">
          <label
            htmlFor={name}
            className="absolute select-none -translate-y-4 translate-x-[2px] text-[12px] text-black hover:cursor-pointer"
          >
            {label}
          </label>

          <select
            {...register(name)}
            name={name}
            id={name}
            multiple
            className="w-full hidden"
            {...props}
          />

          {/* Selected Items */}
          <div
            style={{ width: "calc(100% - 35px)", maxHeight: maxSelectedItemsHeight }}
            className="flex flex-wrap gap-2 mt-[14px] w-[100%] min-h-[28px] overflow-y-scroll"
          >
            {selectedValues.map((option) => (
              <div
                key={option.value}
                className="bg-primaryPurple mt-1 text-white rounded-md px-3 py-1 text-sm flex items-center gap-2"
              >
                {isIcons && (
                  <i
                    key={option.value}
                    className={`${option.value} p-3 text-[18px] rounded-lg cursor-pointer hover:bg-primaryPurple hover:text-white select-none`}
                    onClick={() => handleSelect(option)}
                  ></i>
                )}
                <span>{option.option}</span>
                <button
                  type="button"
                  className="hover:text-red-400"
                  onClick={(e) => {
                    e.stopPropagation();
                    handleRemove(option.value);
                  }}
                >
                  ✕
                </button>
              </div>
            ))}
          </div>
          {/* Dropdown Toggle Icon */}
          <FontAwesomeIcon
            className={`absolute right-3 top-1 transition-transform duration-300 hover:cursor-pointer ${
              isOpen ? "rotate-180" : "rotate-0"
            }`}
            icon={faChevronUp}
          />
        </div>

        {/* Dropdown Options */}
        {isOpen && (
          <div
            className="select-items overflow-y-auto flex flex-col gap-2 absolute left-0 right-0 top-full mt-1 bg-[#efefef] text-black rounded-lg shadow-lg z-10"
            style={{ maxHeight: maxHeight }}
            onClick={(e) => e.stopPropagation()}
          >
            {optionValues.map((optionValue) => (
              <div
                key={optionValue.value}
                className={`p-3 rounded-lg cursor-pointer ${
                  selectedValues.some((item) => item.value === optionValue.value)
                    ? "bg-primaryPurple text-white"
                    : "hover:bg-primaryPurple hover:text-white"
                }`}
                onClick={() => handleSelect(optionValue)}
              >
                {isIcons && <i className={`${optionValue.value} text-[18px] mr-2`}></i>}
                {optionValue.option}
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Error Message */}
      <div style={{ minHeight: errorHeight }}>
        {errorMessage && <div className="text-red-500 text-[14.5px]">{errorMessage}</div>}
      </div>
    </div>
  );
};

export default MultiSelect;
