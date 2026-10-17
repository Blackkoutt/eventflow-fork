using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Helpers.TicketOptions;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces.Configuration.TicketConfiguration;
using EventFlowAPI.Logic.Services.OtherServices.Services.Configuration.TicketConfiguration.Abstract;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace EventFlowAPI.Logic.Services.OtherServices.Services.Configuration.TicketConfiguration
{
    public class EventTicketConfiguration(IAssetService assetService) :
        TicketConfiguration<Event>(assetService),
        IEventTicketConfiguration
    {

        private int detailsPrintX = 0;
        private int detailsPrintY = 546;

        // Ticket Tittle Settings 
        protected sealed override short TitleFirstLineY => 230;
        protected sealed override short TitleLineHeight => 20;
        protected sealed override short TitleLeftMax => 43;
        protected sealed override short TitleRightMax => 748;
        protected sealed override short TitleEstimatedCharWidth => 24;
        protected sealed override short TitleEstimatedMaxCharHeight => 35;
        protected sealed override Font TitleFont => _assetService.GetFont(48, FontStyle.Bold, FontType.Inter);
        protected sealed override Color TitleColor => Color.Black;


        // Ticket Date Settings 
        protected sealed override Font DateFont => _assetService.GetFont(40, FontStyle.Bold, FontType.Inter);
        protected sealed override Color DateColor => Color.Black;
        protected sealed override PointF DateLocation => new(x: 230, y: 426);
        protected sealed override string FormatDate => DateFormat.DateTime;


        // Ticket Qr Code Settings 
        protected sealed override byte QRCodeSize => 9;
        protected sealed override float QRCodeOpacity => 1f;
        protected sealed override Point QRCodeLocation => new(x: 1607, y: 132);


        // Ticket Price Settings
        private const short priceLeftMax = 73;
        private const short priceRightMax = 256;
        private const short priceEstimatedCharWidth = 12;
        private readonly Color priceColor = Color.Black;
        private const Currency priceCurrency = Currency.PLN;
        private PointF PriceLocation => new(x: detailsPrintX, y: detailsPrintY);
        private Font PriceFont => _assetService.GetFont(25, FontStyle.Bold, FontType.Inter);


        // Ticket Hall Settings   
        private readonly Color hallColor = Color.Black;
        private Font HallFont => _assetService.GetFont(25, FontStyle.Bold, FontType.Inter);
        private PointF HallLocation => new(x: 371, y: detailsPrintY);


        // Ticket Duration Settings
        private const short durationLeftMax = 507;
        private const short durationRightMax = 717;
        private const short durationEstimatedCharWidth = 12;
        private readonly Color DurationColor = Color.Black;
        private Font DurationFont => _assetService.GetFont(25, FontStyle.Bold, FontType.Inter);
        private PointF DurationLocation => new(x: detailsPrintX, y: detailsPrintY);


        // Ticket Seats Settings
        private readonly Color SeatsColor = Color.Black;
        private Font SeatsFont => _assetService.GetFont(32, FontStyle.Bold, FontType.Inter);
        private PointF SeatsLocation => new(x: 865, y: 85);

        public void SetDefaultPrintingParams()
        {
                detailsPrintX = 0;
                detailsPrintY = 546;
        }


        protected override string GetTitle(Event entity) => $"{entity.Category.Name}: {entity.Name}";

        public PricePrintingOptions GetPricePrintingOptions(Reservation reservation)
        {
            int maxSpaceForText = priceRightMax - priceLeftMax;
            int priceStringLength = reservation.Ticket.Price.ToString().Length + 3;
            detailsPrintX = priceLeftMax + (maxSpaceForText - priceStringLength * priceEstimatedCharWidth) / 2;

            return new PricePrintingOptions
            {
                Font = PriceFont,
                BrushColor = priceColor,
                Location = PriceLocation,
                Currency = priceCurrency
            };
        }


        public PrintingOptions GetHallPrintingOptions()
        {
            return new PrintingOptions
            {
                Font = HallFont,
                BrushColor = hallColor,
                Location = HallLocation
            };
        }

        public PrintingOptions GetDurationPrintingOpitons(Event eventEntity)
        {
            int spaceForDuration = durationRightMax - durationLeftMax;
            int durationStringLength = eventEntity.Duration.TotalMinutes.ToString().Length + 4;
            detailsPrintX = durationLeftMax + (spaceForDuration - durationStringLength * durationEstimatedCharWidth) / 2;

            return new PrintingOptions
            {
                Font = DurationFont,
                BrushColor = DurationColor,
                Location = DurationLocation
            };
        }

        public PrintingOptions GetSeatsPrintingOptions()
        {
            return new PrintingOptions
            {
                Font = SeatsFont,
                BrushColor = SeatsColor,
                Location = SeatsLocation
            };
        }
    }
}
