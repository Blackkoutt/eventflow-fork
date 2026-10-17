import { Link } from "react-router-dom";
import { Hall } from "../../models/response_models";
import ApiClient from "../../services/api/ApiClientService";
import Button, { ButtonStyle } from "../buttons/Button";
import { faBookmark } from "@fortawesome/free-solid-svg-icons";

interface HallCardProps {
  item: Hall;
}

const HallCard = ({ item }: HallCardProps) => {
  console.log(item);
  return (
    item && (
      <Link
        to={`/rents/${item.id}`}
        className="shadow-xl flex flex-col justify-center items-center h-full hover:bg-slate-50 hover:cursor-pointer"
      >
        <img
          className="object-cover w-full h-full max-h-[320px]"
          src={ApiClient.GetPhotoEndpoint(item.type?.photoEndpoint)}
          alt={`Zdjęcie typu sali nr ${item.hallNr}`}
        />
        <div className="flex flex-col py-4 w-full justify-center items-center px-4 relative hover:bg-slate-50 hover:cursor-pointer">
          <div
            className="absolute w-[90px] h-[70px] shadow-2xl left-4 -top-12 flex flex-col justify-center items-center"
            style={{
              background: `linear-gradient(to bottom, #987EFE, #c23eb1)`,
            }}
          >
            <div className="px-2 py-2 flex flex-col justify-center items-center gap-1">
              <p className="self-start text-[20px] text-white text-shadow font-semibold">
                Nr {item.hallNr}
              </p>
            </div>
          </div>
          <h3 className="text-[22px] font-semibold">{item.type?.name}</h3>
          <div className="flex flex-col justify-start items-start w-full">
            <div className="py-[6px] flex flex-col gap-2">
              <div className="text-[18px]  flex flex-row gap-2">
                <p>Piętro:</p>
                <p className="font-semibold">{item.floor}</p>
              </div>
              <div className="text-[18px]  flex flex-row gap-2">
                <p>Ilość miejsc:</p>
                <p className="font-semibold">{item.seatsCount}</p>
              </div>
            </div>
            <div className="flex flex-row justify-between items-center w-full">
              <div className="flex flex-row gap-2">
                <p className="text-[20px]">Cena za 1h:</p>
                <p className="font-bold text-[20px]">{item.rentalPricePerHour} PLN</p>
              </div>
              <Button
                text="Wynajmij"
                height={45}
                icon={faBookmark}
                width={140}
                fontSize={15}
                isFontSemibold={true}
                style={ButtonStyle.Primary}
                action={() => {}}
              />
            </div>
          </div>
        </div>
      </Link>
    )
  );
};
export default HallCard;
