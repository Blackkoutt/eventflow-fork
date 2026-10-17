using EventFlowAPI.Logic.Response;
using EventFlowAPI.Logic.Response.Abstract;

namespace EventFlowAPI.Logic.Errors
{
    public sealed record PartnerError(HttpResponse? Details = null)
    {
        public static readonly Error MediaPatronNotFound = new(new BadRequestResponse("Patron medialny o podanym ID nie istnieje w bazie danych."));
        public static readonly Error OrganizerNotFound = new(new BadRequestResponse("Organizator o podanym ID nie istnieje w bazie danych."));
        public static readonly Error SponsorNotFound = new(new BadRequestResponse("Sponsor o podanym ID nie istnieje w bazie danych."));
    }
}
