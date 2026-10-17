using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfTextOptions;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EventFlowAPI.Logic.Helpers.PdfOptions.PdfInfoOptions
{
    public class EventPassInfoOptions(EventPass eventPass, EventPassType? oldEventPassType) : InfoOptions
    {
        private readonly EventPass _eventPass = eventPass;
        private readonly EventPassType? _oldEventPassType = oldEventPassType;

        protected override string OrderIdLabel => "Numer karnetu:";
        protected override string OrderId => $"{_eventPass.Id}";
        protected override string DateLabel => "Zakup dokonany:";
        protected override string DateOfOrder => $"{_eventPass.StartDate.ToString(DateFormat.DateTimeFullMonth)}";
        protected override string TypeOfPayment => $"{_eventPass.PaymentType.ToResponse().Name}";
        protected sealed override string UserName => $"{_eventPass.User.Name}";
        protected sealed override string UserSurname => $"{_eventPass.User.Surname}";
        protected sealed override string UserEmail => $"{_eventPass.User.Email}";

        public TextOptions OldEventPassType => new TextOptions
        {
            Text = $"Poprzedni typ karnetu: {(_oldEventPassType != null ? _oldEventPassType.Name : "Brak")}",
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };

        public TextOptions EventPassType => new TextOptions
        {
            Text = $"Typ karnetu: {_eventPass.PassType.Name}",
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };
        public TextOptions EventPassRenew => new TextOptions
        {
            Text = $"Data przedłużenia: {(_eventPass.RenewalDate != null ? _eventPass.RenewalDate.Value.ToString(DateFormat.DateTimeFullMonth) : "Nie przedłużono")}",
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };
    }
}
