using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.Statistics.RequestDto;
using EventFlowAPI.Logic.DTO.Statistics.ResponseDto;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using ScottPlot;
using Serilog;

namespace EventFlowAPI.Logic.Services.OtherServices.Services
{
    public class StatisticsService(
        IUnitOfWork unitOfWork,
        IPlotService plotService) : IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPlotService _plotService= plotService;

        private const int MAX_ELEMENTS_IN_PLOT = 5;
        private const int MIN_MAX_ELEMENTS_IN_PLOT = 3;
        public async Task<StatisticsResponseDto> GenerateStatistics(StatisticsRequestDto statisticsRequestDto)
        {
            var totalIncome = await GenerateDefaultIncomeStatistics(statisticsRequestDto.StartDate, statisticsRequestDto.EndDate);

            HallRentStatistics? hallRentStatistics = null;
            if (statisticsRequestDto.IncludeStatisticsAboutHallRent)
                hallRentStatistics = await GenerateStatisticsAboutHallRent(statisticsRequestDto.StartDate, statisticsRequestDto.EndDate);

            EventStatistics? eventStatistics = null;
            if (statisticsRequestDto.IncludeStatisticsAboutEvent)
                eventStatistics = await GenerateStatisticsAboutEvents(statisticsRequestDto.StartDate, statisticsRequestDto.EndDate);

            EventPassStatistics? eventPassStatistics = null;
            if (statisticsRequestDto.IncludeStatisticsAboutEventPasses)
                eventPassStatistics = await GenerateStatisticsAboutEventPasses(statisticsRequestDto.StartDate, statisticsRequestDto.EndDate);

            FestivalStatistics? festivalStatistics = null;
            if(statisticsRequestDto.IncludeStatisticsAboutFestivals)
                festivalStatistics = await GenerateStatisticsAboutFestivals(statisticsRequestDto.StartDate, statisticsRequestDto.EndDate);

            ReservationStatistics? reservationStatistics = null;
            if(statisticsRequestDto.IncludeStatisticsAboutReservations)
                reservationStatistics = await GenerateStatisticsAboutReservations(statisticsRequestDto.StartDate, statisticsRequestDto.EndDate);

            PaymentStatistics? paymentStatistics = null;
            if(statisticsRequestDto.IncludeStatisticsAboutPayments)
                paymentStatistics = await GenerateStatisticsAboutPayments(statisticsRequestDto.StartDate, statisticsRequestDto.EndDate);

            UserStatistics? userStatistics = null;
            if(statisticsRequestDto.IncludeStatisticsAboutUsers)
                userStatistics = await GenerateStatisticsAboutUsers(statisticsRequestDto.StartDate, statisticsRequestDto.EndDate);

            return new StatisticsResponseDto 
            {
                ReportGuid = Guid.NewGuid(),
                StartDate = statisticsRequestDto.StartDate,
                EndDate = statisticsRequestDto.EndDate,
                TotalIncome = totalIncome,
                HallRentStatistics = hallRentStatistics,
                EventStatistics = eventStatistics,
                EventPassStatistics = eventPassStatistics,
                FestivalStatistics = festivalStatistics,
                ReservationStatistics = reservationStatistics,
                PaymentStatistics = paymentStatistics, 
                UserStatistics = userStatistics
            };
        }

        public async Task<StatisticsToPDFDto> GenerateDataForStatisticsPDF(StatisticsRequestDto statisticsRequestDto)
        {
            var statisticsToPdfDto = new StatisticsToPDFDto();

            var statisticsResponse = await GenerateStatistics(statisticsRequestDto);
            statisticsToPdfDto.StatisticsResponseDto = statisticsResponse;

            await AddIncomePlots(statisticsResponse, statisticsToPdfDto.TotalIncomePlotsBitmaps);

            var hallRentsStatistics = statisticsResponse.HallRentStatistics;
            if (hallRentsStatistics != null)
                AddHallRentPlots(hallRentsStatistics, statisticsToPdfDto.HallRentsPlotsBitmaps);

            var eventStatistics = statisticsResponse.EventStatistics;
            if (eventStatistics != null)
                AddEventPlots(eventStatistics, statisticsToPdfDto.EventPlotsBitmaps);

            var eventPassStatistics = statisticsResponse.EventPassStatistics;
            if (eventPassStatistics != null)
                AddEventPassPlots(eventPassStatistics, statisticsToPdfDto.EventPassPlotsBitmaps);

            var festivalStatistics = statisticsResponse.FestivalStatistics;
            if(festivalStatistics != null)
                AddFestivalPlots(festivalStatistics, statisticsToPdfDto.FestivalPlotsBitmaps);

            var reservationStatistics = statisticsResponse.ReservationStatistics;
            if(reservationStatistics != null)
                AddReservationPlots(reservationStatistics, statisticsToPdfDto.ReservationPlotsBitmaps);

            var paymentStatistics = statisticsResponse.PaymentStatistics;
            if(paymentStatistics != null)
                AddPaymentPlots(paymentStatistics, statisticsToPdfDto.PaymentPlotsBitmaps);

            return statisticsToPdfDto;      
        }


        // *******************************
        // Generate Statistics Plots
        // *******************************


