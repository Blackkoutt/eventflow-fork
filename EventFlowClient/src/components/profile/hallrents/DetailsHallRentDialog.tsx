import { forwardRef, useEffect } from "react";
import { AdditionalServices, Hall, HallRent } from "../../../models/response_models";
import Dialog from "../../common/Dialog";
import LabelText from "../../common/LabelText";
import DateFormatter from "../../../helpers/DateFormatter";
import { DateFormat } from "../../../helpers/enums/DateFormatEnum";
import { getStatus } from "../../../helpers/GetStatus";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import useApi from "../../../hooks/useApi";

interface DetailsHallRentDialogProps {
  hallRent?: HallRent;
  isAdminDetails?: boolean;
}

const DetailsHallRentDialog = forwardRef<HTMLDialogElement, DetailsHallRentDialogProps>(
  ({ hallRent, isAdminDetails = false }: DetailsHallRentDialogProps, ref) => {
    const { data: hallWidthDetails, get: getHallWithDetails } = useApi<Hall>(ApiEndpoint.Hall);

    useEffect(() => {
      getHallWithDetails({ id: hallRent?.hall?.id, queryParams: undefined });
    }, [hallRent]);
    //console.log(hallWidthDetails);

    return (
      <div>
        {hallRent && hallWidthDetails.length !== 0 && (
          <Dialog ref={ref}>
            <article className="flex flex-col justify-center items-center px-5 pb-2 gap-5">
              <div className="flex flex-col justify-center items-center gap-2">
                <h2>Szczegóły rezerwacji sali</h2>
                <p className="text-textPrimary text-base text-center">
                  Poniżej przedstawiono szczegóły dotyczące wybranej rezerwacji sali
                </p>
              </div>
              <div className="flex flex-col justify-center items-center gap-2">
                <div className="flex flex-row justify-center items-center gap-12">
                  <div className="flex flex-col justify-center items-center gap-2">
                    <LabelText labelWidth={150} label="ID:" text={hallRent.id} gap={10} />
                    <LabelText
                      labelWidth={150}
                      label="GUID:"
                      title={hallRent.hallRentGuid}
                      text={
                        hallRent.hallRentGuid?.length > 10
                          ? `${hallRent.hallRentGuid.slice(0, 10)}...`
                          : hallRent.hallRentGuid
                      }
                      gap={10}
                    />
                    {isAdminDetails && (
                      <>
                        <LabelText
                          label="Użytkownik:"
                          labelWidth={150}
                          text={`${hallRent.user?.name} ${hallRent.user?.surname}`}
                          gap={10}
                        />
                        <LabelText
                          label="E-mail:"
                          labelWidth={150}
                          text={hallRent.user?.emailAddress}
                          gap={10}
                        />
                      </>
                    )}
                    <LabelText
                      labelWidth={150}
                      label="Status:"
                      text={getStatus(hallRent.hallRentStatus)}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={150}
                      textWidth={145}
                      label="Data rozpoczęcia:"
                      text={DateFormatter.FormatDate(hallRent.startDate, DateFormat.DateTime)}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={150}
                      textWidth={145}
                      label="Data zakończenia:"
                      text={DateFormatter.FormatDate(hallRent.endDate, DateFormat.DateTime)}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={150}
                      textWidth={145}
                      label="Data płatności:"
                      text={DateFormatter.FormatDate(hallRent.paymentDate, DateFormat.DateTime)}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={150}
                      label="Typ płatności:"
                      text={hallRent.paymentType?.name}
                      gap={10}
                    />
                  </div>
                  <div className="flex flex-col justify-center items-center gap-2">
                    <LabelText
                      labelWidth={185}
                      label="Nr sali:"
                      text={hallRent.hall?.hallNr}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={185}
                      label="Piętro:"
                      text={hallRent.hall?.floor}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={185}
                      label="Typ sali:"
                      text={hallRent.hall?.type?.name}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={185}
                      label="Ilość miejsc:"
                      text={hallWidthDetails[0].seats.length}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={185}
                      label="Powierzchnia (m2):"
                      text={hallWidthDetails[0].totalArea}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={185}
                      label="Cena za 1 h:"
                      text={`${hallRent.hall?.rentalPricePerHour} zł`}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={185}
                      label="Cena całkowita:"
                      text={`${hallRent.hall?.rentalPricePerHour} zł`}
                      gap={10}
                    />
                  </div>
                </div>
                <LabelText
                  label="Dodatkowe usługi:"
                  labelWidth={150}
                  text={hallRent.additionalServices
                    .map((service: AdditionalServices) => `${service.name} (${service.price} zł)`)
                    .join(", ")}
                  gap={10}
                />
              </div>
            </article>
          </Dialog>
        )}
      </div>
    );
  }
);
export default DetailsHallRentDialog;
