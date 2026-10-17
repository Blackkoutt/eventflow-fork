using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfTextOptions;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EventFlowAPI.Logic.Helpers.PdfOptions.PdfContentOptions
{
    public class ContentHallRentOptions(HallRent hallRent)
    {
        private readonly HallRent _hallRent = hallRent;
        private readonly string defaultFontType = FontType.Inter.ToString();
        public float PadLeft => 10f;

        public LabelValueTextOptions HallNr => new LabelValueTextOptions
        {
            Label = "Wynajmowana sala: ",
            LabelStyle = TextStyle.Default.FontFamily(defaultFontType).SemiBold().FontSize(12f),
            Value = $"nr {_hallRent.Hall.HallNr}, piętro {_hallRent.Hall.Floor}",
            ValueStyle = TextStyle.Default.FontFamily(defaultFontType).FontSize(12f),
            Space = 5f,
            PaddingBottom = 2.5f
        };
        public LabelValueTextOptions StartDate => new LabelValueTextOptions
        {
            Label = "Data rozpoczęcia wynajmu: ",
            LabelStyle = TextStyle.Default.FontFamily(defaultFontType).SemiBold().FontSize(12f),
            Value = $"{_hallRent.StartDate.ToString(DateFormat.DateTimeFullDayAndMonth)}",
            ValueStyle = TextStyle.Default.FontFamily(defaultFontType).FontSize(12f),
            Space = 5f,
            PaddingBottom = 2.5f
        };
        public LabelValueTextOptions EndDate => new LabelValueTextOptions
        {
            Label = "Data zakończenia wynajmu: ",
            LabelStyle = TextStyle.Default.FontFamily(defaultFontType).SemiBold().FontSize(12f),
            Value = $"{_hallRent.EndDate.ToString(DateFormat.DateTimeFullDayAndMonth)}",
            ValueStyle = TextStyle.Default.FontFamily(defaultFontType).FontSize(12f),
            Space = 5f,
            PaddingBottom = 10f
        };
        public LabelValueTextOptions HallType => new LabelValueTextOptions
        {
            Label = "Typ sali: ",
            LabelStyle = TextStyle.Default.FontFamily(defaultFontType).SemiBold().FontSize(12f),
            Value = $"{_hallRent.Hall.Type.Name}",
            ValueStyle = TextStyle.Default.FontFamily(defaultFontType).FontSize(12f),
            Space = 5f,
            PaddingTop = 10f,
            PaddingBottom = 2.5f
        };
        public LabelValueTextOptions Equipment => new LabelValueTextOptions
        {
            Label = "Wyposażenie: ",
            LabelStyle = TextStyle.Default.FontFamily(defaultFontType).SemiBold().FontSize(12f),
            Value = $"{string.Join(", ", _hallRent.Hall.Type.Equipments.Select(e => e.Name))}",
            ValueStyle = TextStyle.Default.FontFamily(defaultFontType).FontSize(12f),
            Space = 5f,
        };

        public LabelValueTextOptions RentDuration => new LabelValueTextOptions
        {
            Label = "Czas trwania wynajmu: ",
            LabelStyle = TextStyle.Default.FontFamily(defaultFontType).SemiBold().FontSize(11f),
            Value = $"{Math.Ceiling(_hallRent.Duration.TotalHours)} h",
            ValueStyle = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f),
            Space = 5f,
            PaddingBottom = 2.5f            
        };
        public LabelValueTextOptions HallSeatsCount => new LabelValueTextOptions
        {
            Label = "Ilość miejsc: ",
            LabelStyle = TextStyle.Default.FontFamily(defaultFontType).SemiBold().FontSize(11f),
            Value = $"{_hallRent.Hall.Seats.Count}",
            ValueStyle = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f),
            Space = 5f,
            PaddingBottom = 2.5f
        };
        public LabelValueTextOptions HallMaxSeatsCount => new LabelValueTextOptions
        {
            Label = "Maksymalna ilość miejsc: ",
            LabelStyle = TextStyle.Default.FontFamily(defaultFontType).SemiBold().FontSize(11f),
            Value = $"{_hallRent.Hall.HallDetails!.MaxNumberOfSeats}",
            ValueStyle = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f),
            Space = 5f,
        };


        public LabelValueTextOptions HallDimensions => new LabelValueTextOptions
        {
            Label = "Wymiary sali: ",
            LabelStyle = TextStyle.Default.FontFamily(defaultFontType).SemiBold().FontSize(11f),
            Value = $"{_hallRent.Hall.HallDetails!.TotalWidth} m x {_hallRent.Hall.HallDetails!.TotalLength} m",
            ValueStyle = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f),
            Space = 5f,
            PaddingBottom = 2.5f
        };
        public LabelValueTextOptions HallArea => new LabelValueTextOptions
        {
            Label = "Powierzchnia sali: ",
            LabelStyle = TextStyle.Default.FontFamily(defaultFontType).SemiBold().FontSize(11f),
            Value = $"{_hallRent.Hall.HallDetails!.TotalArea} m²",
            ValueStyle = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f),
            Space = 5f,
            PaddingBottom = 2.5f
        };
        public LabelValueTextOptions StageArea() 
        {
            var stageLength = _hallRent.Hall.HallDetails!.StageLength;
            var stageWidth = _hallRent.Hall.HallDetails.StageWidth;
            decimal stageArea = 0m;
            if (stageLength is not null && stageWidth is not null)
                stageArea = Math.Round((decimal)(stageWidth * stageLength), 2);

            return new LabelValueTextOptions
            {
                Label = "Powierzchnia sceny: ",
                LabelStyle = TextStyle.Default.FontFamily(defaultFontType).SemiBold().FontSize(11f),
                Value = $"{(stageArea != 0m ? stageArea + " m²" : "Brak")}",
                ValueStyle = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f),
                Space = 5f,
            };
        }

    }
}
