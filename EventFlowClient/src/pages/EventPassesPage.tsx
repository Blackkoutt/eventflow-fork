import pass_png from "../assets/pass_png.png";
import { EventPassType, isUserInRole } from "../models/response_models";
import { ApiEndpoint } from "../helpers/enums/ApiEndpointEnum";
import useApi from "../hooks/useApi";
import { useEffect, useRef, useState } from "react";
import SectionHeader from "../components/common/SectionHeader";
import BuyEventPassType from "../components/eventpassespage/BuyEventPassType";
import BuyEventPassDialog from "../components/eventpassespage/BuyEventPassDialog";
import { useAuth } from "../context/AuthContext";
import { Roles } from "../helpers/enums/UserRoleEnum";
import { toast } from "react-toastify";

const EventPassesPage = () => {
  const { data: items, get: getItems } = useApi<EventPassType>(ApiEndpoint.EventPassType);
  const [lowestPrice, setLowestPrice] = useState<number | undefined>(undefined);
  const { currentUser, authenticated } = useAuth();

  const buyEventPassDialog = useRef<HTMLDialogElement>(null);
  const [eventPassTypeToBuy, setEventPassTypeToBuy] = useState<EventPassType | undefined>(
    undefined
  );

  useEffect(() => {
    if (eventPassTypeToBuy != undefined) {
      buyEventPassDialog.current?.showModal();
    }
  }, [eventPassTypeToBuy]);

  useEffect(() => {
    const onInit = async () => {
      const queryParams = {
        sortBy: "Price",
        sortDirection: "ASC",
      };
      getItems({ id: undefined, queryParams: queryParams });
    };
    onInit();
  }, []);

  useEffect(() => {
    if (items && items.length > 0) {
      setLowestPrice(items[0].price);
    }
  }, [items]);

  return (
    <div className="w-full pt-10">
      <h1>Karnety</h1>
      <div className="flex flex-col justify-center items-center w-full pt-10">
        <div className="flex flex-row justify-center items-center py-12 gap-16 ">
          <div className="flex flex-col items-center">
            <img
              className="-rotate-6 object-contain w-full h-[160px]"
              src={pass_png}
              alt="Przykład karnetu"
            />
            {lowestPrice && (
              <div className="z-30 bg-gradient-to-r from-[#B35EBB] via-[#9781FD] to-[#87C3FF] w-fit p-[5px] shadow-xl -translate-y-3">
                <div className="z-30 bg-white w-fit p-[2px] ">
                  <div className="w-fit text-shadow bg-gradient-to-r rounded-md from-[#B35EBB] via-[#9781FD] to-[#87C3FF] text-white z-30 p-2 font-semibold text-[18px]">
                    JUŻ OD {lowestPrice} PLN
                  </div>
                </div>
              </div>
            )}
          </div>
          <article className="flex flex-col justify-center items-center -translate-y-6">
            <SectionHeader title="Karnet" />
            <ul className="list-disc flex flex-col items-start w-full">
              <li>Umożliwia wstęp na wszystkie wydarzenia organizowane przez EventFlow.</li>
              <li>Dostęp do rezerwacji każdego typu miejsca bez dodatkowej opłaty.</li>
              <li>Zaoszczędzasz czas i pieniądze, rezygnując z zakupu pojedynczych biletów.</li>
              <li>Różne długości karnetu dostosowane do twoich potrzeb.</li>
              <li>
                Przedłużając karnet otrzymujesz zniżkę w zależności od posiadanego typu karnetu.
              </li>
            </ul>
          </article>
        </div>
        <div className="min-w-[125%] overflow-auto bg-[#F4F6FA] flex flex-col justify-center items-center pt-10 pb-12">
          <div className="flex flex-col justify-center items-center gap-8 w-[80%]">
            <SectionHeader
              title="Nasza oferta"
              subtitle="Wybierz idealny karnet dla siebie już teraz!"
            />
            <BuyEventPassDialog
              ref={buyEventPassDialog}
              eventPassType={eventPassTypeToBuy}
              onDialogClose={() => {
                buyEventPassDialog.current?.close();
                setEventPassTypeToBuy(undefined);
              }}
            />
            <div className="grid 3xl:grid-cols-3 xl:grid-cols-2 3xl:gap-4 xl:gap-10">
              {items.map((item) => (
                <BuyEventPassType
                  key={item.id}
                  item={item}
                  onClick={() => {
                    if (!authenticated) {
                      toast.error("Musisz być zalogowany, aby kupić karnet.");
                    } else if (isUserInRole(currentUser, Roles.Admin)) {
                      toast.error("Zaloguj się jako użytkownik, aby kupić karnet.");
                    } else {
                      setEventPassTypeToBuy(item);
                      buyEventPassDialog.current?.showModal();
                    }
                  }}
                />
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
export default EventPassesPage;
