import { useMemo } from "react";
import { EventPass, PaymentType } from "../models/response_models";
import { SelectOption } from "../helpers/SelectOptions";

const usePaymentTypeSelectOptions = (paymentTypes: PaymentType[], eventPasses: EventPass[]) => {
  return useMemo(() => {
    const hasEventPass = eventPasses.length > 0;
    return paymentTypes
      .filter((pt) => hasEventPass || pt.name != "Karnet")
      .map((pt) => ({ value: pt.id, option: pt.name } as SelectOption));
  }, [paymentTypes, eventPasses]);
};

export default usePaymentTypeSelectOptions;
