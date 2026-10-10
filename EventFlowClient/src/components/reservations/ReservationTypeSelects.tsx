import { FieldErrors } from "react-hook-form";
import Select from "../common/forms/Select";
import { SelectOption } from "../../helpers/SelectOptions";
import { ReservationRequest } from "../../models/create_schemas/ReservationSchema";

interface ReservationTypeSelectsProps {
  paymentTypeSelectOptions: SelectOption[];
  ticketTypeSelectOptions: SelectOption[];
  errors: FieldErrors<ReservationRequest>;
}

const ReservationTypeSelects = ({
  paymentTypeSelectOptions,
  ticketTypeSelectOptions,
  errors,
}: ReservationTypeSelectsProps) => {
  return (
    <article className="self-start pt-2 flex flex-col justify-center items-center gap-2 mt-2 w-full">
      <h5 className="font-semibold text-base w-full text-center">
        Wybierz typ płatności i typ biletu:
      </h5>
      <Select
        label="Typ płatności"
        name="paymentTypeId"
        selectedOption={paymentTypeSelectOptions[0]}
        optionValues={paymentTypeSelectOptions}
        error={errors.paymentTypeId}
        errorHeight={15}
      />
      <Select
        label="Typ biletu"
        name="ticketId"
        selectedOption={ticketTypeSelectOptions[0]}
        optionValues={ticketTypeSelectOptions}
        error={errors.ticketId}
        errorHeight={15}
      />
    </article>
  );
};

export default ReservationTypeSelects;