        private async Task AddIncomePlots(StatisticsResponseDto statisticsResponse, List<(byte[], PlotType)> plotList)
        {
            var startDate = statisticsResponse.StartDate;
            var endDate = statisticsResponse.EndDate;
            var totalIncome = statisticsResponse.TotalIncome;

            Color[] colors = { Color.FromHex("#9f30f2"), Color.FromHex("#7500f2"), Color.FromHex("#46017a") };
            Dictionary<string, double> incomeDict = new Dictionary<string, double>
            {
                { $"Rezerwacje miejsc ({totalIncome.ReservationsIncome}", totalIncome.ReservationsIncome },
                { $"Rezerwacje sal ({totalIncome.HallRentsIncome}",  totalIncome.HallRentsIncome },
                { $"Karnety ({totalIncome.EventPassesIncome}", totalIncome.EventPassesIncome },
            };
            var totalIncomePlotBitmap = _plotService.GeneratePieChart(incomeDict, colors, "Całkowity przychód z podziałem na źródła");
            plotList.Add((totalIncomePlotBitmap, PlotType.Pie));



            var allNewHallRents = await _unitOfWork.GetRepository<HallRent>().GetAllAsync(q =>
                       q.Where(hr => hr.RentDate >= startDate && hr.RentDate <= endDate && !hr.IsDeleted));
            var allNewEventPasses = await _unitOfWork.GetRepository<EventPass>().GetAllAsync(q =>
                                    q.Where(ep => (ep.StartDate >= startDate && ep.StartDate <= endDate) ||
                                                  (ep.RenewalDate >= startDate && ep.RenewalDate <= endDate) &&
                                                  !ep.IsDeleted));

            var allNewReservations = await _unitOfWork.GetRepository<Reservation>().GetAllAsync(q =>
                                    q.Where(r => r.ReservationDate >= startDate && r.ReservationDate <= endDate && !r.IsDeleted));

            Dictionary<DateTime, double> dailyIncomeDict = [];
            for (var day = startDate.Date; day < endDate.Date; day = day.AddDays(1))
            {
                var dailyHallRents = allNewHallRents.Where(hr => hr.RentDate.Date == day).ToList();
                var dailyEventPasses = allNewEventPasses.Where(ep => ep.StartDate.Date == day ||
                                                                    (ep.RenewalDate != null && ep.RenewalDate != DateTime.MinValue &&
                                                                    ((DateTime)ep.RenewalDate).Date == day)).ToList();
                var dailyReservations = allNewReservations.Where(r => r.ReservationDate.Date == day).ToList();

                var dailyIncome = (double)(dailyHallRents.Sum(hr => hr.PaymentAmount) +
                                  dailyEventPasses.Sum(ep => ep.PaymentAmount) +
                                  dailyReservations.Sum(r => r.PaymentAmount));

                dailyIncomeDict.Add(day, dailyIncome);
            }

            var scatterPlotBitmap = _plotService.GenerateScatterDateTimePlot(dailyIncomeDict, "Przychód w dniach", "Dni", "Przychód (PLN)", Color.FromHex("#7500f2"));

            plotList.Add((scatterPlotBitmap, PlotType.Scatter));
        }

        private void AddPaymentPlots(PaymentStatistics paymentStatistics, List<(byte[], PlotType)> plotList)
        {
            var ticketTypesBitmap = _plotService.GenerateBarChart(paymentStatistics.PaymentTypesDict, "Najczęściej wybierane typy płatności", "Ilość tranzakcji", "Typ płatności", Color.FromHex("#9975e6"));
            plotList.Add((ticketTypesBitmap, PlotType.HorizontalBar));
        }

        private void AddReservationPlots(ReservationStatistics reservationStatistics, List<(byte[], PlotType)> plotList)
        {
            Color[] colors = { Color.FromHex("#651faf"), Color.FromHex("#b140f7"), Color.FromHex("#835cd1"), Color.FromHex("#4415a8"), Color.FromHex("#ac5ed6") };
            var mostProfitableEventPassTypesPlotBitmap = _plotService.GeneratePieChart(reservationStatistics.ReservationFestivalEventsDict, colors, "Stosunek rezerwacji na wydarzenia\n do rezerwacji na festiwale", singularValueUnit: "rezerwacja", pluralValueUnit: "rezerwacje");
            plotList.Add((mostProfitableEventPassTypesPlotBitmap, PlotType.Pie));

            var ticketTypesBitmap = _plotService.GenerateBarChart(reservationStatistics.ReservationTicketsTypesDict, "Najczęściej wybierane typy biletów", "Ilość rezerwacji", "Typ biletu", Color.FromHex("#b66efa"));
            plotList.Add((ticketTypesBitmap, PlotType.HorizontalBar));

            var seatTypesBitmap = _plotService.GenerateBarChart(reservationStatistics.ReservationSeatTypesDict, "Najczęściej wybierane typy miejsc", "Ilość zarezerwowanych miejsc", "Typ miejsca", Color.FromHex("#b66efa"));
            plotList.Add((seatTypesBitmap, PlotType.HorizontalBar));

        }

