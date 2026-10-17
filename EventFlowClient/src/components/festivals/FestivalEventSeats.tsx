import { Hall } from "../../models/response_models";
import SeatsGrid from "../reservations/SeatsGrid";

interface FestivalEventSeatsProps {
  hallReservation?: { hall: Hall; seatNr: number[] };
  selectedSeatNumber?: number;
  onSeatClick: (seatNr: number) => void;
}

const FestivalEventSeats = ({
  hallReservation,
  selectedSeatNumber,
  onSeatClick,
}: FestivalEventSeatsProps) => {
  const selectedHall = hallReservation?.hall;

  return (
    <>
      {!hallReservation && "Brak danych o sali"}
      {hallReservation &&
        selectedHall?.maxNumberOfSeatsRows !== undefined &&
        selectedHall?.maxNumberOfSeatsColumns !== undefined && (
          <SeatsGrid
            maxNumberOfSeatsRows={selectedHall.maxNumberOfSeatsRows}
            maxNumberOfSeatsColumns={selectedHall.maxNumberOfSeatsColumns}
            seats={hallReservation.hall.seats}
            reservedSeatNumbers={hallReservation.seatNr}
            selectedSeatNumbers={selectedSeatNumber === undefined ? [] : [selectedSeatNumber]}
            onSeatClick={onSeatClick}
          />
        )}
    </>
  );
};

export default FestivalEventSeats;
