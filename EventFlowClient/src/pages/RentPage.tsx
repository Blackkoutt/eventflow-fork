import { useParams } from "react-router-dom";
import useApi from "../hooks/useApi";
import { ApiEndpoint } from "../helpers/enums/ApiEndpointEnum";
import { AdditionalServices, Hall } from "../models/response_models";
import { useEffect, useRef, useState } from "react";
import ApiClient from "../services/api/ApiClientService";
import Button, { ButtonStyle } from "../components/buttons/Button";
import {
  faBookmark,
  faCheck,
  faCouch,
  faExpand,
  faGears,
  faHandshake,
  faInfoCircle,
  faRulerHorizontal,
  faRulerVertical,
  faStairs,
} from "@fortawesome/free-solid-svg-icons";
import HallInfoElement from "../components/hallrentspage/HallInfoElement";
import HallInfoSectionHeader from "../components/hallrentspage/HallInfoSectionHeader";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import RentHallDialog from "../components/hallrentspage/RentHallDialog";
import { useAuth } from "../context/AuthContext";

const RentPage = () => {
  const { hallId } = useParams();

  const { data: halls, get: getHalls } = useApi<Hall>(ApiEndpoint.Hall);
  const { data: services, get: getServices } = useApi<AdditionalServices>(
    ApiEndpoint.AdditionalServices
  );

  const [hall, setHall] = useState<Hall>();
  //const [hallToRent, setHallToRent] = useState<HallRent | undefined>(undefined);

  const rentHallDialog = useRef<HTMLDialogElement>(null);

  useEffect(() => {
    getHalls({ id: hallId, queryParams: undefined });
    getServices({ id: undefined, queryParams: undefined });
  }, [hallId]);

  useEffect(() => {
    if (halls && halls.length > 0) setHall(halls[0]);
  }, [halls]);

  return (
    hall && (
      <div className="flex flex-col w-[70%] justify-start items-start my-10 pb-6 rounded-md shadow-xl overflow-hidden">
        <RentHallDialog
          ref={rentHallDialog}
          hall={hall}
          onDialogClose={() => rentHallDialog.current?.close()}
          onDialogConfirm={() => rentHallDialog.current?.close()}
        />
        <img
          className="object-cover w-full max-h-[300px] shadow-md"
          src={ApiClient.GetPhotoEndpoint(hall.type?.photoEndpoint)}
          alt={`Zdjęcie sali nr ${hall.hallNr}`}
        />
        <div className="relative">
          <div
            className="absolute w-[90px] h-[70px] shadow-2xl left-4 -top-12 flex flex-col justify-center items-center"
            style={{
              background: `linear-gradient(to bottom, #987EFE, #c23eb1)`,
            }}
          >
            <div className="px-2 py-2 flex flex-col justify-center items-center gap-1">
              <p className="self-start text-[20px] text-white text-shadow font-semibold">
                Nr {hall.hallNr}
              </p>
            </div>
          </div>
        </div>
        <div className="flex flex-row justify-between items-start w-full px-8 pt-4">
          <article className="flex flex-col justify-start items-center gap-8 w-full">
            <div className="flex flex-col justify-start items-start w-full pl-[100px] gap-2">
              <div className="w-full flex flex-row justify-between items-center">
                <h3 className="text-[28px] font-semibold ">{hall.type?.name}</h3>
                <div className="flex flex-row justify-center items-center gap-5">
                  <div className="flex flex-row justify-center items-center gap-2 text-[18px] ">
                    <p className="text-textPrimary">Cena za 1h:</p>
                    <p className="font-bold">{hall.rentalPricePerHour} PLN</p>
                  </div>
                  <Button
                    text="Wynajmij"
                    height={44}
                    icon={faBookmark}
                    width={140}
                    fontSize={16}
                    isFontSemibold={true}
                    style={ButtonStyle.Primary}
                    action={() => {
                      rentHallDialog.current?.showModal();
                    }}
                  />
                </div>
              </div>
            </div>
            <p className="text-textPrimary -mt-5">{hall.type?.description}</p>
            <div className="flex flex-col justify-center items-center w-full gap-4">
              <div className="w-full flex flex-col justify-center items-center gap-4">
                <HallInfoSectionHeader icon={faInfoCircle} text="Informacje o sali" />
                <div className="grid grid-cols-3 w-[85%] gap-y-6">
                  <HallInfoElement text="Piętro" icon={faStairs} value={hall.floor} />
                  <HallInfoElement text="Ilość miejsc" icon={faCouch} value={hall.seatsCount} />
                  <HallInfoElement
                    text="Max ilość miejsc"
                    icon={faCouch}
                    value={hall.maxNumberOfSeats}
                  />
                  <HallInfoElement
                    text="Długość sali"
                    icon={faRulerVertical}
                    value={hall.totalLength.toString() + " m"}
                  />
                  <HallInfoElement
                    text="Szerokość sali"
                    icon={faRulerHorizontal}
                    value={hall.totalWidth.toString() + " m"}
                  />
                  <HallInfoElement
                    text="Powierzchnia sali"
                    icon={faExpand}
                    value={hall.totalArea.toString() + " m²"}
                  />
                  {hall.stageArea && (
                    <HallInfoElement
                      text="Długość sceny"
                      icon={faRulerVertical}
                      value={hall.stageLength?.toString() + " m"}
                    />
                  )}
                  {hall.stageArea && (
                    <HallInfoElement
                      text="Szerokość sceny"
                      icon={faRulerHorizontal}
                      value={hall.stageWidth?.toString() + " m"}
                    />
                  )}
                  {hall.stageArea && (
                    <HallInfoElement
                      text="Powierzchnia sceny"
                      icon={faExpand}
                      value={hall.stageArea.toString() + " m²"}
                    />
                  )}
                </div>
              </div>
              <div className="flex flex-col justify-center items-center gap-4 pt-3">
                <HallInfoSectionHeader icon={faGears} text="Wyposażenie" />
                <ul className="list-none">
                  {hall.type?.equipments.map((eq) => (
                    <li className="hover:cursor-pointer pb-3" title={eq.description} key={eq.id}>
                      <div className="flex flex-row gap-2">
                        <FontAwesomeIcon className="text-primaryPurple" icon={faCheck} />
                        <p className="text-textPrimary">{eq.name}</p>
                      </div>
                    </li>
                  ))}
                </ul>
              </div>
              <div className="flex flex-col justify-center items-center gap-4">
                <HallInfoSectionHeader icon={faHandshake} text="Dodatkowe usługi" />
                <ul className="list-none">
                  {services.map((s) => (
                    <li
                      className="hover:cursor-pointer pb-3"
                      title={s.description ?? undefined}
                      key={s.id}
                    >
                      <div className="flex flex-row justify-between">
                        <div className="flex flex-row gap-2">
                          <FontAwesomeIcon className="text-primaryPurple" icon={faCheck} />
                          <p className="text-textPrimary">{s.name}</p>
                        </div>
                        <p className="pl-6 text-textPrimary font-semibold">{`${s.price} PLN`}</p>
                      </div>
                    </li>
                  ))}
                </ul>
              </div>
            </div>
          </article>
        </div>
      </div>
    )
  );
};
export default RentPage;
