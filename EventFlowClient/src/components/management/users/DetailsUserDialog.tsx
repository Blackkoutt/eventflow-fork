import { forwardRef, useEffect, useState } from "react";
import { User } from "../../../models/response_models";
import Dialog from "../../common/Dialog";
import LabelText from "../../common/LabelText";
import ApiClient from "../../../services/api/ApiClientService";
import DateFormatter from "../../../helpers/DateFormatter";
import { DateFormat } from "../../../helpers/enums/DateFormatEnum";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import Status from "../../common/Status";

interface DetailsUserDialogProps {
  item?: User;
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogClose: () => void;
}

const DetailsUserDialog = forwardRef<HTMLDialogElement, DetailsUserDialogProps>(
  ({ item, maxWidth, minWidth, onDialogClose, paddingX }: DetailsUserDialogProps, ref) => {
    console.log(item);
    const { data: items, get: getItems } = useApi<User>(ApiEndpoint.User);

    const [user, setUser] = useState<User>();

    useEffect(() => {
      if (item != undefined) {
        getItems({ id: item?.id, queryParams: undefined });
        console.log("here");
      }
    }, [item]);

    useEffect(() => {
      console.log(items);
      if (items && items.length > 0) {
        setUser(items[0]);
      }
    }, [items]);

    return (
      <div>
        {item && (
          <Dialog
            ref={ref}
            maxWidth={maxWidth}
            paddingLeft={paddingX}
            paddingRight={paddingX}
            minWidth={minWidth}
            onClose={onDialogClose}
          >
            <article className="flex flex-col justify-center items-center px-5 pb-2 gap-5">
              <div className="flex flex-col justify-center items-center gap-2">
                <h2>Szczegóły użytkownika</h2>
                <p className="text-textPrimary text-base text-center">
                  Poniżej przedstawiono szczegóły dotyczące wybranego użytkownika
                </p>
              </div>
              <div className="flex flex-col justify-center items-center gap-2">
                <div className="flex flex-row justify-center items-center gap-6">
                  <div className="flex flex-col justify-center items-center gap-2">
                    <LabelText labelWidth={160} label="ID:" text={user?.id} gap={10} />
                    <LabelText labelWidth={160} label="Imię:" text={user?.name} gap={10} />
                    <LabelText labelWidth={160} label="Nazwisko:" text={user?.surname} gap={10} />
                    <LabelText
                      labelWidth={160}
                      label="E-mail:"
                      text={user?.emailAddress}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={160}
                      label="Data urodzin:"
                      text={DateFormatter.FormatDate(user?.dateOfBirth, DateFormat.Date)}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={160}
                      label="Data rejestracji:"
                      text={DateFormatter.FormatDate(user?.registeredDate, DateFormat.Date)}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={160}
                      label="Role:"
                      text={user?.userRoles.join(", ")}
                      gap={10}
                    />
                    <div className="flex flex-row justify-start items-center self-start gap-2">
                      <p
                        style={{ fontSize: 16, minWidth: 160 }}
                        className="font-bold text-end text-textPrimary hover:cursor-default"
                      >
                        Status weryfikacji:
                      </p>
                      <Status
                        value={user?.isVerified}
                        positiveTitle="Zweryfikowany"
                        negativeTitle="Nie zweryfikowany"
                      />
                    </div>
                  </div>
                  <div className="flex flex-col justify-center items-center gap-2">
                    <LabelText
                      labelWidth={236}
                      label="Miasto:"
                      text={user?.userData?.city}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={236}
                      label="Ulica:"
                      text={user?.userData?.street}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={236}
                      label="Kod pocztowy:"
                      text={user?.userData?.zipCode}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={236}
                      label="Nr domu:"
                      text={user?.userData?.houseNumber}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={236}
                      label="Nr mieszkania:"
                      text={user?.userData?.flatNumber}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={236}
                      label="Nr telefonu:"
                      text={user?.userData?.phoneNumber}
                      gap={10}
                    />
                    <div className="flex flex-row justify-start items-center self-start gap-2">
                      <p
                        style={{ fontSize: 16, minWidth: 236 }}
                        className="font-bold text-end text-textPrimary hover:cursor-default"
                      >
                        Karnet:
                      </p>
                      <Status
                        value={user?.isActiveEventPass}
                        positiveTitle="Aktywny karnet"
                        negativeTitle="Brak aktywnego karnetu"
                      />
                    </div>

                    <LabelText
                      labelWidth={236}
                      label="Całkowita ilość rezerwacji:"
                      text={user?.allReservationsCount}
                      gap={10}
                    />
                    <LabelText
                      labelWidth={236}
                      label="Całkowita ilość rezerwacji sal:"
                      text={user?.allHallRentsCount}
                      gap={10}
                    />
                  </div>
                </div>
                <div className="flex flex-row self-start items-center gap-2">
                  <p
                    style={{ fontSize: 16, minWidth: 160 }}
                    className={`font-bold text-end text-textPrimary hover:cursor-default`}
                  >
                    Zdjęcie:
                  </p>
                  <img
                    src={`${ApiClient.GetPhotoEndpoint(user?.photoEndpoint)}`}
                    alt={`Zdjęcie użytkownika o id ${user?.id}`}
                    className="w-[100px] h-[100px] object-contain"
                  />
                </div>
              </div>
            </article>
          </Dialog>
        )}
      </div>
    );
  }
);

export default DetailsUserDialog;
