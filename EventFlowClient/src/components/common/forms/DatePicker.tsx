import React, { useEffect, useState } from "react";
import { faCalendar } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { FieldError, useFormContext } from "react-hook-form";
import DateFormatter from "../../../helpers/DateFormatter";

export interface DatePickerProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label: string;
  name: string;
  error: FieldError | undefined;
  errorHeight?: number;
  isTime?: boolean;
  date?: Date;
}

const DatePicker = React.forwardRef<HTMLInputElement, DatePickerProps>(
  ({ label, name, error, errorHeight, isTime = false, date, type = "text", ...props }, _ref) => {
    const [isOpen, setIsOpen] = useState(false);
    //console.log(date != undefined ? DateFormatter.FormatDateForCalendar(date).split(" ")[1] : "");
    const [selectedDate, setSelectedDate] = useState<Date | null>(date ?? null);
    //const [selectedTime, setSelectedTime] = useState<string>("00:00");
    const hour =
      date != undefined
        ? DateFormatter.FormatDateForCalendar(date).split(" ")[1].split(":")[0]
        : "00";
    const minute =
      date != undefined
        ? DateFormatter.FormatDateForCalendar(date).split(" ")[1].split(":")[1]
        : "00";
    const [selectedHour, setSelectedHour] = useState<string>(hour);
    const [selectedMinute, setSelectedMinute] = useState<string>(minute);
    const [currentMonth, setCurrentMonth] = useState(new Date());

    const { setValue, register } = useFormContext();

    // useEffect(() => {
    //   if (selectedDate !== null) setValue(name, selectedDate.toLocaleDateString());
    // }, [selectedDate]);
    useEffect(() => {
      if (selectedDate !== null) {
        const dateString = selectedDate.toLocaleDateString();
        setValue(name, isTime ? `${dateString} ${selectedHour}:${selectedMinute}` : dateString);
      }
    }, [selectedDate, selectedHour, selectedMinute, isTime]);

    const days = ["pon", "wto", "śro", "czw", "pią", "sob", "nie"];
    const daysInMonth = (year: number, month: number) => new Date(year, month + 1, 0).getDate();

    const firstDayOfMonth = new Date(
      currentMonth.getFullYear(),
      currentMonth.getMonth(),
      1
    ).getDay();
    const totalDays = daysInMonth(currentMonth.getFullYear(), currentMonth.getMonth());

    const addCount = firstDayOfMonth != 0 ? firstDayOfMonth - 1 : 6;

    const gridCells = [
      ...Array(addCount).fill(null),
      ...Array.from({ length: totalDays }, (_, i) => i + 1),
    ];

    while (gridCells.length % 7 !== 0) {
      gridCells.push(null);
    }

    const handleDateClick = (day: number) => {
      // const newDate = new Date(currentMonth.getFullYear(), currentMonth.getMonth(), day);
      const newDate = new Date(currentMonth.getFullYear(), currentMonth.getMonth(), day);

      if (isTime) {
        const hours = Number(selectedHour);
        const minutes = Number(selectedMinute);
        newDate.setHours(hours, minutes);
      }

      setSelectedDate(newDate);
    };

    const handleChange = (offset: number, monthOrYear: string) => {
      let newMonth: Date | undefined = undefined;
      switch (monthOrYear) {
        case "month":
          newMonth = new Date(currentMonth.getFullYear(), currentMonth.getMonth() + offset);
          break;
        case "year":
          newMonth = new Date(currentMonth.getFullYear() + offset, currentMonth.getMonth());
          break;
      }
      // if (newMonth !== undefined) {
      //   setCurrentMonth(newMonth);
      //   const day = selectedDate?.getDate();
      //   const newDate = new Date(newMonth.getFullYear(), newMonth.getMonth(), day);

      //   if (!isNaN(newDate.getTime())) {
      //     setSelectedDate(newDate);
      //   }
      // }
      if (newMonth !== undefined) {
        setCurrentMonth(newMonth);

        const day = selectedDate?.getDate();
        const newDate = new Date(newMonth.getFullYear(), newMonth.getMonth(), day ?? 1); // Default to 1st if no selected day

        // If `isTime` is true, add the time to the new date
        if (isTime && selectedHour && selectedMinute) {
          const hours = Number(selectedHour);
          const minutes = Number(selectedMinute);
          newDate.setHours(hours, minutes);
        }

        // Only set the date if it is valid
        if (!isNaN(newDate.getTime())) {
          setSelectedDate(newDate);
        }
      }
    };

    return (
      <div className="w-full">
        <div
          className={`w-full bg-[#ECECEC] rounded-md px-3 flex flex-row justify-start items-center gap-3 ${
            isOpen ? "outline outline-primaryPurple outline-2" : "outline-none"
          }`}
          onClick={() => setIsOpen(true)}
        >
          <FontAwesomeIcon
            icon={faCalendar}
            style={{ color: `black`, width: "24px", height: "24px" }}
          />
          <div className="w-full relative pt-6 pb-2 ">
            <label
              htmlFor={name}
              className="absolute transition-all text-[#2F2F2F] duration-300 -translate-y-4 text-[12px]"
            >
              {label}
            </label>
            <input
              {...register(name)}
              //ref={ref}
              name={name}
              id={name}
              type="text"
              className="w-full text-[#2F2F2F] font-semibold bg-[#ECECEC] cursor-pointer"
              style={{ outline: "none" }}
              {...props}
              value={
                selectedDate
                  ? isTime
                    ? `${selectedDate.toLocaleDateString()} ${selectedHour}:${selectedMinute}`
                    : selectedDate.toLocaleDateString()
                  : ""
              }
              //value={selectedDate ? selectedDate.toLocaleDateString() : ""}
              readOnly
            />
            {isOpen && (
              <div
                className="absolute top-[102%] -left-12 bg-white p-3 drop-shadow-xl z-10 flex flex-row gap-8"
                onBlur={() => setIsOpen(false)}
                tabIndex={0}
              >
                <div onBlur={(e) => e.stopPropagation()}>
                  <div className="flex flex-row items-center justify-between mb-3">
                    <div
                      className="bg-primaryPurple text-white px-[22px] py-[10px] text-base rounded-md text-center cursor-pointer"
                      onClick={() => handleChange(-1, "month")}
                    >
                      {"<"}
                    </div>
                    <div className="flex flex-row items-center justify-center gap-1 font-semibold">
                      <p>{currentMonth.toLocaleString("default", { month: "long" })}</p>
                      <p>{currentMonth.getFullYear()}</p>
                      <div className="flex flex-col justify-center items-center gap-[2px] pl-2">
                        <div
                          className="bg-primaryPurple text-white px-[7px] py-[4px] text-[13px] rounded-md text-center cursor-pointer"
                          onClick={() => handleChange(1, "year")}
                        >
                          {"↑"}
                        </div>
                        <div
                          className="bg-primaryPurple text-white px-[7px] py-[4px] text-[13px] rounded-md text-center cursor-pointer"
                          onClick={() => handleChange(-1, "year")}
                        >
                          {"↓"}
                        </div>
                      </div>
                    </div>
                    <div
                      className="bg-primaryPurple text-white px-[22px] py-[10px] text-base rounded-md text-center cursor-pointer"
                      onClick={() => handleChange(1, "month")}
                    >
                      {">"}
                    </div>
                  </div>
                  <div className="grid gap-[5px]" style={{ gridTemplateColumns: "repeat(7, 1fr)" }}>
                    {days.map((day, index) => (
                      <div className="text-center" key={index}>
                        {day}
                      </div>
                    ))}
                    {gridCells.map((day, index) => (
                      <div
                        key={index}
                        className="p-[10px] border-[1px] border-[#ddd] text-center"
                        style={{
                          cursor: day ? "pointer" : "default",
                          backgroundColor: day
                            ? selectedDate?.getDate() === day
                              ? "#7B2CBF"
                              : "white"
                            : "#efefef",
                          color: day && selectedDate?.getDate() === day ? "white" : "black",
                        }}
                        onClick={day ? () => handleDateClick(day) : undefined}
                      >
                        {day ? day : ""}
                      </div>
                    ))}
                  </div>
                </div>
                {isTime && (
                  <div className="flex flex-col justify-center items-center gap-2 pt-10">
                    <p className="font-semibold">Czas:</p>
                    <div
                      className="flex flex-row items-start gap-2"
                      onBlur={(e) => e.stopPropagation()}
                    >
                      <div
                        className="grid grid-cols-1 gap-3 overflow-y-auto max-h-[280px] hide-scrollbar"
                        style={{ gridTemplateColumns: "repeat(1, 1fr)" }}
                      >
                        {Array.from({ length: 24 }, (_, hour) => (
                          <div
                            className="p-[10px] border-[#ddd] border-[1px] text-center hover:cursor-pointer"
                            style={{
                              backgroundColor:
                                selectedHour === `${hour.toString().padStart(2, "0")}`
                                  ? "#7B2CBF"
                                  : "white",
                              color:
                                selectedHour === `${hour.toString().padStart(2, "0")}`
                                  ? "white"
                                  : "black",
                            }}
                            onClick={() => setSelectedHour(`${hour.toString().padStart(2, "0")}`)}
                            key={hour}
                          >{`${hour.toString().padStart(2, "0")}`}</div>
                        ))}
                      </div>
                      <div
                        className="grid grid-cols-1 gap-3 overflow-y-auto max-h-[280px] hide-scrollbar"
                        style={{ gridTemplateColumns: "repeat(1, 1fr)" }}
                      >
                        {Array.from({ length: 60 }, (_, min) => (
                          <div
                            className="p-[10px] border-[#ddd] border-[1px] text-center hover:cursor-pointer"
                            key={min}
                            style={{
                              backgroundColor:
                                selectedMinute === `${min.toString().padStart(2, "0")}`
                                  ? "#7B2CBF"
                                  : "white",
                              color:
                                selectedMinute === `${min.toString().padStart(2, "0")}`
                                  ? "white"
                                  : "black",
                            }}
                            onClick={() => setSelectedMinute(`${min.toString().padStart(2, "0")}`)}
                          >{`${min.toString().padStart(2, "0")}`}</div>
                        ))}
                      </div>
                    </div>
                  </div>
                )}
              </div>
            )}
          </div>
        </div>
        <div style={{ height: errorHeight !== undefined ? `${errorHeight}px` : "auto" }}>
          {error && <div className="text-red-500 text-[14.5px]">{error.message}</div>}
        </div>
      </div>
    );
  }
);

export default DatePicker;
