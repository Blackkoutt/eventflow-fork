import { forwardRef, useEffect, useState } from "react";
import { Hall } from "../../../models/response_models";
import Dialog from "../../common/Dialog";
import LabelText from "../../common/LabelText";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import useApi from "../../../hooks/useApi";

interface DetailsHallDialogProps {
  hall?: Hall;
}

const DetailsHallDialog = forwardRef<HTMLDialogElement, DetailsHallDialogProps>(
  ({ hall }: DetailsHallDialogProps, ref) => {
    const { data: item, get: getItem } = useApi<Hall>(ApiEndpoint.Hall);
    const [hallWithDetails, setHallWithDetails] = useState<Hall | undefined>(undefined);

    useEffect(() => {
      getItem({ id: hall?.id, queryParams: undefined });
    }, [hall]);

    useEffect(() => {
      if (item && item.length > 0) {
        setHallWithDetails(item[0]);
      }
    }, [item]);

    return (
      <div>
        {hall && hallWithDetails != undefined && (
          <Dialog ref={ref}>
            <article className="flex flex-col justify-center items-center px-5 pb-2 gap-5">
              <div className="flex flex-col justify-center items-center gap-2">
                <h2>Szczegóły sali</h2>
                <p className="text-textPrimary text-base text-center">
                  Poniżej przedstawiono szczegóły dotyczące wybranej sali
                </p>
              </div>
              <div className="flex flex-col justify-center items-center gap-2">
                <div className="flex flex-row justify-center items-center gap-5">
                  <div className="flex flex-col justify-center items-center gap-2">
                    <LabelText labelWidth={162} label="ID:" text={hallWithDetails.id} gap={10} />
                    <LabelText
                      labelWidth={162}
                      label="Nr sali:"
                      text={hallWithDetails.hallNr}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={162}
                      label="Piętro:"
                      text={hallWithDetails.floor}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={162}
                      label="Cena za 1h:"
                      text={`${hallWithDetails.rentalPricePerHour} zł`}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={162}
                      label="Ilość miejsc:"
                      text={hallWithDetails.seatsCount}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={162}
                      label="Max ilość miejsc:"
                      text={hallWithDetails.maxNumberOfSeats}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={162}
                      label="Typ:"
                      text={hallWithDetails.type?.name}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={162}
                      label="Wyposażenie:"
                      text={hallWithDetails.type?.equipments.map((eq) => eq.name).join(", ")}
                      gap={10}
                    />
                  </div>
                  <div className="flex flex-col justify-center items-center gap-2">
                    <LabelText
                      labelWidth={162}
                      label="Powierzchnia:"
                      text={`${hallWithDetails.totalArea} m2`}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={162}
                      label="Długość:"
                      text={`${hallWithDetails.totalLength} m`}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={162}
                      label="Szerokość:"
                      text={`${hallWithDetails.totalWidth} m`}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={162}
                      label="Powierzchnia sceny:"
                      text={`${hallWithDetails.stageArea ?? 0} m2`}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={162}
                      label="Max ilość rzędów:"
                      text={hallWithDetails.maxNumberOfSeatsRows}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={162}
                      label="Max ilość kolumn:"
                      text={hallWithDetails.maxNumberOfSeatsColumns}
                      gap={10}
                    />
                  </div>
                </div>
              </div>
            </article>
          </Dialog>
        )}
      </div>
    );
  }
);
export default DetailsHallDialog;
