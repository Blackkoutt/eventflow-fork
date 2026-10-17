using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfTextOptions;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EventFlowAPI.Logic.Helpers.PdfOptions.PdfInfoOptions
{
    public class HallViewInfoOptions(Hall hall, HallRent? hallRent = null, Event? eventEntity = null)
    {
        private readonly string defaultFontType = FontType.Inter.ToString();
        public float PadLeft => 10f;
        public float PadTop => 10f;
        public float PadBottom => 10f;

        public HallRent? HallRent => hallRent;
        public Event? Event => eventEntity;

        public TextOptions HallNr => new TextOptions
        {
            Text = $"Sala: nr {hall.HallNr}, piętro {hall.Floor}",
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f).SemiBold()
        };
        public TextOptions HallType => new TextOptions
        {
            Text = $"Typ sali: {hall.Type.Name}",
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };
        public TextOptions HallSeatsCount => new TextOptions
        {
            Text = $"Ilość miejsc: {hall.Seats.Count} / {hall.MaxNumberOfSeats}",
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };

        public TextOptions HallDimensions => new TextOptions
        {
            Text = $"Wymiary sali: {hall.TotalWidth} m x {hall.TotalLength} m",
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };
        public TextOptions HallStageArea()
        {
            var stageLength = hall.StageLength;
            var stageWidth = hall.StageWidth;
            decimal stageArea = 0m;
            if (stageLength is not null && stageWidth is not null)
                stageArea = Math.Round((decimal)(stageWidth * stageLength), 2);

            return new TextOptions
            {
                Text = $"Powierzchnia (sceny / sali): {(stageArea != 0m ? stageArea + " m²" : "Brak")} / {hall.TotalArea} m²",
                PaddingBottom = 0f,
                Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
            };
        }


        // If HallRent is not null
        public TextOptions HallRentOrderNumber => new TextOptions
        {
            Text = $"Numer wynajmu: {hallRent?.Id}",
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f).SemiBold()
        };

        public TextOptions HallRentOrderDate => new TextOptions
        {
            Text = $"Wynajem dokonany: {hallRent?.RentDate.ToString(DateFormat.DateTime)}",
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };

        public TextOptions HallRentOrderingPerson => new TextOptions
        {
            Text = GetOrderingPerson(),
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };

        public TextOptions HallRentStartDate => new TextOptions
        {
            Text = $"Data rozpoczęcia wynajmu: {hallRent?.StartDate.ToString(DateFormat.DateTime)}",
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };
        public TextOptions HallRentEndDate => new TextOptions
        {
            Text = $"Data zakończenia wynajmu: {hallRent?.EndDate.ToString(DateFormat.DateTime)}",
            PaddingBottom = 0f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };



        // If EventEntity is not null
        public TextOptions EventCategory => new TextOptions
        {
            Text = $"{eventEntity?.Category}",
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f),
            PaddingBottom = 2.5f
        };
        public TextOptions EventName => new TextOptions
        {
            Text = $"{eventEntity?.Name}",
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f).SemiBold(),
            PaddingBottom = 2.5f
        };
        public TextOptions EventStartDate => new TextOptions
        {
            Text = $"Data rozpoczęcia wydarzenia: {eventEntity?.StartDate.ToString(DateFormat.DateTime)}",
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };
        public TextOptions EventEndDate => new TextOptions
        {
            Text = $"Data zakończenia wydarzenia: {eventEntity?.EndDate.ToString(DateFormat.DateTime)}",
            PaddingBottom = 0f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };

        private string GetOrderingPerson()
        {
            if (hallRent == null) return "";

            if (string.IsNullOrEmpty(hallRent.User.Name) || string.IsNullOrEmpty(hallRent.User.Surname))
            {
                return $"Osoba zamawiająca: {hallRent.User.Email}";
            }
            return $"Osoba zamawiająca: {hallRent.User.Name} {hallRent.User.Surname}";
        }
    }
}
