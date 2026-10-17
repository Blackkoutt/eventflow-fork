import { useContext, useEffect, useState } from "react";
import { format, addDays, subDays } from "date-fns";
import { pl } from "date-fns/locale"; // Importujemy lokalizację polską
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faAnglesLeft,
  faAnglesRight,
  faArrowLeft,
  faArrowRight,
} from "@fortawesome/free-solid-svg-icons";
import { SelectedEventDateContext } from "../../context/SelectedEventDateContext";

const EventCalendar = () => {
  const [currentDate, setCurrentDate] = useState<Date>(new Date());
  const [dates, setDates] = useState<Date[]>([]);

  const context = useContext(SelectedEventDateContext);

  if (context === undefined) {
    throw new Error("SelectedEventDateContext must be used within an SelectedEventDateProvider");
  }

  const { selectedDate, setSelectedDate } = context;

  // Funkcja generująca 14 dni od aktualnej daty
  const generateDates = () => {
    // const startOfPeriod = startOfWeek(currentDate, { weekStartsOn: 1 }); // Rozpoczynamy od poniedziałku
    // return Array.from({ length: 14 }, (_, i) => addDays(startOfPeriod, i));
    return Array.from({ length: 14 }, (_, i) => {
      const newDate = addDays(currentDate, i);
      newDate.setHours(0, 0, 0, 0); // Ustawia godzinę na 00:00:00.000
      return newDate;
    });
  };

  useEffect(() => {
    setDates(generateDates());
  }, [currentDate]);

  // Funkcja do przewijania o 1 dzień w lewo lub w prawo
  const handleScroll = (direction: "left" | "right") => {
    setCurrentDate((prevDate) =>
      direction === "left" ? subDays(prevDate, 1) : addDays(prevDate, 1)
    );
  };

  // Funkcja zmieniająca miesiąc o 1 miesiąc w lewo lub w prawo
  const changeMonth = (direction: "left" | "right") => {
    setCurrentDate((prevDate) => {
      return direction === "left" ? subDays(prevDate, 30) : addDays(prevDate, 30);
    });
  };

  // Formatowanie miesiąca (np. Styczeń 2025)
  const formattedMonth = format(currentDate, "LLLL yyyy", { locale: pl });

  return (
    <div className="w-full flex flex-col gap-6">
      {/* Event Types */}

      {/* Month Navigation */}
      <div className="flex flex-row gap-6 items-center px-4 py-1 bg-gray-200 max-w-[318px]">
        <button
          className="text-lg font-bold text-gray-600 hover:text-primaryPurple "
          onClick={() => changeMonth("left")}
        >
          <FontAwesomeIcon icon={faAnglesLeft} />
        </button>
        <div className="text-lg font-semibold min-w-[200px] text-center">{formattedMonth}</div>
        <button
          className="text-lg font-bold text-gray-600 hover:text-primaryPurple"
          onClick={() => changeMonth("right")}
        >
          <FontAwesomeIcon icon={faAnglesRight} />
        </button>
      </div>

      {/* Calendar */}
      <div className="relative flex items-center justify-center flex-row gap-4">
        {/* Left Scroll Button */}
        <button
          className=" bg-gray-200 rounded-full px-4 py-3 shadow-md hover:bg-gray-300 focus:outline-none"
          onClick={() => handleScroll("left")}
        >
          <FontAwesomeIcon icon={faArrowLeft} />
        </button>

        {/* Dates */}
        <div className="flex gap-3 overflow-x-auto scrollbar-hide px-8">
          {dates.map((date) => (
            <div
              key={date.toISOString()}
              className={`text-center w-[80px] py-3 hover:cursor-pointer ${
                new Date(selectedDate.setHours(0, 0, 0, 0)).getTime() === date.getTime()
                  ? "bg-primaryPurple text-white"
                  : "bg-gray-200"
              } `}
              onClick={() => {
                const dateWithZeroTime = new Date(date);
                dateWithZeroTime.setHours(0, 0, 0, 0); // Ustawiamy godzinę na 00:00
                setSelectedDate(dateWithZeroTime);
              }}
            >
              <div
                className={`text-sm ${
                  selectedDate.getTime() === date.getTime() ? "text-white" : "text-gray-500"
                }`}
              >
                {format(date, "EEE", { locale: pl }).toUpperCase()} {/* Day of the week */}
              </div>
              <div className="text-lg font-bold">{format(date, "dd")}</div> {/* Day of the month */}
            </div>
          ))}
        </div>

        {/* Right Scroll Button */}
        <button
          className=" bg-gray-200 rounded-full px-4 py-3 shadow-md hover:bg-gray-300 focus:outline-none"
          onClick={() => handleScroll("right")}
        >
          <FontAwesomeIcon icon={faArrowRight} />
        </button>
      </div>
    </div>
  );
};

export default EventCalendar;
