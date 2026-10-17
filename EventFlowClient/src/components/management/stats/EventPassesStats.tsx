import { Chart } from "primereact/chart";
import LabelText from "../../common/LabelText";
import { useBarChart } from "../../../hooks/useBarChart";
import { usePieChart } from "../../../hooks/usePieChart";
import { EventPassStatistics } from "../../../models/response_models/stats/EventPassStatistics";

interface EventPassesStatsProps {
  stats: EventPassStatistics;
}

const EventPassesStats = ({ stats }: EventPassesStatsProps) => {
  const { chartData: mostProfitableChartData, chartOptions: mostProfitableChartOptions } =
    usePieChart(
      Object.keys(stats.mostProfitableEventsPassTypeDict),
      Object.values(stats.mostProfitableEventsPassTypeDict),
      ["#7d41f0", "#3c0f9b", "#5f0a9b", "#5a377d", "#a06eeb"],
      ["#8e52f1", "#4d1fac", "#6f1bac", "#6b488e", "#b17ffc"],
      "Przychód z karnetów z podziałem na typy"
    );

  const { chartData: passTypesChartData, chartOptions: passTypesChartOptions } = useBarChart(
    stats.eventPassTypeDict,
    "Najczęsciej kupowane typy karnetów",
    "#7d41f0",
    "Ilość karnetów"
  );

  return (
    stats && (
      <div className="flex flex-col justify-start items-start gap-2 mt-6">
        <h3 className="text-textPrimary font-bold text-[25px] border-b-2 border-textPrimary w-full mb-2">
          Karnety
        </h3>
        <LabelText
          labelWidth={405}
          isTextLeft={true}
          label="Liczba nowo zakupionych karnetów:"
          text={stats.allBoughtEventPassesCount}
        />
        <LabelText
          labelWidth={405}
          isTextLeft={true}
          label="Liczba przedłużonych karnetów:"
          text={stats.allRenewedEventPassesCount}
        />
        <LabelText
          labelWidth={405}
          isTextLeft={true}
          label="Liczba anulowanych karnetów:"
          text={stats.allCanceledEventPassesCount}
        />
        <LabelText
          labelWidth={405}
          isTextLeft={true}
          label="Całkowity przychód z karnetów:"
          text={`${stats.totalEventPassesIncome} PLN`}
        />
        <section
          id="charts"
          className="flex flex-col justify-center items-center gap-6 mt-6 w-full"
        >
          <Chart
            type="pie"
            data={mostProfitableChartData}
            options={mostProfitableChartOptions}
            className="w-[45%] h-[4%]"
          />
          <Chart
            type="bar"
            data={passTypesChartData}
            options={passTypesChartOptions}
            className="w-[75%]"
          />
        </section>
      </div>
    )
  );
};
export default EventPassesStats;
