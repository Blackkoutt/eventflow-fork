import { useParams } from "react-router-dom";
import { Festival } from "../models/response_models";
import useApi from "../hooks/useApi";
import { ApiEndpoint } from "../helpers/enums/ApiEndpointEnum";
import { useEffect, useRef, useState } from "react";
import ApiClient from "../services/api/ApiClientService";
import DateFormatter from "../helpers/DateFormatter";
import { DateFormat } from "../helpers/enums/DateFormatEnum";
import Button, { ButtonStyle } from "../components/buttons/Button";
import FestivalIcon from "../assets/festival_icon.png";
import { faShoppingCart } from "@fortawesome/free-solid-svg-icons";
import FestivalReservationDialog from "../components/festivals/FestivalReservationDialog";

const FestivalPage = () => {
  const { data: items, get: getItems } = useApi<Festival>(ApiEndpoint.Festival);
  const [festival, setFestival] = useState<Festival>();
  const reservationDialog = useRef<HTMLDialogElement>(null);
  const { festivalId } = useParams();

  useEffect(() => {
    getItems({ id: festivalId, queryParams: undefined });
  }, [festivalId]);

  useEffect(() => {
    if (items && items.length > 0) {
      setFestival(items[0]);
    }
  }, [items]);

  return (
    festival && (
      <div className="flex flex-row w-[80%] justify-start items-start my-10 rounded-md shadow-xl overflow-hidden">
        <FestivalReservationDialog
          ref={reservationDialog}
          festival={festival}
          onDialogClose={() => reservationDialog.current?.close()}
          onDialogConfirm={() => reservationDialog.current?.close()}
        />
        <img
          className="object-cover min-w-[42%] max-w-[42%] min-h-[550px]"
          src={ApiClient.GetPhotoEndpoint(festival.photoEndpoint)}
          alt={`Zdjęcie festiwalu ${festival.title}`}
        />
        <div className="px-8 flex flex-col justify-center items-start gap-3 py-8 w-full">
          <div className="flex flex-col justify-center items-start gap-4">
            <div className="flex flex-col justify-center items-start gap-[10px]">
              <div className="flex flex-row justify-center items-center gap-2">
                <img src={FestivalIcon} alt="Ikona festivalu" />
                <p className="text-[20px] text-[#00BFC3] font-semibold m-0">FESTIWAL</p>
              </div>
              <h3 className="text-[40px] font-bold header_text text-[#4C4C4C]">{festival.title}</h3>
            </div>
            <p className="text-2xl font-semibold text-textPrimary m-0 p-0">Program festiwalu:</p>
            <div className="flex flex-col justify-center items-start">
              {festival.events.map((event) => {
                return (
                  <div key={event.id} className="flex flex-row justify-center items-start gap-1">
                    <p className="pb-2 text-[18px] font-semibold text-[#00BFC3] w-[170px]">
                      {DateFormatter.FormatDate(event.start, DateFormat.DateTime)}
                    </p>
                    <p className="text-textPrimary">
                      {event.category?.name}: {event.title},
                    </p>
                    <p className="text-textPrimary">sala: nr {event.hall?.hallNr}</p>
                  </div>
                );
              })}
            </div>
            <div className="self-center -translate-y-1">
              <Button
                text="Kup bilet"
                icon={faShoppingCart}
                width={130}
                height={43}
                fontSize={17}
                isFontSemibold={true}
                style={ButtonStyle.Default}
                action={() => reservationDialog.current?.showModal()}
              />
            </div>
          </div>
          <div className="w-full h-[1px] bg-textPrimary"></div>
          <div className="flex flex-col justify-start items-start">
            <p className="text-textPrimary">{festival.details?.longDescription}</p>
          </div>
          <div className="w-full h-[0.5px] bg-textPrimary"></div>
          <div className="flex flex-row justify-start items-center flex-wrap gap-3">
            <p className="text-[15px] font-semibold text-textPrimary m-0 p-0 min-w-[130px]">
              Organizatorzy:
            </p>
            {festival.organizers.map((o) => {
              return (
                <img
                  key={o.id}
                  title={o.name}
                  className="object-contain max-w-[120px] h-[55px] hover:cursor-pointer"
                  src={ApiClient.GetPhotoEndpoint(o.photoEndpoint)}
                  alt={`Zdjęcie organizatora ${o.name}`}
                />
              );
            })}
          </div>
          <div className="flex flex-row justify-start items-center flex-wrap gap-3">
            <p className="text-[15px] font-semibold text-textPrimary m-0 p-0 min-w-[130px]">
              Patroni medialni:
            </p>
            {festival.mediaPatrons.map((mp) => {
              return (
                <img
                  key={mp.id}
                  title={mp.name}
                  className="object-contain max-w-[120px] h-[50px] hover:cursor-pointer"
                  src={ApiClient.GetPhotoEndpoint(mp.photoEndpoint)}
                  alt={`Zdjęcie patrona medialnego ${mp.name}`}
                />
              );
            })}
          </div>
          <div className="flex flex-row justify-start items-center flex-wrap gap-3">
            <p className="text-[15px] font-semibold text-textPrimary m-0 p-0 min-w-[130px]">
              Sponsorzy:
            </p>
            {festival.sponsors.map((s) => {
              return (
                <img
                  key={s.id}
                  title={s.name}
                  className="object-contain max-w-[120px] h-[55px] hover:cursor-pointer"
                  src={ApiClient.GetPhotoEndpoint(s.photoEndpoint)}
                  alt={`Zdjęcie sponsora ${s.name}`}
                />
              );
            })}
          </div>
        </div>
      </div>
    )
  );
};
export default FestivalPage;
