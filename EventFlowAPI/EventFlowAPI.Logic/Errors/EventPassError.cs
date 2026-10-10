using EventFlowAPI.Logic.Response;
using EventFlowAPI.Logic.Response.Abstract;

namespace EventFlowAPI.Logic.Errors
{
    public sealed record EventPassError(HttpResponse? Details = null)
    {
        public static readonly Error UserAlreadyHaveActiveEventPass = new(new BadRequestResponse("Użytkownik posiada już aktywny karnet."));
        public static readonly Error UserDoesNotHaveActiveEventPass = new(new BadRequestResponse("Użytkownik nie posiada aktywnego karnetu."));
        public static readonly Error OnlyOneSeatPerEvent = new(new BadRequestResponse("Karnet pozwala na rezerwację tylko jednego miejsca na jedno wydarzenie."));
        public static readonly Error EventPassIsDeleted = new(new BadRequestResponse("Karnet został anulowany."));
        public static readonly Error EventPassIsExpired = new(new BadRequestResponse("Karnet jest przedawniony."));
        public static readonly Error EventPassExpireBeforeEndOfEvent = new(new BadRequestResponse("Nie można dokonać rezerwacji na wydarzenie, ponieważ aktualny karnet wygasa przed zakończeniem wydarzenia."));
        public static readonly Error AdminCannotBuyOrRenewEventPass = new(new ForbiddenResponse("Administrator nie może kupić lub przedłużyć karnetu."));
    }
}