        private void AddEventPassPlots(EventPassStatistics eventPassStatistics, List<(byte[], PlotType)> plotList)
        {
            Color[] colors = { Color.FromHex("#7f44f4"), Color.FromHex("#62099e"), Color.FromHex("#3c129e"), Color.FromHex("#a558c9"), Color.FromHex("#965ee0") };
            var mostProfitableEventPassTypesPlotBitmap = _plotService.GeneratePieChart(eventPassStatistics.MostProfitableEventsPassTypeDict, colors, "Przychód z karnetów z podziałem na typy");
            plotList.Add((mostProfitableEventPassTypesPlotBitmap, PlotType.Pie));

            var mostPopularEventPassTypePlotBitmap = _plotService.GenerateBarChart(eventPassStatistics.EventPassTypeDict, "Najczęściej kupowane typy karnetów", "Ilość karnetów", "Typy karnetów", Color.FromHex("#7f44f4"));
            plotList.Add((mostPopularEventPassTypePlotBitmap, PlotType.HorizontalBar));
        }

        private void AddFestivalPlots(FestivalStatistics festivalStatistics, List<(byte[], PlotType)> plotList)
        {
            Color[] colors1 = { Color.FromHex("#cd70ff"), Color.FromHex("#6d18a5"), Color.FromHex("#b667fc"), Color.FromHex("#5d1aaf"), Color.FromHex("#6b2edd") };
            var mostProfitableFestivalPlotBitmap = _plotService.GeneratePieChart(festivalStatistics.MostProfitableFestivals, colors1, "Przychód z festiwali");
            plotList.Add((mostProfitableFestivalPlotBitmap, PlotType.Pie));

            var mostPopularEventPlotBitmap = _plotService.GenerateBarChart(festivalStatistics.MostPopularFestivals, "Najpopularniejsze festiwale", "Ilość rezerwacji", "Festiwal", Color.FromHex("#a44bfc"));
            plotList.Add((mostPopularEventPlotBitmap, PlotType.HorizontalBar));

            Color[] colors2 = { Color.FromHex("#9247cc"), Color.FromHex("#7242b5"), Color.FromHex("#5636a0"), Color.FromHex("#6d049e"), Color.FromHex("#c166ff") };
            var organizerFestivalPlotBitmap = _plotService.GeneratePieChart(festivalStatistics.OrganizatorFestivalsDict, colors2, "Organizatorzy festiwali", singularValueUnit: "festiwal", pluralValueUnit: "festiwale");
            plotList.Add((organizerFestivalPlotBitmap, PlotType.Pie));

            Color[] colors3 = { Color.FromHex("#b30ef9"), Color.FromHex("#6d389e"), Color.FromHex("#460889"), Color.FromHex("#8d21d1"), Color.FromHex("#641fd3") };
            var sponsorFestivalPlotBitmap = _plotService.GeneratePieChart(festivalStatistics.SponsorFestivalsDict, colors3, "Sponsorzy festiwali", singularValueUnit: "festiwal", pluralValueUnit: "festiwale");
            plotList.Add((sponsorFestivalPlotBitmap, PlotType.Pie));
            Color[] colors4 = { Color.FromHex("#4e1ac9"), Color.FromHex("#a863dd"), Color.FromHex("#8848af"), Color.FromHex("#9247cc"), Color.FromHex("#8329aa") };
            var mediaPatronFestivalPlotBitmap = _plotService.GeneratePieChart(festivalStatistics.MediaPatronFestivalsDict, colors4, "Patroni mediali festiwali", singularValueUnit: "festiwal", pluralValueUnit: "festiwale");
            plotList.Add((mediaPatronFestivalPlotBitmap, PlotType.Pie));
        }

        private void AddEventPlots(EventStatistics eventStatistics, List<(byte[], PlotType)> plotList)
        {
            Color[] colors = { Color.FromHex("#9f30f2"), Color.FromHex("#5b3b80"), Color.FromHex("#4318a5"), Color.FromHex("#6e2793"), Color.FromHex("#8156ef") };
            var mostProfitableEventPlotBitmap = _plotService.GeneratePieChart(eventStatistics.MostProfitableEvents, colors, "Przychód z poszczególnych wydarzeń");//_plotService.GenerateBarChart(eventStatistics.MostProfitableEvents, "Najbardziej dochodowe wydarzenia", "Dochód", "Wydarzenie", Color.FromHex("#6b26a3"), xAxisTicksIdentity: false);
            plotList.Add((mostProfitableEventPlotBitmap, PlotType.Pie));

            var mostPopularEventPlotBitmap = _plotService.GenerateBarChart(eventStatistics.MostPopularEvents, "Najpopularniejsze wydarzenia", "Ilość rezerwacji", "Wydarzenie", Color.FromHex("#6b26a3"));
            plotList.Add((mostPopularEventPlotBitmap, PlotType.HorizontalBar));

            var eventHallPlotBitmap = _plotService.GenerateBarChart(eventStatistics.EventHallDict, "Najczęściej występujące sale wydarzeń", "Ilość wydarzeń", "Numer sali", Color.FromHex("#6b26a3"));
            plotList.Add((eventHallPlotBitmap, PlotType.HorizontalBar));

            var eventCategoryPlotBitmap = _plotService.GenerateBarChart(eventStatistics.EventCategoryDict, "Najczęściej występujące kategorie wydarzeń", "Ilość wydarzeń", "Numer sali", Color.FromHex("#6b26a3"));
            plotList.Add((eventCategoryPlotBitmap, PlotType.HorizontalBar));
        }

