import { useEffect, useState } from "react";
import { ApiEndpoint } from "../helpers/enums/ApiEndpointEnum";
import useApi from "../hooks/useApi";
import { EventCategory, EventEntity } from "../models/response_models";
import EventCard from "../components/common/EventCard";

interface NameValue {
  name: string;
  value: number | null;
}

function ArchivePage() {
  const { data: items, get: getItems } = useApi<EventEntity>(ApiEndpoint.Event);
  const { data: categories, get: getCategories } = useApi<EventCategory>(ApiEndpoint.EventCategory);
  const [selectedCategory, setSelectedCategory] = useState<NameValue>({
    name: "Wszystkie",
    value: null,
  });

  useEffect(() => {
    getCategories({ id: undefined, queryParams: undefined });
  }, []);

  useEffect(() => {
    let queryParams;
    if (selectedCategory.value != null) {
      queryParams = {
        status: "Expired",
        sortBy: "StartDate",
        sortDirection: "ASC",
        categoryId: selectedCategory.value,
      };
    } else {
      queryParams = {
        status: "Expired",
        sortBy: "StartDate",
        sortDirection: "ASC",
      };
    }

    getItems({ id: undefined, queryParams: queryParams });
  }, [selectedCategory]);

  return (
    <div className="w-full py-10">
      <div className="flex flex-col gap-6">
        <h1>Archiwum</h1>
        <div className="flex justify-center items-center flex-wrap min-w-[150px] gap-4">
          <p className="font-semibold text-[18px]">Kategorie:</p>
          <button
            className={`px-4 py-2 rounded-full ${
              selectedCategory.name == "Wszystkie"
                ? "text-white bg-primaryPurple"
                : "bg-gray-200 text-gray-600"
            } `}
            style={{ fontSize: 18 }}
            onClick={() =>
              setSelectedCategory({
                name: "Wszystkie",
                value: null,
              })
            }
          >
            Wszystkie
          </button>
          {categories.map((cat) => (
            <button
              key={cat.id}
              className={`px-4 py-2 rounded-full min-w-[150px] ${
                selectedCategory.name == cat.name
                  ? "text-white bg-primaryPurple"
                  : "bg-gray-200 text-gray-600"
              }`}
              onClick={() =>
                setSelectedCategory({
                  name: cat.name,
                  value: cat.id,
                })
              }
              style={{ fontSize: 18 }}
            >
              {cat.name}
            </button>
          ))}
        </div>
      </div>

      <div className="py-6">
        {items && (
          <div className="grid 3xl:grid-cols-3 xl:grid-cols-2 3xl:gap-4 xl:gap-10">
            {items.map((item) => (
              <EventCard key={item.id} event={item} isArchived={true} />
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
export default ArchivePage;
