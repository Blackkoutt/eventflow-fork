import { Link } from "react-router-dom";
import { News } from "../../models/response_models";
import ApiClient from "../../services/api/ApiClientService";

interface NewsCardProps {
  news: News;
  objectSize: number;
  headerSize: number;
  articleSize: number;
  buttonSize: number;
  shortText?: boolean;
}
const NewsCard = ({
  news,
  objectSize,
  headerSize,
  articleSize,
  buttonSize,
  shortText = false,
}: NewsCardProps) => {
  const getShortDescription = (description: string) => {
    if (shortText && description.length > 85) {
      return description.substring(0, 85) + "...";
    }
    return description;
  };

  return (
    <Link
      to={`/news/${news.id}`}
      className="shadow-xl flex flex-col justify-center items-center gap-2 h-full hover:bg-slate-50 hover:cursor-pointer"
    >
      <div className="relative w-full flex-1 aspect-video overflow-hidden">
        <img
          className="absolute inset-0 w-full h-full object-cover"
          src={ApiClient.GetPhotoEndpoint(news.photoEndpoint)}
          alt={`Zdjęcie newsu ${news.title}`}
        />
      </div>
      <div className="relative w-full flex flex-row justify-start items-center gap-2">
        <div
          className="absolute bg-primaryPurple left-3"
          style={{
            width: `${objectSize}px`,
            height: `${objectSize}px`,
            top: `-${objectSize / 2}px`,
          }}
        ></div>
        <article
          className="flex flex-col justify-start items-start gap-1 w-full pb-4 pr-5 pt-4"
          style={{ paddingLeft: `${objectSize + 28}px` }}
        >
          <h3
            className="text-[24px] font-semibold text-[#4C4C4C]"
            style={{ fontSize: `${headerSize}px` }}
          >
            {news.title}
          </h3>
          <p className="text-textPrimary" style={{ fontSize: `${articleSize}px` }}>
            {getShortDescription(news.shortDescription)}
          </p>
          <p
            className="text-[#987EFE] pb-2 text-[14px] pt-1"
            style={{ fontSize: `${buttonSize}px` }}
          >
            Czytaj dalej &rarr;
          </p>
        </article>
      </div>
    </Link>
  );
};
export default NewsCard;