        private void AddHallRentPlots(HallRentStatistics hallRentsStatistics, List<(byte[], PlotType)> plotList)
        {
            var userHallRentPlotBitmap = _plotService.GenerateBarChart(hallRentsStatistics!.UserReservationsDict, "Użytkownicy z największą liczbą rezerwacji sal", "Ilość rezerwacji sal", "Użytkownicy", Color.FromHex("#a46fed"));
            plotList.Add((userHallRentPlotBitmap, PlotType.HorizontalBar));

            var hallPlotBitmap = _plotService.GenerateBarChart(hallRentsStatistics!.HallReservationsDict, "Najcześciej rezerwowane sale", "Ilość rezerwacji sal", "Numery sal", Color.FromHex("#a46fed"));
            plotList.Add((hallPlotBitmap, PlotType.HorizontalBar));

            var hallTypePlotBitmap = _plotService.GenerateBarChart(hallRentsStatistics!.HallTypeReservationsDict, "Najczęściej wybierane typy sal", "Ilość rezerwacji", "Typy sal", Color.FromHex("#a46fed"));
            plotList.Add((hallTypePlotBitmap, PlotType.HorizontalBar));

            var additionalServicePlotBitmap = _plotService.GenerateBarChart(hallRentsStatistics!.HallAddtionalServicesReservationsDict, "Najczęściej wybierane usługi przy rezerwacji sal", "Ilość rezerwacji", "Dodatkowe usługi", Color.FromHex("#a46fed"));
            plotList.Add((additionalServicePlotBitmap, PlotType.HorizontalBar));
        }



        // *******************************
        // Generate Data for Statistics
        // *******************************



        private async Task<TotalIncomeStatistics> GenerateDefaultIncomeStatistics(DateTime startDate, DateTime endDate)
        {
            var allNewHallRents = await _unitOfWork.GetRepository<HallRent>().GetAllAsync(q =>
                                    q.Where(hr => hr.RentDate >= startDate && hr.RentDate <= endDate && !hr.IsDeleted));
            var hallRentsIncome = allNewHallRents.Select(hr => hr.PaymentAmount).Sum();

            var allNewEventPasses = await _unitOfWork.GetRepository<EventPass>().GetAllAsync(q =>
                                    q.Where(ep => (ep.StartDate >= startDate && ep.StartDate <= endDate) ||
                                                  (ep.RenewalDate >= startDate && ep.RenewalDate <= endDate) &&
                                                  !ep.IsDeleted));
            var eventPassIncome = allNewEventPasses.Select(ep => ep.PaymentAmount).Sum();

            var allNewReservations = await _unitOfWork.GetRepository<Reservation>().GetAllAsync(q =>
                                    q.Where(r => r.ReservationDate >= startDate && r.ReservationDate <= endDate && !r.IsDeleted && r.EventPassId == null));
            var reservationIncome = allNewReservations.Select(r => r.PaymentAmount).Sum();

            var totalIncome = hallRentsIncome + eventPassIncome + reservationIncome;
                
            return new TotalIncomeStatistics
            {
                HallRentsIncome = Math.Round((double)hallRentsIncome, 2),
                EventPassesIncome = Math.Round((double)eventPassIncome, 2),
                ReservationsIncome = Math.Round((double)reservationIncome, 2),
                TotalIncome = Math.Round((double)totalIncome, 2),
            };
        }

        private async Task<UserStatistics> GenerateStatisticsAboutUsers(DateTime startDate, DateTime endDate)
        {
            var allUsers = await _unitOfWork.Users.GetAllAsync();
            var totalUsersCount = allUsers.Count();

            var newUsersCount = allUsers.Where(u => u.RegisteredDate >= startDate && u.RegisteredDate <= endDate).Count();

            var userAgeSum = allUsers.Select(u => (DateTime.Now - u.DateOfBirth).TotalDays / 365).Sum();
            var userAgeAvg = (int)(userAgeSum / allUsers.Count());

            return new UserStatistics
            {
                TotalUsersCount = totalUsersCount,
                NewRegistredUsersCount = newUsersCount,
                UsersAgeAvg = userAgeAvg
            };
        }

        private async Task<PaymentStatistics> GenerateStatisticsAboutPayments(DateTime startDate, DateTime endDate)
        {
            var allNewHallRents = await _unitOfWork.GetRepository<HallRent>().GetAllAsync(q =>
                                  q.Where(hr => hr.RentDate >= startDate && hr.RentDate <= endDate && !hr.IsDeleted));

            var hallRentsIncome = allNewHallRents.Select(hr => hr.PaymentAmount).Sum();

            var allNewOrRenewedEventPasses = await _unitOfWork.GetRepository<EventPass>().GetAllAsync(q =>
                                            q.Where(ep => (ep.StartDate >= startDate && ep.StartDate <= endDate) ||
                                                          (ep.RenewalDate >= startDate && ep.RenewalDate <= endDate) &&
                                                          !ep.IsDeleted));

            var eventPassIncome = allNewOrRenewedEventPasses.Select(ep => ep.PaymentAmount).Sum();

            var allNewReservations = await _unitOfWork.GetRepository<Reservation>().GetAllAsync(q =>
                                    q.Where(r => r.ReservationDate >= startDate && r.ReservationDate <= endDate && !r.IsDeleted && r.EventPassId == null));
            var reservationIncome = allNewReservations.Select(r => r.PaymentAmount).Sum();

            var totalTransationsCount = allNewHallRents.Count() + allNewOrRenewedEventPasses.Count() + allNewReservations.Count();
            var totalIncome = (double)(hallRentsIncome + eventPassIncome + reservationIncome);

            var paymentTypesDict = PaymentTypeCatalogue.All
                                    .ToDictionary(
                                        x => x.Name,
                                        x => (double)(allNewOrRenewedEventPasses.Count(ep => (int)ep.PaymentType == x.Id) +
                                             allNewReservations.Count(r => (int)r.PaymentType == x.Id) +
                                             allNewHallRents.Count(hr => (int)hr.PaymentType == x.Id)))
                                             .OrderByDescending(x => x.Value)
                                             .Take(MAX_ELEMENTS_IN_PLOT)
                                             .ToDictionary();

            return new PaymentStatistics
            {
                PaymentsCount = totalTransationsCount,
                TotalTransactionsCost = totalIncome,
                PaymentTypesDict = paymentTypesDict
            };
        }

