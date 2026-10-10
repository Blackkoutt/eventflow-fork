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
  const hallDetails = hallReservation?.hall.hallDetails;

  return (
    <>
      {!hallReservation && "Brak danych o sali"}
      {hallReservation &&
        hallDetails?.maxNumberOfSeatsRows !== undefined &&
        hallDetails?.maxNumberOfSeatsColumns !== undefined && (
          <SeatsGrid
            maxNumberOfSeatsRows={hallDetails.maxNumberOfSeatsRows}
            maxNumberOfSeatsColumns={hallDetails.maxNumberOfSeatsColumns}
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
