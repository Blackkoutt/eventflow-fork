import { useEffect, useRef } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft, faChevronRight } from "@fortawesome/free-solid-svg-icons";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import { Partner } from "../../models/response_models";
import fadeIn from "../../assets/fade_in.png";
import fadeOut from "../../assets/fade_out.png";
import PartnerCard from "../common/PartnerCard";

const ScrollStep = 480;

const PartnerList = () => {
  const { data: partners, get: getPartners } = useApi<Partner>(ApiEndpoint.Partner);
  const scrollContainer = useRef<HTMLDivElement>(null);

  useEffect(() => {
    getPartners({ id: undefined, queryParams: undefined });
  }, []);

  const scrollPartners = (direction: number) => {
    scrollContainer.current?.scrollBy({ left: direction * ScrollStep, behavior: "smooth" });
  };

  return (
    <div className="relative w-full min-w-0">
      <img
        src={fadeIn}
        alt="Fade in"
        className="absolute left-0 top-0 h-full z-10 pointer-events-none"
      />
      <img
        src={fadeOut}
        alt="Fade out"
        className="absolute right-0 top-0 h-full z-10 pointer-events-none"
      />
      <button
        type="button"
        aria-label="Poprzedni partnerzy"
        onClick={() => scrollPartners(-1)}
        className="absolute left-2 top-1/2 -translate-y-1/2 z-20 w-10 h-10 rounded-full bg-white shadow-md hover:bg-slate-100"
      >
        <FontAwesomeIcon icon={faChevronLeft} />
      </button>
      <button
        type="button"
        aria-label="Następni partnerzy"
        onClick={() => scrollPartners(1)}
        className="absolute right-2 top-1/2 -translate-y-1/2 z-20 w-10 h-10 rounded-full bg-white shadow-md hover:bg-slate-100"
      >
        <FontAwesomeIcon icon={faChevronRight} />
      </button>
      <div
        ref={scrollContainer}
        className="w-full flex flex-row justify-start items-center overflow-x-auto hide-scrollbar overflow-y-hidden gap-6 px-8"
      >
        {partners?.map((partner) =>
          partner ? <PartnerCard key={partner.id} partner={partner} /> : null
        )}
      </div>
    </div>
  );
};

export default PartnerList;