        private async Task<ReservationStatistics> GenerateStatisticsAboutReservations(DateTime startDate, DateTime endDate)
        {
            var allNewReservations = await _unitOfWork.GetRepository<Reservation>().GetAllAsync(q => q.Where(r => r.ReservationDate >= startDate && r.ReservationDate <= endDate && !r.IsDeleted));
            var allNewReservationsCount = allNewReservations.Count();

            var allNewFestivalReservations = allNewReservations.Where(r => r.Ticket.FestivalId != null);
            var allNewFestivalReservationsCount = allNewFestivalReservations.Count();

            var allCanceledReservationsCount = (await _unitOfWork.GetRepository<Reservation>().GetAllAsync(q => q.Where(r => r.IsDeleted))).Count();

            var allNewEventsReservations = allNewReservations.Where(r => r.Ticket.FestivalId == null);
            var allNewEventsReservationsCount = allNewEventsReservations.Count();

            var reservationFestivalEventsDict = new Dictionary<string, double>
            { 
                { "Wydarzenia", allNewEventsReservationsCount},
                { "Festiwale", allNewFestivalReservationsCount}
            };

            var reservationTicketsTypesDict = allNewReservations.GroupBy(r => r.Ticket.TicketTypeId)
                                                    .ToDictionary(group => group.First().Ticket.TicketType.Name, group => (double)group.Count())
                                                    .OrderByDescending(x => x.Value)
                                                    .Take(MAX_ELEMENTS_IN_PLOT)
                                                    .ToDictionary();

            var reservationSeatTypesDict = allNewReservations.SelectMany(r => r.Seats)
                                            .GroupBy(s => s.SeatTypeId)
                                            .ToDictionary(group => group.First().SeatType.Name, group => (double)group.Count())
                                            .OrderByDescending(x => x.Value)
                                            .Take(MAX_ELEMENTS_IN_PLOT)
                                            .ToDictionary();

            return new ReservationStatistics
            {
                AllNewReservationsCount = allNewReservationsCount,
                AllCanceledReservationsCount = allCanceledReservationsCount,
                AllNewFestivalReservationsCount = allNewFestivalReservationsCount,
                AllNewEventReservationsCount = allNewEventsReservationsCount,
                ReservationFestivalEventsDict = reservationFestivalEventsDict,
                ReservationTicketsTypesDict = reservationTicketsTypesDict,
                ReservationSeatTypesDict = reservationSeatTypesDict
            };
        }

