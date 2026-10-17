using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Extensions.PdfBuilderExtensions;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Helpers.PdfOptions;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfCommonOptions;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfContentOptions;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfInfoAndStatuteOptions;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfInfoOptions;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfPictureOptions;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfSummaryOptions;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using QuestPDF.Fluent;

namespace EventFlowAPI.Logic.Services.OtherServices.Services
{
    public class PdfBuilderService(IAssetService assetService, IUnitOfWork unitOfWork) : IPdfBuilderService
    {
        private readonly IAssetService _assetService = assetService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<byte[]> CreateHallRentPdf(HallRent hallRent)
        {
            var logoSmall = await _assetService.GetPictureAsBitmap(Enums.Picture.EventFlowLogo_Small, ImageFormat.PNG);
            var additionalServices = (await _unitOfWork.GetRepository<AdditionalServices>().GetAllAsync()).ToList();

            PageOptions pageOptions = new();
            CommonOptions commonOptions = new();
            HeaderOptions headerOptions = new();
            ContentHallRentOptions hallRentContentOptions = new(hallRent);
            HallRentInfoOptions hallRentInfoOptions = new(hallRent);
            HallRentSummaryOptions summaryOptions = new(hallRent, additionalServices);
            InfoAndStatuteHallRentOptions infoAndStatuteOptions = new();
            FooterOptions footerOptions = new();

            using (var memoryStream = new MemoryStream())
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.ConfigurePage(pageOptions);

                        page.Header()
                        .AddBottomLine(commonOptions)
                        .AddHeaderLogo(logoSmall, headerOptions);

                        page.Content()
                        .Column(column =>
                        {
                            column.Item()
                            .AddBottomLine(commonOptions)
                            .AddOrderInfo(hallRentInfoOptions);

                            column.Item()
                            .AddBottomLine(commonOptions)
                            .AddHallRentContent(hallRentContentOptions);

                            column.Item()
                            .AddBottomLine(commonOptions)
                            .AddSummaryContainer(summaryOptions);

                            column.Item()
                            .AddInfoAndStatute(infoAndStatuteOptions);
                        });

                        page.Footer()
                        .AddTopLine(commonOptions)
                        .AddFooterLogoAndPageNumber(logoSmall, footerOptions);
                    });
                }).GeneratePdf(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<byte[]> CreateHallViewPdf(byte[] hallBitmap, Hall hall, HallRent? hallRent = null, Event? eventEntity = null)
        {
            var logoSmall = await _assetService.GetPictureAsBitmap(Enums.Picture.EventFlowLogo_Small, ImageFormat.PNG);
            var testJPG = await _assetService.GetOutputBitmap(TestsOutput.HallRent, ImageFormat.JPEG);

            PageOptions pageOptions = new();
            CommonOptions commonOptions = new();
            HeaderOptions headerOptions = new();
            HallViewInfoOptions hallViewInfoOptions = new(hall, hallRent, eventEntity);
            HallViewPictureOptions hallViewPictureOptions = new();
            FooterOptions footerOptions = new();

            using (var memoryStream = new MemoryStream())
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.ConfigurePage(pageOptions);

                        page.Header()
                        .AddHeaderLogo(logoSmall, headerOptions);

                        page.Content()
                        .Column(column =>
                        {
                            column.Item()
                                .AddPicture(testJPG, hallViewPictureOptions);

                            column.Item()
                            .AlignCenter()
                            .PaddingTop(10)
                            .EnsureSpace(100)
                            .AddFrame(commonOptions)
                            .AddHallViewInfo(hallViewInfoOptions);



                        });

                        page.Footer()
                         .AddPageNumber();
                    });
                }).GeneratePdf(memoryStream);
                return memoryStream.ToArray();
            }
        }


        public async Task<byte[]> CreateEventPassPdf(EventPass eventPass, byte[] eventPassJPGBitmap, EventPassType? oldEventPassType)
        {
            var logoSmall = await _assetService.GetPictureAsBitmap(Enums.Picture.EventFlowLogo_Small, ImageFormat.PNG);
            var eventPassTypes = (await _unitOfWork.GetRepository<EventPassType>().GetAllAsync()).ToList();   

            PageOptions pageOptions = new();
            HeaderOptions headerOptions = new();
            CommonOptions commonOptions = new();
            EventPassInfoOptions eventPassInfoOptions = new(eventPass, oldEventPassType);
            PictureOptions eventPassPictureOptions = new();
            EventPassSummaryOptions summaryOptions = new(eventPass, eventPassTypes);
            InfoAndStatuteEventPassOptions infoAndStatuteOptions = new();
            FooterOptions footerOptions = new();

            using (var memoryStream = new MemoryStream())
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.ConfigurePage(pageOptions);

                        page.Header()
                        .AddBottomLine(commonOptions)
                        .AddHeaderLogo(logoSmall, headerOptions);

                        page.Content()
                        .Column(column =>
                        {
                            column.Item()
                            .AddBottomLine(commonOptions)
                            .AddOrderInfo(eventPassInfoOptions);

                            column.Item()
                            .AddBottomLine(commonOptions)
                            .AddPicture(eventPassJPGBitmap, eventPassPictureOptions);

                            column.Item()
                            .AddBottomLine(commonOptions)
                            .AddSummaryContainer(summaryOptions);

                            column.Item()
                            .AddInfoAndStatute(infoAndStatuteOptions);
                        });

                        page.Footer()
                        .AddTopLine(commonOptions)
                        .AddFooterLogoAndPageNumber(logoSmall, footerOptions);
                    });
                }).GeneratePdf(memoryStream);

                return memoryStream.ToArray();
            }            
        }

        private async Task<List<Ticket>> GetTicketsForEventOrFestival(Reservation reservation)
        {
            if (reservation.IsFestivalReservation)
            {
                return (await _unitOfWork.GetRepository<Ticket>().GetAllAsync(q =>
                        q.Where(t =>
                        t.FestivalId == reservation.Ticket.FestivalId))
                        ).DistinctBy(t => t.TicketType)
                        .ToList();
            }
            return (await _unitOfWork.GetRepository<Ticket>().GetAllAsync(q =>
                    q.Where(t =>
                    t.FestivalId == null &&
                    t.EventId == reservation.Ticket.EventId))
                    ).DistinctBy(t => t.TicketType)
                    .ToList();
        }
        public async Task<byte[]> CreateTicketPdf(Reservation reservation, List<byte[]> tickets)
        { 
            var seatTypes = (await _unitOfWork.GetRepository<SeatType>().GetAllAsync()).ToList();
            var ticketsForEventOrFestival = await GetTicketsForEventOrFestival(reservation);
            var logoSmall = await _assetService.GetPictureAsBitmap(Enums.Picture.EventFlowLogo_Small, ImageFormat.PNG);

            PageOptions pageOptions = new();
            HeaderOptions headerOptions = new();
            CommonOptions commonOptions = new();
            ReservationInfoOptions reservationInfoOptions = new(reservation);
            ContentFestivalEventOptions festivalEventContentOptions = new(reservation);
            PictureOptions ticketPictureOptions = new();
            ReservationSummaryOptions summaryOptions = new(reservation, seatTypes, ticketsForEventOrFestival);                                
            FooterOptions footerOptions = new();  
            InfoAndStatuteTicketOptions infoAndStatuteOptions = new();

            using(var memoryStream = new MemoryStream())
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.ConfigurePage(pageOptions);

                        page.Header()
                        .AddBottomLine(commonOptions)
                        .AddHeaderLogo(logoSmall, headerOptions);

                        page.Content()
                        .Column(column =>
                        {
                            column.Item()
                            .AddBottomLine(commonOptions)
                            .AddOrderInfo(reservationInfoOptions);

                            column.Item()
                            .AddEventOrFestivalContent(festivalEventContentOptions);

                            column.Item()
                            .AddTicketPictures(tickets, ticketPictureOptions, commonOptions);

                            column.Item()
                            .AddBottomLine(commonOptions)
                            .AddSummaryContainer(summaryOptions);

                            column.Item()
                            .AddInfoAndStatute(infoAndStatuteOptions, reservation);
                        });

                        page.Footer()
                        .AddTopLine(commonOptions)
                        .AddFooterLogoAndPageNumber(logoSmall, footerOptions);
                    });
                }).GeneratePdf(memoryStream);

                return memoryStream.ToArray();
            }
        }
    }
}
