import { ReactNode } from "react";
import LabelText from "../common/LabelText";

interface AdditionalPayment {
  percentage: number;
  cost: number;
}

interface ReservationSummaryProps {
  ticketPrice: number;
  totalCost: number;
  additionalPayment?: AdditionalPayment;
  children: ReactNode;
}

const ReservationSummary = ({
  ticketPrice,
  totalCost,
  additionalPayment,
  children,
}: ReservationSummaryProps) => {
  return (
    <article className="self-start flex flex-col justify-center items-center gap-2 mt-2 w-full">
      <h5 className="font-semibold text-base w-full text-center">Podsumowanie:</h5>
      {children}
      <LabelText label="Cena biletu:" text={`${ticketPrice} zł`} />
      {additionalPayment && (
        <LabelText
          label={`Dodatkowe opłaty (+${additionalPayment.percentage}%):`}
          text={`${additionalPayment.cost} zł`}
        />
      )}
      <LabelText label="Całkowity koszt:" text={`${totalCost} zł`} />
    </article>
  );
};

export default ReservationSummary;