        private async Task<FestivalStatistics> GenerateStatisticsAboutFestivals(DateTime startDate, DateTime endDate)
        {
            var allFestivals = await _unitOfWork.GetRepository<Festival>().GetAllAsync();

            var allAddedFestivals = allFestivals.Where(e => e.AddDate >= startDate && e.AddDate <= endDate && !e.IsDeleted);
            var allAddedFestivalsCount = allAddedFestivals.Count();

            var allFestivalsThatTookPlaceInTimePeriod = allFestivals.Where(e => startDate < e.EndDate && endDate > e.StartDate && !e.IsDeleted);
            var allFestivalsThatTookPlaceInTimePeriodCount = allFestivalsThatTookPlaceInTimePeriod.Count();
            var allCanceledFestivalsCount = allFestivals.Where(e => e.DeleteDate >= startDate && e.DeleteDate <= endDate && e.IsDeleted).Count();

            int minutes = 0;
            int eventsAvg = 0;
            if (allFestivalsThatTookPlaceInTimePeriodCount > 0)
            {
                // Festival duration avg
                Log.Information("Liczba festiwali w analizowanym okresie: {FestivalCount}", allFestivalsThatTookPlaceInTimePeriod.Count());
                var durationSumInMinutes = allFestivalsThatTookPlaceInTimePeriod.Select(hr => (hr.EndDate - hr.StartDate).TotalMinutes).Sum();
                Log.Information("Suma minut trwania festiwali: {TotalMinutes}", durationSumInMinutes);

                var durationAvgInMinutes = durationSumInMinutes / allFestivalsThatTookPlaceInTimePeriodCount;
                Log.Information("Średnia długość festiwali w minutach: {AverageDurationInMinutes}", durationAvgInMinutes);

                int hours = (int)(durationAvgInMinutes / 60);
                minutes = (int)durationAvgInMinutes;
                Log.Information("Średnia długość festiwali: {Hours} godziny i {Minutes} minuty", hours, minutes);
                //TimeSpan durationAvgTimeSpan = new TimeSpan(hours, minutes, 0);

                // Festival event count avg
                var eventCounts = allFestivalsThatTookPlaceInTimePeriod.Select(f => f.Events.Count).Sum();
                eventsAvg = (int)(eventCounts / allFestivalsThatTookPlaceInTimePeriodCount);
            }
           

            // Total eventsIncome
            var totalIncome = (double)Math.Round(allFestivalsThatTookPlaceInTimePeriod
                                .SelectMany(e => e.Tickets.Where(t => !t.IsDeleted && t.FestivalId != null)
                                    .SelectMany(t => t.Reservations.Where(r => r.EventPassId == null)
                                        .Select(r => r.PaymentAmount))).Sum(), 2);

            // Most popualar (most of reservations)
            var mostPopularFestivalsDict = allFestivalsThatTookPlaceInTimePeriod
                                .OrderByDescending(e => e.Tickets.Where(t => !t.IsDeleted && t.FestivalId != null)
                                    .SelectMany(t => t.Reservations.Where(r => r.EventPassId == null)).Count())
                                        .Take(MAX_ELEMENTS_IN_PLOT)
                                            .ToDictionary(e => e.Name, e => (double)(e.Tickets.Where(t => !t.IsDeleted && t.FestivalId != null)
                                                .SelectMany(t => t.Reservations.Where(r => r.EventPassId == null)).Count()));

            // Most profitable festivals (max income from reservations)
            var mostProfitableFestivalsDict = allFestivalsThatTookPlaceInTimePeriod
                              .OrderByDescending(e => Math.Round(e.Tickets.Where(t => !t.IsDeleted && t.FestivalId != null)
                                    .SelectMany(t => t.Reservations.Where(r => r.EventPassId == null)
                                        .Select(r => r.PaymentAmount)).Sum(), 2))
                                            .Take(MAX_ELEMENTS_IN_PLOT)
                                                .ToDictionary(e => e.Name, e => (double)(Math.Round(e.Tickets.Where(t => !t.IsDeleted && t.FestivalId != null)
                                                    .SelectMany(t => t.Reservations.Where(r => r.EventPassId == null)
                                                        .Select(r => r.PaymentAmount)).Sum(), 2)));

            var allOrganizers = await _unitOfWork.GetRepository<Organizer>().GetAllAsync(q => q.Where(o => o.Festivals.Any(f => allFestivalsThatTookPlaceInTimePeriod.Contains(f))));
            var organizatorFestivalsDict = allOrganizers.ToDictionary(o => o.Name, o => (double)o.Festivals.Where(f => allFestivalsThatTookPlaceInTimePeriod.Contains(f)).Count())
                                                       .OrderByDescending(f => f.Value).Take(MIN_MAX_ELEMENTS_IN_PLOT).ToDictionary();

            var allMediaPatrons = await _unitOfWork.GetRepository<MediaPatron>().GetAllAsync(q => q.Where(o => o.Festivals.Any(f => allFestivalsThatTookPlaceInTimePeriod.Contains(f))));
            var mediaPatronsFestivalsDict = allMediaPatrons.ToDictionary(mp => mp.Name, mp => (double)mp.Festivals.Where(f => allFestivalsThatTookPlaceInTimePeriod.Contains(f)).Count())
                                                       .OrderByDescending(f => f.Value).Take(MIN_MAX_ELEMENTS_IN_PLOT).ToDictionary();

            var allSponsors = await _unitOfWork.GetRepository<Sponsor>().GetAllAsync(q => q.Where(o => o.Festivals.Any(f => allFestivalsThatTookPlaceInTimePeriod.Contains(f))));
            var sponsorsFestivalsDict = allSponsors.ToDictionary(s => s.Name, s => (double)s.Festivals.Where(f => allFestivalsThatTookPlaceInTimePeriod.Contains(f)).Count())
                                                       .OrderByDescending(f => f.Value).Take(MIN_MAX_ELEMENTS_IN_PLOT).ToDictionary();

            return new FestivalStatistics
            {
                AllAddedFestivalsCount = allAddedFestivalsCount,
                AllFestivalsThatTookPlaceInTimePeriod = allFestivalsThatTookPlaceInTimePeriodCount, 
                AllCanceledFestivalsCount = allCanceledFestivalsCount,
                DurationAvg = minutes,
                EventCountAvg = eventsAvg,
                TotalFestivalsIncome = totalIncome,
                MostPopularFestivals = mostPopularFestivalsDict,
                MostProfitableFestivals = mostProfitableFestivalsDict,
                OrganizatorFestivalsDict = organizatorFestivalsDict,
                MediaPatronFestivalsDict = mediaPatronsFestivalsDict,
                SponsorFestivalsDict = sponsorsFestivalsDict
            };

        }

