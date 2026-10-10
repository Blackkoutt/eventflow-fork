using EventFlowAPI.Logic.Response;
using EventFlowAPI.Logic.Response.Abstract;

namespace EventFlowAPI.Logic.Errors
{
    public sealed record HallRentError(HttpResponse? Details = null)
    {
        public static readonly Error NotFound = new(new BadRequestResponse("Wynajem sali o podanym ID nie istnieje w bazie danych."));
        public static readonly Error HallRentIsDeleted = new(new BadRequestResponse("Nie można wykonać akcji, ponieważ wynajem sali został anulowany."));
        public static readonly Error HallRentIsExpired = new(new BadRequestResponse("Nie można wykonać akcji, ponieważ wynajem sali wygasł."));
        public static readonly Error CollisionWithExistingEvent = new(new BadRequestResponse("Wynajem sali koliduje z innym istniejącym wydarzeniem. Zmień salę lub daty rozpoczęcia/zakończenia wynajmu."));
        public static readonly Error CollisionWithExistingHallRent = new(new BadRequestResponse("Wynajem sali koliduje z innym istniejącym wynajmem sali. Zmień salę lub daty rozpoczęcia/zakończenia wynajmu."));
        public static readonly Error HallNotFound = new(new BadRequestResponse("Sala o podanym ID nie istnieje w bazie danych."));
        public static readonly Error HallRentDoesNotExist = new(new BadRequestResponse("Taki wynajem sali nie istnieje lub został anulowany wcześniej."));
        public static readonly Error TooMuchActiveHallRentsInMonth = new(new BadRequestResponse("Użytkownik ma za dużo aktywnych wynajmów sali w tym miesiącu."));
        public static readonly Error HallRentListIsEmpty = new(new BadRequestResponse("Nie znaleziono wynajmów sali do zaktualizowania."));
    }
}
