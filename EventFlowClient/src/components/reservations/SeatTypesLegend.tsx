import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCouch } from "@fortawesome/free-solid-svg-icons";
import { SeatType } from "../../models/response_models";

interface SeatTypesLegendProps {
  seatTypes: SeatType[];
}

const SeatTypesLegend = ({ seatTypes }: SeatTypesLegendProps) => {
  return (
    <article className="flex flex-col justify-start items-start gap-1 w-full">
      <h5 className="text-center font-semibold text-base w-full">Typy miejsc:</h5>
      <div className="flex justify-start flex-col gap-2">
        {seatTypes.map((type, index) => (
          <div key={index} className="flex flex-row justify-start items-center gap-2">
            <div
              className="py-[2px] px-[6px] border-[1px]"
              style={{ borderColor: type.seatColor, color: type.seatColor }}
            >
              <FontAwesomeIcon fontSize={12} icon={faCouch} />
            </div>
            <p className="text-[14px]">
              {type.name} {`(+ ${type.addtionalPaymentPercentage} % ceny biletu)`}
            </p>
          </div>
        ))}
        <div className="flex flex-row justify-start items-center gap-2">
          <div className="py-[2px] px-[6px] border-gray-400 bg-gray-200">
            <FontAwesomeIcon fontSize={12} icon={faCouch} color="#6b7280" />
          </div>
          <p className="text-[14px]">Miejsce nieaktywne</p>
        </div>
        <div className="flex flex-row justify-start items-center gap-2">
          <div className="py-[2px] px-[6px] border-red-600 bg-red-600">
            <FontAwesomeIcon fontSize={12} icon={faCouch} color="#000" />
          </div>
          <p className="text-[14px]">Miejsce zarezerwowane</p>
        </div>
      </div>
    </article>
  );
};

export default SeatTypesLegend;
