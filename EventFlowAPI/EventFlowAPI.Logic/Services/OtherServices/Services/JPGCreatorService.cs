using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces.Configuration.HallConfiguration;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces.Configuration.PassConfiguration;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces.Configuration.TicketConfiguration;
using EventFlowAPI.Logic.UnitOfWork;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace EventFlowAPI.Logic.Services.OtherServices.Services
{
    public class JPGCreatorService(
        IFestivalTicketConfiguration festivalTicketConfig,
        IEventTicketConfiguration eventTicketConfig,
        IHallSeatsConfiguration hallSeatsConfig,
        IEventPassConfiguration eventPassConfig,
        IQRCodeGeneratorService qrCoder,
        IUnitOfWork unitOfWork,
        IAssetService assetService) : IJPGCreatorService
    {
        private readonly IFestivalTicketConfiguration _festivalTicketConfig = festivalTicketConfig;
        private readonly IEventTicketConfiguration _eventTicketConfig = eventTicketConfig;
        private readonly IHallSeatsConfiguration _hallSeatsConfig = hallSeatsConfig;
        private readonly IEventPassConfiguration _eventPassConfig = eventPassConfig;
        private readonly IQRCodeGeneratorService _qrCoder = qrCoder;
        private readonly IAssetService _assetService = assetService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<byte[]> CreateHallJPG(Hall hall)
        {
            var logo = await _assetService.GetPicture(Picture.EventFlowLogo_Big);
            _hallSeatsConfig.SetCanvasDimensions(hall, logo, isDefault: false);

            var canvasWidth = _hallSeatsConfig.CanvasWidth;
            var canvasHeight = _hallSeatsConfig.CanvasHeight;
            var canvasColor = _hallSeatsConfig.CanvasBackgroundColor;        

            using (Image<Rgba32> canvas = new Image<Rgba32>(canvasWidth, canvasHeight, canvasColor))
            {
                // Canvas border
                DrawCanvasBorder(canvas);

                // Stage
                DrawStage(canvas, hall);

                // Seats
                await DrawSeats(canvas, hall);

                // Legend
                await DrawLegend(canvas, hall);

                // Watermark
                DrawWatermark(canvas, logo);

                var outputPath = _assetService.GetOutputTestPath(TestsOutput.HallRent);
                await canvas.SaveAsJpegAsync(outputPath);

                return await canvas.AsBitmap(ImageFormat.JPEG);
            }
        }


        public async Task<byte[]> CreateEventPass(EventPass eventPass)
        {
            // TEST
            var outputPath = _assetService.GetOutputTestPath(TestsOutput.EventPass);

            var image = await _assetService.GetTemplate(Template.EventPass);

            var passTypeOptions = _eventPassConfig.EventPassTypePrintingOptions;
            image.DrawText(eventPass.PassType.Name, passTypeOptions);

            var passOwnerOptions = _eventPassConfig.EventPassOwnerPrintingOptions;
            image.DrawText($"{eventPass.User.Name} {eventPass.User.Surname}", passOwnerOptions);

            var passDateOptions = _eventPassConfig.EventPassDatePrintingOptions;
            image.DrawText($"{eventPass.EndDate.ToString(passDateOptions.DateFormat)}", passDateOptions);

            var qrCodeOptions =_eventPassConfig.EventPassQrCodePrintingOptions;
            var qrCode = _qrCoder.GenerateQRCode(eventPass.EventPassGuid.ToString(), qrCodeOptions.Size);
            image.DrawImage(qrCode, qrCodeOptions);

            await image.SaveAsJpegAsync(outputPath);

            return await image.AsBitmap(ImageFormat.JPEG);
        }

        public async Task<byte[]> CreateEventTicket(Reservation reservation)
        {
            // TEST
            var outputPath = _assetService.GetOutputTestPath(TestsOutput.EventPath);

            var image = await _assetService.GetTemplate(Template.EventTicket);
            var eventEntity = reservation.Ticket.Event;

            _eventTicketConfig.SetDefaultPrintingParams();

            var titleOptions = _eventTicketConfig.GetTitlePrintingOptions(eventEntity);     
            image.DrawText(eventEntity.Name, titleOptions);

            var dateOptions = _eventTicketConfig.GetDatePrintingOptions();
            image.DrawText($"{eventEntity.StartDate.ToString(dateOptions.DateFormat)}", dateOptions);

            var priceOptions = _eventTicketConfig.GetPricePrintingOptions(reservation);
            image.DrawText($"{reservation.Ticket.Price} {priceOptions.Currency}", priceOptions);

            var hallOptions = _eventTicketConfig.GetHallPrintingOptions();
            image.DrawText(eventEntity.Hall.HallNr.ToString(), hallOptions);

            var durationOptions = _eventTicketConfig.GetDurationPrintingOpitons(eventEntity);
            image.DrawText($"{(eventEntity.EndDate - eventEntity.StartDate).TotalMinutes} min", durationOptions);
 
            var seatsOptions = _eventTicketConfig.GetSeatsPrintingOptions();
            image.DrawText(string.Join(", ", reservation.Seats.Select(s => s.SeatNr)), seatsOptions);

            var qrCodeOptions = _eventTicketConfig.GetQRCodePrintingOptions();
            var qrCode = _qrCoder.GenerateQRCode(reservation.ReservationGuid.ToString(), qrCodeOptions.Size);
            image.DrawImage(qrCode, qrCodeOptions);
                
            // TEST
            await image.SaveAsJpegAsync(outputPath);

            return await image.AsBitmap(ImageFormat.JPEG);         
        }

        public async Task<List<byte[]>> CreateFestivalTicket(Festival festival, List<Reservation> reservations)
        {
            List<byte[]> imagesBitmaps = [];

            // TEST
            var outputPath = _assetService.GetOutputTestPath(TestsOutput.FestivalPathFront);

            reservations = reservations.OrderBy(r => r.Ticket.Event.StartDate).ToList();

            var frontTemplate = await _assetService.GetTemplate(Template.FestivalTicketFront);
            var frontTicket = CreateFestivalFrontOfTicket(frontTemplate, festival, reservations.First());

            // TEST
            await frontTicket.SaveAsPngAsync(outputPath);

            var frontTicketBitmap = await frontTicket.AsBitmap(ImageFormat.JPEG);
            imagesBitmaps.Add(frontTicketBitmap);

            List<Image> reverseOfTickets = [];

            // TEST
            var outputReverseTicketPath = _assetService.GetOutputTestPath(TestsOutput.FestivalPathReverse);

            var reverseTemplate = await _assetService.GetTemplate(Template.FestivalTicketReverse);
            reverseOfTickets = CreateFestivalReverseOfTickets(reverseTemplate, reservations);

            for (int i = 0; i < reverseOfTickets.Count; i++)
            {
                // TEST
                var outputName = $"festival_reverse_test{i + 1}.png";
                var reverseOutputPath = $"{outputReverseTicketPath}{outputName}";
                await reverseOfTickets[i].SaveAsPngAsync(reverseOutputPath);

                var ticketReverse = await reverseOfTickets[i].AsBitmap(ImageFormat.JPEG);
                imagesBitmaps.Add(ticketReverse);
            }
            return imagesBitmaps;
        }

        private Image CreateFestivalFrontOfTicket(Image template, Festival festival, Reservation reservation)
        {
            var titleOptions = _festivalTicketConfig.GetTitlePrintingOptions(festival);
            template.DrawText(festival.Name, titleOptions);

            var dateOptions = _festivalTicketConfig.GetDatePrintingOptions();
            template.DrawText($"{festival.StartDate.ToString(dateOptions.DateFormat)} - " +
                $"{festival.EndDate.ToString(dateOptions.DateFormat)}", dateOptions);

            var qrCodeOptions = _festivalTicketConfig.GetQRCodePrintingOptions();
            var qrCode = _qrCoder.GenerateQRCode(reservation.ReservationGuid.ToString(), qrCodeOptions.Size);
            template.DrawImage(qrCode, qrCodeOptions);

            return template;
        }

        private List<Image> CreateFestivalReverseOfTickets(Image template, List<Reservation> reservations)
        {
            List<Image> reverseOfTickets = [];
            var defaultTemplate = template;

            _festivalTicketConfig.SetDefaultPrintingParams();

            int howManyReverseOfTicket = _festivalTicketConfig.GetReverseTicketCount(reservations.Count);
            int tabsCount = _festivalTicketConfig.TabsCount;

            for (int reverseNr = 0; reverseNr < howManyReverseOfTicket; reverseNr++)
            {
                template = defaultTemplate;

                int startResIndex = reverseNr * tabsCount;

                for (int rIndex = startResIndex; rIndex < reservations.Count; rIndex++)
                {
                    var tabNr = rIndex + 1;

                    Event eventEntity = reservations[rIndex].Ticket.Event;
                    var eventInfo = _festivalTicketConfig.GetEventInfo(eventEntity);
                    var seatsString = _festivalTicketConfig.GetSeatsString(reservations[rIndex]);

                    var tabNrOptions = _festivalTicketConfig.GetTabNumberPrintingOptions();
                    template.DrawText($"{tabNr}.", tabNrOptions);

                    var eventInfoOptions = _festivalTicketConfig.GetEventInfoPrintingOpitons();
                    template.DrawText(eventInfo, eventInfoOptions);

                    var seatsOptions = _festivalTicketConfig.GetSeatsPrintingOpitons();
                    template.DrawText(seatsString, seatsOptions);

                    _festivalTicketConfig.MoveCursorToNextTab(tabNr);

                    if (_festivalTicketConfig.IsNextPageOfReverseTicket())
                    {
                        break;
                    }

                }
                reverseOfTickets.Add(template);
            }
            return reverseOfTickets;
        }


        private void DrawCanvasBorder(Image<Rgba32> canvas)
        {
            var canvasBorderOptions = _hallSeatsConfig.CanvasBorderPrintingOptions;
            canvas.DrawRectangle(canvasBorderOptions);
        }
        private void DrawStage(Image<Rgba32> canvas, Hall hallDetails)
        {
            var stageOptions = _hallSeatsConfig.GetStageRectanglePrintingOptions(hallDetails.StageWidth, hallDetails.StageLength, (float)hallDetails.TotalWidth, (float)hallDetails.TotalLength);
            if (stageOptions is not null)
            {
                canvas.DrawRectangle(stageOptions);
            }
            var stageTextOptions = _hallSeatsConfig.GetStageTextPrintingOptions(hallDetails.StageWidth, hallDetails.StageLength);
            if (stageTextOptions is not null)
            {
                var stageWidth = hallDetails.StageWidth;
                var stageLength = hallDetails.StageLength;
                canvas.DrawText($"SCENA ({stageWidth}m x {stageLength}m)", stageTextOptions);
            }
        }

        private void DrawSeatRowNumber(Image<Rgba32> canvas)
        {
            _hallSeatsConfig.SetSeatCursorToPrintRowNumber();
            var rowNumberOptions = _hallSeatsConfig.RowColNumberPrintingOptions;
            var rowNr = _hallSeatsConfig.RowNr;
            var romanRowNumber = _hallSeatsConfig.ConvertToRoman(rowNr);

            canvas.DrawText(romanRowNumber, rowNumberOptions);

            _hallSeatsConfig.SetSeatCursorAfterPrintRowNumber();
        }
        private void DrawSeatColNumber(Image<Rgba32> canvas)
        {
            _hallSeatsConfig.SetSeatCursorToPrintColNumber();

            var colNr = _hallSeatsConfig.ColNr;
            var romanColNumber = _hallSeatsConfig.ConvertToRoman(colNr);
            var colNumberOptions = _hallSeatsConfig.RowColNumberPrintingOptions;

            canvas.DrawText(romanColNumber, colNumberOptions);

            _hallSeatsConfig.SetSeatCursorAfterPrintColNumber();
        }

        private async Task DrawActiveSeat(Image<Rgba32> canvas, Seat seat)
        {
            Log.Information("\n\n\n\n");
            if (seat == null)
            {
                Log.Error("The 'seat' object is null. Cannot draw the active seat.");
                throw new ArgumentNullException(nameof(seat), "The 'seat' object is null.");
            }

            Log.Information("Drawing seat with ID: {SeatId}, SeatType: {SeatType}, SeatNr: {SeatNr} SeatTypeId: {SeatNr}",
               seat.Id, seat.SeatType, seat.SeatNr, seat.SeatTypeId);

            var seatType = await _unitOfWork.GetRepository<SeatType>().GetOneAsync(seat.SeatTypeId);
            var activeSeatOptions = _hallSeatsConfig.GetActiveSeatRectanglePrintingOptions(seatType);
            canvas.DrawRectangle(activeSeatOptions);

            var seatNumberOptions = _hallSeatsConfig.GetSeatNumberPrintingOptions(seat.SeatNr);
            canvas.DrawText(seat.SeatNr.ToString(), seatNumberOptions);
        }
        private void DrawNonActiveSeat(Image<Rgba32> canvas)
        {
            var nonActiveSeatOptions = _hallSeatsConfig.NonActiveSeatRectanglePrintingOptions;
            canvas.DrawRectangle(nonActiveSeatOptions);
        }

        private async Task DrawSeats(Image<Rgba32> canvas, Hall hall)
        {
            _hallSeatsConfig.PrepareToPrintSeats(hall.MaxNumberOfSeatsColumns);

            var seatsInHall = hall.Seats
                                .OrderBy(s => s.GridRow)
                                .ThenBy(s => s.GridColumn)
                                .ToDictionary(s => (s.GridRow, s.GridColumn), s => s);
            foreach(var seat in seatsInHall.Values)
            {
                Log.Information("test: {SeatId}, SeatType: {SeatType}, SeatNr: {SeatNr}",
    seat.Id, seat.SeatType, seat.SeatNr);
            }



            for (int gridRow = 1; gridRow <= hall.MaxNumberOfSeatsRows; gridRow++)
            {
                _hallSeatsConfig.SetSeatCursorXDefault();
                DrawSeatRowNumber(canvas);
                /*if (seatsInHall.Any(s => s.Key.GridRow == gridRow))
                {

                }*/

                for (int gridCol = 1; gridCol <= hall.MaxNumberOfSeatsColumns; gridCol++)
                {
                    if (gridRow == 1) DrawSeatColNumber(canvas);

                    var seat = seatsInHall.FirstOrDefault(s => s.Key.GridRow == gridRow && s.Key.GridColumn == gridCol).Value;

                    if (seat is not null)
                    {
                        await DrawActiveSeat(canvas, seat);
                    }
                    else
                    {
                        DrawNonActiveSeat(canvas);
                    }
                    _hallSeatsConfig.SetCursorToPrintNextSeatCol();
                }
                _hallSeatsConfig.SetCursorToPrintNextSeatsRow();
            }
        }

        private async Task DrawLegend(Image<Rgba32> canvas, Hall hall)
        {
            var legendHeaderOptions = _hallSeatsConfig.GetLegendHeaderPrintingOptions();
            canvas.DrawText("Legenda:", legendHeaderOptions);

            var nonActiveSeatBlockOptions = _hallSeatsConfig.GetNonActiveSeatColorBlockPrintingOptions();
            canvas.DrawRectangle(nonActiveSeatBlockOptions);

            var legendItemOptions = _hallSeatsConfig.GetLegendItemDescription();
            canvas.DrawText("- nieaktywne miejsce", legendItemOptions);

            var seatsInHall = hall.Seats
                            .OrderBy(s => s.GridRow)
                            .ThenBy(s => s.GridColumn)
                            .ToDictionary(s => (s.GridRow, s.GridColumn), s => s);

            var seatsinHallSeatTypeIds = seatsInHall.Select(s => s.Value.SeatTypeId);
            var seatTypes = await _unitOfWork.GetRepository<SeatType>().GetAllAsync(q => q.Where(st => seatsinHallSeatTypeIds.Contains(st.Id)));

            //var seatTypes = seatsInHall.Select(x => x.Value.SeatType).DistinctBy(x => x.Name);

            foreach (var seatType in seatTypes)
            {
                var activeSeatBlockOptions = _hallSeatsConfig.GetActiveSeatColorBlockPrintingOptions(seatType);
                canvas.DrawRectangle(activeSeatBlockOptions);

                legendItemOptions = _hallSeatsConfig.GetLegendItemDescription();
                canvas.DrawText($"- aktywne miejsce typu {seatType.Name}", legendItemOptions);
            }
        }
        private void DrawWatermark(Image<Rgba32> canvas, Image logo)
        {
            _hallSeatsConfig.CreateWatermark();
            var watermarkOptions = _hallSeatsConfig.GetWaterMarkPrintingOptions(logo);
            var resizedLogo = _hallSeatsConfig.ResizedLogo;
            canvas.DrawImage(resizedLogo!, watermarkOptions);
        }
    }
}
