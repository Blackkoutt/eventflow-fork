import LabelText from "../../common/LabelText";
import { UserStatistics } from "../../../models/response_models/stats/UserStatistics";

interface UserStatsProps {
  stats: UserStatistics;
}

const EventsStats = ({ stats }: UserStatsProps) => {
  return (
    stats && (
      <div className="flex flex-col justify-start items-start gap-2 mt-6">
        <h3 className="text-textPrimary font-bold text-[25px] border-b-2 border-textPrimary w-full mb-2">
          Użytkownicy
        </h3>
        <LabelText
          labelWidth={405}
          isTextLeft={true}
          label="Całkowita liczba użytkowników: "
          text={stats.totalUsersCount}
        />
        <LabelText
          labelWidth={405}
          isTextLeft={true}
          label="Liczba nowych użytkowników:"
          text={stats.newRegisteredUsersCount}
        />
        <LabelText
          labelWidth={405}
          isTextLeft={true}
          label="Średnia wieku użytkowników: "
          text={stats.usersAgeAvg}
        />
      </div>
    )
  );
};
export default EventsStats;
