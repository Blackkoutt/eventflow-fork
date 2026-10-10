import { Seat } from "../../models/response_models";
import { ToRoman } from "../../helpers/NumberConverter";
import SeatsGridCell from "./SeatsGridCell";

interface SeatsGridProps {
  maxNumberOfSeatsRows: number;
  maxNumberOfSeatsColumns: number;
  seats: Seat[];
  reservedSeatNumbers: number[];
  selectedSeatNumbers: number[];
  onSeatClick: (seatNr: number) => void;
}

const SeatsGrid = ({
  maxNumberOfSeatsRows,
  maxNumberOfSeatsColumns,
  seats,
  reservedSeatNumbers,
  selectedSeatNumbers,
  onSeatClick,
}: SeatsGridProps) => {
  const seatNumbers = Array.from(
    { length: maxNumberOfSeatsRows * maxNumberOfSeatsColumns },
    (_, index) => index + 1
  );

  return (
    <div className="flex flex-col gap-[10px]">
      <div className="flex flex-row justify-center items-center gap-2">
        <div className="flex flex-col justify-center items-center gap-[24.5px] pt-[28px]">
          {Array.from({ length: maxNumberOfSeatsRows }, (_, rowIndex) => (
            <div key={rowIndex} className="text-[14px] text-textPrimary">
              {ToRoman(rowIndex + 1)}
            </div>
          ))}
        </div>
        <div className="flex flex-col justify-start items-start gap-2">
          <div
            className="grid gap-[10px]"
            style={{
              gridTemplateColumns: `repeat(${maxNumberOfSeatsColumns}, 37px)`,
            }}
          >
            {Array.from({ length: maxNumberOfSeatsColumns }, (_, colIndex) => (
              <div key={colIndex} className="text-[14px] text-textPrimary text-center">
                {ToRoman(colIndex + 1)}
              </div>
            ))}
          </div>
          <div
            className="grid gap-[10px]"
            style={{
              gridTemplateColumns: `repeat(${maxNumberOfSeatsColumns}, minmax(0, 1fr))`,
            }}
          >
            {seatNumbers.map((seatNumber) => (
              <SeatsGridCell
                key={seatNumber}
                seatNumber={seatNumber}
                seat={seats.find((s) => s.seatNr == seatNumber)}
                isReserved={reservedSeatNumbers.includes(seatNumber)}
                isSelected={selectedSeatNumbers.includes(seatNumber)}
                onClick={() => onSeatClick(seatNumber)}
              />
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default SeatsGrid;
