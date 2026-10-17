using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Helpers.JpgOptions;
using EventFlowAPI.Logic.Helpers.TicketOptions;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces.Configuration.HallConfiguration;
using SixLabors.ImageSharp;
using System.Security.Cryptography;

namespace EventFlowAPI.Logic.Services.OtherServices.Services.Configuration.HallConfiguration
{
    public class HallSeatsConfiguration : IHallSeatsConfiguration
    {
        private StageConfiguration _stageConfiguration;
        private SeatsConfiguration _seatsConfiguration;
        private LegendConfiguration _legendConfiguration;
        private WatermarkConfiguration? _watermarkConfiguration;
        private CanvasBorderConfiguration _canvasBorderConfiguration;

        // Canvas
        public Color CanvasBackgroundColor => Color.White;
        private const int paddingFromCanvasBorder = 30;
        private float canvasInnerWidth;
        private float canvasInnerHeight;
        public int canvasWidth;
        private int canvasHeight;
        public int CanvasWidth => canvasWidth;
        public int CanvasHeight => canvasHeight;     

        // Painting
        private float paintingAreaWidth;
        private float paintingAreaHeight;
        private float startPointX;
        private float startPointY;

        // Canvas Border
        private float canvasBorderThickness;

        public HallSeatsConfiguration(IAssetService assetService)
        {
            _stageConfiguration = new StageConfiguration(assetService);
            _seatsConfiguration = new SeatsConfiguration(assetService);
            _legendConfiguration = new LegendConfiguration(assetService);
            _canvasBorderConfiguration = new CanvasBorderConfiguration();
            canvasBorderThickness = _canvasBorderConfiguration.CanvasBorderThickness;
            
        }

        public void CreateWatermark()
        {
            _watermarkConfiguration = new WatermarkConfiguration(
                              CanvasWidth,
                              canvasBorderThickness,
                              paddingFromCanvasBorder,
                              _legendConfiguration.FooterPadding,
                              _legendConfiguration.LegendY,
                              _legendConfiguration.LegendTextHeight,
                              _legendConfiguration.LegendTextPadding);
        }

        public void SetCanvasDimensions(Hall hall, Image logo, bool isDefault = false)
        {
            canvasWidth = 1800;  
            if (isDefault)
            {
                canvasHeight = 2000;
            }
            else
            {
                var borderTickness = _canvasBorderConfiguration.CanvasBorderThickness;
                var borderAndPaddingLength = 2 * paddingFromCanvasBorder + 2 * borderTickness;
                var seatHeight = _seatsConfiguration.SeatHeight;
                var rowPadding = _seatsConfiguration.RowPadding;
                var seatRows = hall.MaxNumberOfSeatsRows;
                var rowNumberHeight = _seatsConfiguration.RowColNumberHeight + 10;

                var legendTextHeight = _legendConfiguration.LegendTextHeight;
                var legendItemPadding = _legendConfiguration.LegendItemPadding;
                var seatTypesCount = hall.Seats.DistinctBy(s => s.SeatTypeId).Count();
                var totalLegendHeight = (seatTypesCount + 2) * legendTextHeight + (seatTypesCount + 1) * legendItemPadding;

                var logoScaleRatio = 0.35;
                var logoHeight = (int)(logo.Height * logoScaleRatio);

                int biggerFooterItem;
                if (logoHeight > totalLegendHeight) biggerFooterItem = logoHeight;
                else biggerFooterItem = totalLegendHeight;

                var topSeatsPadding = _stageConfiguration.PaddingTopSeats;
                var footerPadding = _legendConfiguration.FooterPaddingTop;

                var totalSeatsHeight = seatHeight * seatRows + rowPadding * (seatRows - 1) + rowNumberHeight;

                var totalPrintingAreaHeight = biggerFooterItem + topSeatsPadding + footerPadding + totalSeatsHeight;

                if (hall.StageLength is not null)
                {
                    var stageToHallLengthRatio = (float)hall.StageLength! / (float)hall.TotalLength;
                    var seatsAndOtherItemsToStageRatio = 1 - stageToHallLengthRatio;

                    var onePercentLength = totalPrintingAreaHeight / (seatsAndOtherItemsToStageRatio * 100);
                    totalPrintingAreaHeight = (int)(onePercentLength * 100);
                }
                canvasHeight = (int)(totalPrintingAreaHeight + borderAndPaddingLength);
            }           
            canvasInnerWidth = CanvasWidth - (2 * canvasBorderThickness);
            canvasInnerHeight = CanvasHeight - (2 * canvasBorderThickness);
            paintingAreaWidth = canvasInnerWidth - (2 * paddingFromCanvasBorder);
            paintingAreaHeight = canvasInnerHeight - (2 * paddingFromCanvasBorder);
            startPointX = canvasBorderThickness + paddingFromCanvasBorder;
            startPointY = canvasBorderThickness + paddingFromCanvasBorder;
        }

