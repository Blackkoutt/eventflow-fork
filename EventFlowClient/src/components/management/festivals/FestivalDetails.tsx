import DateFormatter from "../../../helpers/DateFormatter";
import { Festival } from "../../../models/response_models";
import LabelText from "../../common/LabelText";
import ApiClient from "../../../services/api/ApiClientService";
import { DateFormat } from "../../../helpers/enums/DateFormatEnum";

interface FestivalDetailsProps {
  item?: Festival;
  maxWidth?: number;
}

const FestivalDetails = ({ item, maxWidth }: FestivalDetailsProps) => {
  return (
    <article
      className="flex flex-col justify-center items-center px-5 pb-2 gap-5 py-3"
      style={{ maxWidth: maxWidth }}
    >
      <div className="flex flex-col justify-center items-center gap-2">
        <h2>Szczegóły festiwalu</h2>
        <p className="text-textPrimary text-base text-center">
          Poniżej przedstawiono szczegóły dotyczące wybranego festiwalu
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
          label="Bilety:"
          text={item?.tickets
            .filter((t) => t.isFestival)
            .filter(
              (t, index, self) =>
                self.findIndex((tt) => tt.ticketType?.id === t.ticketType?.id) === index
            )
            .map((t) => `${t.ticketType?.name}: ${t.price} zł`)
            .join(", ")}
          gap={10}
        />
        <LabelText
          labelWidth={145}
          isTextLeft={true}
          label="Wydarzenia:"
          text={item?.events
            .map(
              (e) =>
                `${e.title} (${DateFormatter.FormatDate(
                  e.start,
                  DateFormat.DateTime
                )} - ${DateFormatter.FormatDate(e.end, DateFormat.DateTime)})`
            )
            .join(", ")}
          gap={10}
        />
        <LabelText
          labelWidth={145}
          isTextLeft={true}
          label="Organizatorzy:"
          text={item?.organizers.map((x) => x.name).join(", ")}
          gap={10}
        />
        <LabelText
          labelWidth={145}
          isTextLeft={true}
          label="Sponsorzy:"
          text={item?.sponsors.map((x) => x.name).join(", ")}
          gap={10}
        />
        <LabelText
          labelWidth={145}
          isTextLeft={true}
          label="Patroni medialni:"
          text={item?.mediaPatrons.map((x) => x.name).join(", ")}
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
            alt={`Zdjęcie festiwalu o id ${item?.id}`}
            className="w-[220px] h-[220px] object-contain"
          />
        </div>
      </div>
    </article>
  );
};
export default FestivalDetails;
