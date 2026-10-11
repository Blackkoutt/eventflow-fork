import { Festival } from "../../models/response_models";
import ApiClient from "../../services/api/ApiClientService";
import FestivalIcon from "../../assets/festival_icon.png";
import DateFormatter from "../../helpers/DateFormatter";
import { DateFormat } from "../../helpers/enums/DateFormatEnum";
import Button, { ButtonStyle } from "../buttons/Button";
import { Link } from "react-router-dom";

interface FestivalCardProps {
  festival: Festival;
}
const FestivalCard = ({ festival }: FestivalCardProps) => {
  return (
    <Link
      to={`/festivals/${festival.id}`}
      className="shadow-xl flex flex-row justify-center items-stretch w-full min-h-[500px] overflow-hidden hover:bg-slate-50 hover:cursor-pointer"
    >
      <div className="relative w-[42%] shrink-0">
        <img
          className="absolute inset-0 w-full h-full object-cover"
          src={ApiClient.GetPhotoEndpoint(festival.photoEndpoint)}
          alt={`Zdjęcie festiwalu ${festival.title}`}
        />
      </div>
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
                    {event.category?.name}: {event.title}
                  </p>
                </div>
              );
            })}
          </div>
        </div>
        <div className="w-full h-[1px] bg-black"></div>
        <div className="flex flex-col justify-start items-start">
          <p className="text-textPrimary">{festival.shortDescription}</p>
          {/* link to festival id*/}
          <p className="text-[#987EFE] text-[14px] pt-1">Czytaj dalej &rarr;</p>
        </div>
        <div className="self-center pt-3">
          <Button
            text="Kup bilet"
            width={130}
            height={45}
            fontSize={18}
            isFontSemibold={true}
            style={ButtonStyle.Default}
            action={() => {}}
          />
        </div>
      </div>
    </Link>
  );
};

export default FestivalCard;