        public OutlineRectanglePrintingOptions CanvasBorderPrintingOptions => 
            _canvasBorderConfiguration.GetCanvasBorderPrintingOptions(CanvasWidth, CanvasHeight);




        // Stage
        public OutlineRectanglePrintingOptions? GetStageRectanglePrintingOptions(float? stageWidth, float? stageLength, float hallWidth, float hallLength)
        {
            return _stageConfiguration
                        .GetStageRectanglePrintingOptions(
                            new PointF(startPointX, startPointY),
                            new SizeF(paintingAreaWidth, paintingAreaHeight),
                            stageWidth,
                            stageLength,
                            hallWidth,
                            hallLength);
        }
        public PrintingOptions? GetStageTextPrintingOptions(float? stageWidth, float? stageLength)
        {
            return _stageConfiguration.GetStageTextPrintingOptions(ref startPointY, stageWidth, stageLength);
        }



        // Seats
        public void PrepareToPrintSeats(int maxColumns)
        {
            _seatsConfiguration.PrepareToPrintSeats(
                new PointF(startPointX, startPointY),
                new SizeF(paintingAreaWidth, paintingAreaHeight),
                maxColumns);
        }
        public void SetSeatCursorXDefault() => _seatsConfiguration.SetSeatCursorXDefault();
        public void SetSeatCursorToPrintRowNumber() => _seatsConfiguration.SetSeatCursorToPrintRowNumber();
        public PrintingOptions RowColNumberPrintingOptions => _seatsConfiguration.RowColNumberPrintingOptions;
        public void SetSeatCursorAfterPrintRowNumber() => _seatsConfiguration.SetSeatCursorAfterPrintRowNumber();
        public void SetSeatCursorToPrintColNumber() => _seatsConfiguration.SetSeatCursorToPrintColNumber();
        public void SetSeatCursorAfterPrintColNumber() => _seatsConfiguration.SetSeatCursorAfterPrintColNumber();
        public void SetCursorToPrintNextSeatCol() => _seatsConfiguration.SetCursorToPrintNextSeatCol();
        public void SetCursorToPrintNextSeatsRow() => _seatsConfiguration.SetCursorToPrintNextSeatsRow();
        public OutlineRectanglePrintingOptions GetActiveSeatRectanglePrintingOptions(SeatType? seatType) => _seatsConfiguration.GetActiveSeatRectanglePrintingOptions(seatType);
        public FillRectanglePrintingOptions NonActiveSeatRectanglePrintingOptions => _seatsConfiguration.NonActiveSeatRectanglePrintingOptions;
        public int RowNr => _seatsConfiguration.RowNr;
        public int ColNr => _seatsConfiguration.ColNr;
        public PrintingOptions GetSeatNumberPrintingOptions(int seatNr) => _seatsConfiguration.GetSeatNumberPrintingOptions(seatNr);
        public PrintingOptions GetLegendHeaderPrintingOptions()
        {
            var seatY = _seatsConfiguration.SeatY;
            return _legendConfiguration.GetLegendHeaderPrintingOptions(startPointX, seatY);
        }
        public FillRectanglePrintingOptions GetNonActiveSeatColorBlockPrintingOptions()
        {
            var nonActiveSeatColor = _seatsConfiguration.NonActiveSeatColor;
            return _legendConfiguration.GetNonActiveSeatColorBlockPrintingOptions(nonActiveSeatColor);
        }
        public FillRectanglePrintingOptions GetActiveSeatColorBlockPrintingOptions(SeatType seatType) => _legendConfiguration.GetActiveSeatColorBlockPrintingOptions(seatType);
        public PrintingOptions GetLegendItemDescription() => _legendConfiguration.GetLegendItemDescription();




        // Watermark 
        public ImagePrintingOptions GetWaterMarkPrintingOptions(Image logo) => _watermarkConfiguration!.GetWaterMarkPrintingOptions(logo);
        public Image? ResizedLogo => _watermarkConfiguration!.ResizedLogo;



        // Helper
        public string ConvertToRoman(int number)
        {
            if ((number < 1) || (number > 3999)) return "";
            string[] thou = { "", "M", "MM", "MMM" };
            string[] hund = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
            string[] tens = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
            string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
            return thou[number / 1000] + hund[(number % 1000) / 100] + tens[(number % 100) / 10] + ones[number % 10];
        }       
    }
}
