import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import Checkbox from "../../components/common/Checkbox";
import DatePicker from "../../components/common/forms/DatePicker";
import { zodResolver } from "@hookform/resolvers/zod";
import FormButton from "../../components/common/forms/FormButton";
import { StatisticsRequest, statisticsSchema } from "../../models/create_schemas/StatisticsSchema";
import { useEffect } from "react";
import { Statistics } from "../../models/response_models/stats/Statistics";
import { ApiEndpoint } from "../../helpers/enums/ApiEndpointEnum";
import useApi from "../../hooks/useApi";
import TotalIncomeStats from "../../components/management/stats/TotalIncomeStats";
import HallRentsStats from "../../components/management/stats/HallRentsStats";
import EventsStats from "../../components/management/stats/EventsStats";
import EventPassesStats from "../../components/management/stats/EventPassesStats";
import FestivalsStats from "../../components/management/stats/FestivalsStats";
import ReservationsStats from "../../components/management/stats/ReservationsStats";
import UserStats from "../../components/management/stats/UserStats";
import { faChartLine, faDownload } from "@fortawesome/free-solid-svg-icons";
import Button, { ButtonStyle } from "../../components/buttons/Button";
import BlobService from "../../services/BlobService";
import DateFormatter from "../../helpers/DateFormatter";

const StatsManagement = () => {
  const { data: stats, get: getStats } = useApi<Statistics>(ApiEndpoint.Stats);
  const { data: statsPDF, get: getStatsPDF } = useApi<Blob>(ApiEndpoint.StatsPDF);

  const methods = useForm<StatisticsRequest>({
    resolver: zodResolver(statisticsSchema),
    defaultValues: {
      includeStatisticsAboutHallRent: false,
      includeStatisticsAboutEvent: false,
      includeStatisticsAboutEventPasses: false,
      includeStatisticsAboutFestivals: false,
      includeStatisticsAboutReservations: false,
      includeStatisticsAboutUsers: false,
    },
  });
  const { handleSubmit, formState, watch } = methods;
  const { errors, isSubmitting } = formState;

  useEffect(() => {
    const data = {
      startDate: new Date(new Date().setMonth(new Date().getMonth() - 1)).toISOString(),
      endDate: new Date().toISOString(),
    };
    getStats({ id: undefined, queryParams: data });
  }, []);

  const downloadFile = async () => {
    const data = watch();
    let formettedData;
    if (data.startDate.toString() === "" || data.endDate.toString() === "") {
      formettedData = {
        ...data,
        startDate: new Date(new Date().setMonth(new Date().getMonth() - 1)).toISOString(),
        endDate: new Date().toISOString(),
      };
    } else {
      formettedData = {
        ...data,
        startDate: DateFormatter.ParseDate(data.startDate.toString())?.toISOString(),
        endDate: DateFormatter.ParseDate(data.endDate.toString())?.toISOString(),
      };
    }

    await getStatsPDF({ id: undefined, queryParams: formettedData, isBlob: true });
  };

  useEffect(() => {
    if (statsPDF.length !== 0) {
      const fileName = `raport.pdf`;
      BlobService.DownloadBlob(statsPDF[0], fileName);
    }
  }, [statsPDF]);

  const onSubmit: SubmitHandler<StatisticsRequest> = (data) => {
    const formattedData = {
      ...data,
      startDate: data.startDate ? data.startDate.toISOString() : undefined,
      endDate: data.endDate ? data.endDate.toISOString() : undefined,
    };
    console.log(formattedData);
    getStats({ id: undefined, queryParams: formattedData });
  };

  useEffect(() => {
    console.log(stats);
  }, [stats]);

  return (
    <div className="flex flex-col justify-start items-start self-start px-10 w-full">
      <div className="flex flex-row justify-start items-start gap-10 w-full">
        <div className="flex flex-col justify-start items-start gap-2 min-w-[300px]">
          <h2 className="text-[#374151] mb-6 w-full text-center">Statystyki</h2>
          <FormProvider {...methods}>
            <form
              className="flex flex-col justify-start items-start gap-6 w-full"
              onSubmit={handleSubmit(onSubmit)}
            >
              <div className="flex flex-col justify-start items-start gap-2 w-full">
                <DatePicker
                  label="Początkowa data"
                  name="startDate"
                  error={errors.startDate}
                  errorHeight={15}
                />
                <DatePicker
                  label="Końcowa data"
                  name="endDate"
                  error={errors.endDate}
                  errorHeight={15}
                />
                <div className="flex flex-col justify-start items-start gap-4 w-full">
                  <Checkbox
                    name="includeStatisticsAboutHallRent"
                    color="#7B2CBF"
                    text="Dołącz statystyki rezerwacji sal"
                    textColor="#4C4C4C"
                    fontSize={16}
                  />
                  <Checkbox
                    name="includeStatisticsAboutEvent"
                    color="#7B2CBF"
                    text="Dołącz statystyki wydarzeń"
                    textColor="#4C4C4C"
                    fontSize={16}
                  />
                  <Checkbox
                    name="includeStatisticsAboutEventPasses"
                    color="#7B2CBF"
                    text="Dołącz statystyki karnetów"
                    textColor="#4C4C4C"
                    fontSize={16}
                  />
                  <Checkbox
                    name="includeStatisticsAboutFestivals"
                    color="#7B2CBF"
                    text="Dołącz statystyki festiwali"
                    textColor="#4C4C4C"
                    fontSize={16}
                  />
                  <Checkbox
                    name="includeStatisticsAboutReservations"
                    color="#7B2CBF"
                    text="Dołącz statystyki rezerwacji"
                    textColor="#4C4C4C"
                    fontSize={16}
                  />
                  <Checkbox
                    name="includeStatisticsAboutUsers"
                    color="#7B2CBF"
                    text="Dołącz statystyki użytkowników"
                    textColor="#4C4C4C"
                    fontSize={16}
                  />
                </div>
              </div>
              <FormButton
                isSubmitting={isSubmitting}
                icon={faChartLine}
                iconSize={18}
                text="Pokaż statystyki"
              />
              <Button
                isFullWidth={true}
                text="Pobierz"
                icon={faDownload}
                style={ButtonStyle.Secondary}
                action={downloadFile}
                height={64}
                rounded="rounded-md"
              />
            </form>
          </FormProvider>
        </div>
        <div className="flex flex-col justify-center w-full">
          {stats && stats.length > 0 && (
            <>
              <TotalIncomeStats stats={stats[0].totalIncome} />
              {stats[0].hallRentStatistics && (
                <HallRentsStats stats={stats[0].hallRentStatistics} />
              )}
              {stats[0].eventStatistics && <EventsStats stats={stats[0].eventStatistics} />}
              {stats[0].eventPassStatistics && (
                <EventPassesStats stats={stats[0].eventPassStatistics} />
              )}
              {stats[0].festivalStatistics && (
                <FestivalsStats stats={stats[0].festivalStatistics} />
              )}
              {stats[0].reservationStatistics && (
                <ReservationsStats stats={stats[0].reservationStatistics} />
              )}
              {stats[0].userStatistics && <UserStats stats={stats[0].userStatistics} />}
            </>
          )}{" "}
        </div>
      </div>
    </div>
  );
};
export default StatsManagement;