        private async Task<EventPassStatistics> GenerateStatisticsAboutEventPasses(DateTime startDate, DateTime endDate)
        {
            var allEventPasses = await _unitOfWork.GetRepository<EventPass>().GetAllAsync();

            var allBoughtEventPasses = allEventPasses.Where(ep => ep.StartDate >= startDate && ep.StartDate <= endDate && !ep.IsDeleted && (ep.RenewalDate == null || ep.RenewalDate == DateTime.MinValue));
            var allBoughtEventPassesCount = allBoughtEventPasses.Count();

            var allRenewedEventPasses = allEventPasses.Where(ep => ep.RenewalDate >= startDate && ep.RenewalDate <= endDate && !ep.IsDeleted);
            var allRenewedEventPassesCount = allRenewedEventPasses.Count();

            var allCanceledEventPassesCount = allEventPasses.Where(ep => ep.DeleteDate >= startDate && ep.DeleteDate <= endDate && ep.IsDeleted).Count();

            var newOrRenewedEventPasses = allBoughtEventPasses.Union(allRenewedEventPasses);

            var totalIncome = (double)Math.Round(newOrRenewedEventPasses.Select(ep => ep.PaymentAmount).Sum(), 2);

            // Event Pass Types Dict
            var mostBoughtEventPassTypesDict = newOrRenewedEventPasses.GroupBy(ep => ep.PassTypeId)
                                                .OrderByDescending(g => g.Count())
                                                .Take(MAX_ELEMENTS_IN_PLOT)
                                                .ToDictionary(group => $"{group.First().PassType.Name}", group => (double)group.Count());


            var mostProfitableEventsPassTypeDict = newOrRenewedEventPasses.GroupBy(ep => ep.PassTypeId)
                                                .ToDictionary(g => g.First().PassType.Name, g => (double)Math.Round(g.Select(ep => ep.PaymentAmount).Sum(), 2))
                                                .OrderByDescending(ep => ep.Value)
                                                .Take(MAX_ELEMENTS_IN_PLOT).ToDictionary();
                                                
            return new EventPassStatistics
            {
                AllBougthEventPassesCount = allBoughtEventPassesCount,
                AllRenewedEventPassesCount = allRenewedEventPassesCount,
                AllCanceledEventPassesCount = allCanceledEventPassesCount,
                TotalEventPassesIncome = totalIncome,
                EventPassTypeDict = mostBoughtEventPassTypesDict,
                MostProfitableEventsPassTypeDict = mostProfitableEventsPassTypeDict
            };
        }

        private async Task<EventStatistics> GenerateStatisticsAboutEvents(DateTime startDate, DateTime endDate)
        {
            var allEvents = await _unitOfWork.GetRepository<Event>().GetAllAsync();

            var allAddedEvents = allEvents.Where(e => e.AddDate >= startDate && e.AddDate <= endDate && !e.IsDeleted);
            var allAddedEventsCount = allAddedEvents.Count();

            var allEventsThatTookPlaceInTimePeriod = allEvents.Where(e => startDate < e.EndDate && endDate > e.StartDate && !e.IsDeleted);
            var allEventsThatTookPlaceInTimePeriodCount = allEventsThatTookPlaceInTimePeriod.Count();

            var allCanceledEventsCount = allEvents.Where(e => e.DeleteDate >= startDate && e.DeleteDate <= endDate && e.IsDeleted).Count();

            // Srednia długość eventów
            int minutes = 0;
            if(allEventsThatTookPlaceInTimePeriodCount > 0)
            {
                var durationSumInMinutes = allEventsThatTookPlaceInTimePeriod.Select(hr => (hr.EndDate - hr.StartDate).TotalMinutes).Sum();
                var durationAvgInMinutes = durationSumInMinutes / allEventsThatTookPlaceInTimePeriodCount;
                int hours = (int)(durationAvgInMinutes / 60);
                minutes = (int)durationAvgInMinutes;
            }
            //TimeSpan durationAvgTimeSpan = new TimeSpan(hours, minutes, 0);

            // Total eventsIncome
            var totalIncome = (double)Math.Round(allEventsThatTookPlaceInTimePeriod.
                                SelectMany(e => e.Tickets.Where(t => !t.IsDeleted && t.FestivalId == null)
                                        .SelectMany(t => t.Reservations.Where(r => r.EventPassId == null)
                                            .Select(r => r.PaymentAmount))).Sum(), 2);;

            // Most popualar (most of reservations)
            var mostPopularEventsDict = allEventsThatTookPlaceInTimePeriod
                                .OrderByDescending(e => e.Tickets.Where(t => !t.IsDeleted && t.FestivalId == null)
                                    .SelectMany(t => t.Reservations.Where(r => r.EventPassId == null)).Count())
                                        .Take(MAX_ELEMENTS_IN_PLOT)
                                            .ToDictionary(e => e.Name, e => (double)(e.Tickets.Where(t => !t.IsDeleted && t.FestivalId == null)
                                                    .SelectMany(t => t.Reservations.Where(r => r.EventPassId == null)).Count()));

            // Most profitable events (max income from reservations)
            var mostProfitableEventsDict = allEventsThatTookPlaceInTimePeriod
                              .OrderByDescending(e => Math.Round(e.Tickets.Where(t => !t.IsDeleted && t.FestivalId == null)
                                    .SelectMany(t => t.Reservations.Where(r => r.EventPassId == null)
                                        .Select(r => r.PaymentAmount)).Sum(), 2))
                                            .Take(MAX_ELEMENTS_IN_PLOT)
                                                .ToDictionary(e => e.Name, e => (double)(Math.Round(e.Tickets.Where(t => !t.IsDeleted && t.FestivalId == null)
                                                    .SelectMany(t => t.Reservations.Where(r => r.EventPassId == null)
                                                        .Select(r => r.PaymentAmount)).Sum(), 2)));

            // Events hall
            var eventHallDict = allEventsThatTookPlaceInTimePeriod
                                       .GroupBy(e => e.Hall.HallNr)
                                       .OrderByDescending(g => g.Count())
                                       .Take(MAX_ELEMENTS_IN_PLOT)
                                       .ToDictionary(group => $"{group.First().Hall.HallNr}", group => (double)group.Count());

            // Events category
            var eventCategoryDict = allEventsThatTookPlaceInTimePeriod
                                      .GroupBy(e => e.CategoryId)
                                      .OrderByDescending(g => g.Count())
                                      .Take(MAX_ELEMENTS_IN_PLOT)
                                      .ToDictionary(group => $"{group.First().Category.Name}", group => (double)group.Count());


            return new EventStatistics
            {
                AllAddedEventsCount = allAddedEventsCount,
                AllEventsThatTookPlaceInTimePeriod = allEventsThatTookPlaceInTimePeriodCount,
                AllCanceledEventsCount = allCanceledEventsCount,
                DurationAvg = minutes,
                TotalEventsIncome = totalIncome,
                MostPopularEvents = mostPopularEventsDict,
                MostProfitableEvents = mostProfitableEventsDict,
                EventHallDict = eventHallDict,
                EventCategoryDict = eventCategoryDict,
            };
        }

