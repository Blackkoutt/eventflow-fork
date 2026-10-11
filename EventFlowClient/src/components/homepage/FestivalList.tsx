import { useEffect } from "react";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { Festival } from "../../models/response_models";
import FestivalCard from "../common/FestivalCard";

const FestivalList = () => {
  const { data: festivals, get: getFestivals } = useApi<Festival>(ApiEndpoint.Festival);

  useEffect(() => {
    // Festivals
    const festivalStartDateQueryParams = {
      sortBy: "StartDate",
      sortDirection: "ASC",
      pageNumber: 1,
      pageSize: 1,
    };
    getFestivals({ id: undefined, queryParams: festivalStartDateQueryParams });
  }, []);

  return (
    <>
      {festivals[0] ? (
        <div className="w-full">
          <FestivalCard festival={festivals[0]} />
        </div>
      ) : (
        ""
      )}
    </>
  );
};
export default FestivalList;
