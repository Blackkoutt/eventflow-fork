import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCouch } from "@fortawesome/free-solid-svg-icons";
import { Seat } from "../../models/response_models";

interface SeatsGridCellProps {
  seatNumber: number;
  seat?: Seat;
  isReserved: boolean;
  isSelected: boolean;
  onClick: () => void;
}

const SeatsGridCell = ({ seatNumber, seat, isReserved, isSelected, onClick }: SeatsGridCellProps) => {
  const seatColor = seat?.seatType?.seatColor;
  let textColor = seatColor;
  if (isSelected) {
    textColor = "white";
  } else if (isReserved) {
    textColor = "black";
  }

  let stateClassName = "border-gray-400 bg-gray-200 text-gray-500";
  if (seat && isReserved) {
    stateClassName = "border-red-600 text-black bg-red-600";
  } else if (seat) {
    stateClassName = "border-gray-300 text-gray-500 hover:cursor-pointer";
  }

  return (
    <div
      className={`border px-[8px] py-[4px] text-center text-[12px] flex flex-col justify-center gap-[2px] items-center ${stateClassName}`}
      style={{
        borderColor: isReserved ? undefined : seatColor,
        color: textColor,
        backgroundColor: isSelected ? seatColor : undefined,
      }}
      onClick={() => {
        if (seat && !isReserved) {
          onClick();
        }
      }}
    >
      <FontAwesomeIcon icon={faCouch} fontSize={12} />
      <p className="hover:cursor-default text-[11px] font-semibold select-none">{seatNumber}</p>
    </div>
  );
};

export default SeatsGridCell;
