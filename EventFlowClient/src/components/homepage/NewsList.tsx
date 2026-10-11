import { useEffect, useMemo } from "react";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { News } from "../../models/response_models";
import NewsCard from "../common/NewsCard";

const NewsList = () => {
  const { data: news, get: getNews } = useApi<News>(ApiEndpoint.News);

  useEffect(() => {
    // News
    const newsPublicationDateQueryParams = {
      sortBy: "PublicationDate",
      sortDirection: "ASC",
      pageNumber: 1,
      pageSize: 6,
    };
    getNews({ id: undefined, queryParams: newsPublicationDateQueryParams });
  }, []);

  const GetNewsInGroups = (news: News[]): News[][] => {
    const groupedNews = [[news[0]], news.slice(1, 3), news.slice(3, 6)];
    return groupedNews;
  };
  const newsGroups = useMemo(() => GetNewsInGroups(news), [news]);

  return (
    <div className="w-full max-w-[1400px] flex flex-col justify-center items-center gap-8">
      <div className="w-full grid grid-cols-12 gap-8">
        <div className="col-span-7">
          {newsGroups[0]?.map((news) =>
            news ? (
              <NewsCard
                key={news.id}
                news={news}
                objectSize={70}
                imageAspect="aspect-[4/3]"
                headerSize={24}
                articleSize={16}
                buttonSize={14}
              />
            ) : null
          )}
        </div>
        <div className="col-span-5 flex flex-col justify-center items-center gap-8">
          {newsGroups[1].map((news) =>
            news ? (
              <NewsCard
                key={news.id}
                news={news}
                objectSize={50}
                imageAspect="aspect-[5/2]"
                headerSize={20}
                articleSize={14}
                buttonSize={12}
              />
            ) : null
          )}
        </div>
      </div>
      <div className="w-full grid grid-cols-3 gap-8">
        {newsGroups[2]?.map((news) =>
          news ? (
            <div key={news.id}>
              <NewsCard
                news={news}
                objectSize={40}
                headerSize={16}
                articleSize={12}
                buttonSize={12}
              />
            </div>
          ) : null
        )}
      </div>
    </div>
  );
};
export default NewsList;