        private async Task<HallRentStatistics> GenerateStatisticsAboutHallRent(DateTime startDate, DateTime endDate)
        {
            var allHallRents = await _unitOfWork.GetRepository<HallRent>().GetAllAsync();
            
            var allAddedHallRents = allHallRents.Where(hr => hr.RentDate >= startDate && hr.RentDate <= endDate && !hr.IsDeleted);
            var allAddedHallRentsCount = allAddedHallRents.Count();

            var allHallRentsThatTookPlaceInTimePeriod = allHallRents.Where(hr => startDate < hr.EndDate && endDate > hr.StartDate && !hr.IsDeleted);
            var allHallRentsThatTookPlaceInTimePeriodCount = allHallRentsThatTookPlaceInTimePeriod.Count();
            
            var allCanceledHallRentsCount = allHallRents.Where(hr => hr.DeleteDate >= startDate && hr.DeleteDate <= endDate && hr.IsDeleted).Count();

            // Srednia długość rezerwacji sal
            int minutes = 0;
            if(allHallRentsThatTookPlaceInTimePeriodCount > 0)
            {
                var durationSumInMinutes = allHallRentsThatTookPlaceInTimePeriod.Select(hr => (hr.EndDate - hr.StartDate).TotalMinutes).Sum();
                var durationAvgInMinutes = durationSumInMinutes / allHallRentsThatTookPlaceInTimePeriodCount;
                int hours = (int)(durationAvgInMinutes / 60); // godziny
                minutes = (int)durationAvgInMinutes;
            }

            //TimeSpan durationAvgTimeSpan = new TimeSpan(hours, minutes, 0);

            // Total eventsIncome
            var totalIncome = (double)Math.Round(allHallRentsThatTookPlaceInTimePeriod.
                                Select(hr => hr.PaymentAmount).Sum(), 2);

            // User res
            var userReservationsDict = allHallRentsThatTookPlaceInTimePeriod.GroupBy(hr =>
                                    hr.UserId).OrderByDescending(g => g.Count())
                                    .Take(MAX_ELEMENTS_IN_PLOT)
                                    .ToDictionary(group => $"{group.First().User.Name} {group.First().User.Surname}", group => (double)group.Count());

            // Hall res
            var hallReservationsDict = allHallRentsThatTookPlaceInTimePeriod
                                    .GroupBy(hr => hr.Hall.DefaultId)
                                    .OrderByDescending(g => g.Count())
                                    .Take(MAX_ELEMENTS_IN_PLOT)
                                    .ToDictionary(group => $"{group.First().Hall.HallNr}", group => (double)group.Count());

            // HallType 
            var hallTypeReservationsDict = allHallRentsThatTookPlaceInTimePeriod
                                        .GroupBy(hr => hr.Hall.HallTypeId)
                                        .OrderByDescending(g => g.Count())
                                        .Take(MAX_ELEMENTS_IN_PLOT)
                                        .ToDictionary(group => $"{group.First().Hall.Type.Name}", group => (double)group.Count());

            // Hall additionalServices
            var addtionalServicesReservationsDict = allHallRentsThatTookPlaceInTimePeriod.
                                                    SelectMany(hr => hr.AdditionalServices.Select(service => new { hr, service }))
                                                    .GroupBy(x => x.service)
                                                    .OrderByDescending(g => g.Count())
                                                    .Take(MAX_ELEMENTS_IN_PLOT)
                                                    .ToDictionary(group => $"{group.Key.Name}", group => (double)group.Count());

            return new HallRentStatistics
            {
                AllAddedHallRentsCount = allAddedHallRentsCount,
                AllHallRentsThatTookPlaceInTimePeriod = allHallRentsThatTookPlaceInTimePeriodCount,
                AllCanceledHallRentsCount = allCanceledHallRentsCount,
                DurationAvg = minutes,//durationAvgTimeSpan,
                TotalHallRentsIncome = totalIncome,
                UserReservationsDict = userReservationsDict,
                HallReservationsDict = hallReservationsDict,
                HallTypeReservationsDict = hallTypeReservationsDict,
                HallAddtionalServicesReservationsDict = addtionalServicesReservationsDict
            };
        }
    }
}
