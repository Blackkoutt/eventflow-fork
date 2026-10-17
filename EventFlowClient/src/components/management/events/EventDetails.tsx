import DateFormatter from "../../../helpers/DateFormatter";
import { EventEntity } from "../../../models/response_models";
import LabelText from "../../common/LabelText";
import ApiClient from "../../../services/api/ApiClientService";

interface EventDetailsProps {
  item?: EventEntity;
}

const EventDetails = ({ item }: EventDetailsProps) => {
  return (
    <article className="flex flex-col justify-center items-center px-5 pb-2 gap-5 py-3">
      <div className="flex flex-col justify-center items-center gap-2">
        <h2>Szczegóły wydarzenia</h2>
        <p className="text-textPrimary text-base text-center">
          Poniżej przedstawiono szczegóły dotyczące wybranego wydarzenia
        </p>
      </div>
      <div className="flex flex-col justify-start items-start gap-2">
        <LabelText labelWidth={145} isTextLeft={true} label="ID:" text={item?.id} gap={10} />
        <LabelText labelWidth={145} isTextLeft={true} label="Nazwa:" text={item?.title} gap={10} />
        <LabelText
          labelWidth={145}
          isTextLeft={true}
          label="Data rozpoczęcia:"
          text={DateFormatter.FormatDateFromCalendar(item?.start)}
          gap={10}
        />
        <LabelText
          labelWidth={145}
          isTextLeft={true}
          label="Data zakończenia:"
          text={DateFormatter.FormatDateFromCalendar(item?.end)}
          gap={10}
        />
        <LabelText
          labelWidth={145}
          isTextLeft={true}
          label="Czas trwania:"
          text={DateFormatter.CalculateTimeDifference(item?.start, item?.end)}
          gap={10}
        />
        <LabelText
          labelWidth={145}
          isTextLeft={true}
          label="Kategoria:"
          text={item?.category?.name}
          gap={10}
        />
        <LabelText
          labelWidth={145}
          isTextLeft={true}
          label="Nr sali:"
          text={item?.hall?.hallNr}
          gap={10}
        />
        <LabelText
          labelWidth={145}
          isTextLeft={true}
          label="Bilety:"
          text={item?.tickets
            .filter((t) => !t.isFestival)
            .map((t) => `${t.ticketType?.name}: ${t.price} zł`)
            .join(", ")}
          gap={10}
        />
        <LabelText
          labelWidth={145}
          isTextLeft={true}
          label="Krótki opis:"
          text={item?.shortDescription}
          gap={10}
        />
        <LabelText
          labelWidth={145}
          isTextLeft={true}
          label="Długi opis:"
          text={item?.longDescription}
          gap={10}
        />
        <div className="flex flex-row justify-start items-center gap-2">
          <p
            style={{
              fontSize: 16,
              minWidth: 145,
            }}
            className="font-bold text-left text-textPrimary hover:cursor-default"
          >
            Zdjęcie:
          </p>
          <img
            src={`${ApiClient.GetPhotoEndpoint(item?.photoEndpoint)}`}
            alt={`Zdjęcie wydarzenia o id ${item?.id}`}
            className="w-[220px] h-[220px] object-contain"
          />
        </div>
      </div>
    </article>
  );
};
export default EventDetails;
