using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfTextOptions;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EventFlowAPI.Logic.Helpers.PdfOptions.PdfInfoOptions
{
    public class ReservationInfoOptions(Reservation reservation) : InfoOptions
    {
        private readonly Reservation _reservation = reservation;    

        protected override string OrderIdLabel => "Numer rezerwacji";
        protected sealed override string OrderId => $"{_reservation.Id}";
        protected override string DateLabel => "Rezerwacja dokonana:";
        protected sealed override string DateOfOrder => $"{_reservation.ReservationDate.ToString(DateFormat.DateTimeFullMonth)}";
        protected sealed override string TypeOfPayment => $"{_reservation.PaymentType.ToResponse().Name}";
        protected sealed override string UserName => $"{_reservation.User.Name}";
        protected sealed override string UserSurname => $"{_reservation.User.Surname}";
        protected sealed override string UserEmail => $"{_reservation.User.Email}";   

        public TextOptions TicketType => new TextOptions
        {
            Text = $"Typ biletu: {_reservation.Ticket.TicketType.Name}",
            PaddingBottom = 2.5f,
            Style = TextStyle.Default.FontFamily(defaultFontType).FontSize(11f)
        };
    }
}
