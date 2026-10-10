using EventFlowAPI.Logic.Response;
using EventFlowAPI.Logic.Response.Abstract;

namespace EventFlowAPI.Logic.Errors
{
    public sealed record Error(HttpResponse? Details = null)
    {
        public static readonly Error None = new();
        public static readonly Error NullParameter = new(new BadRequestResponse("Parametr w ciele żądania jest null."));
        public static readonly Error RouteParamOutOfRange = new(new BadRequestResponse("Parametr w ścieżce jest poza zakresem."));
        public static readonly Error QueryParamOutOfRange = new(new BadRequestResponse("Parametr zapytania jest poza zakresem."));
        public static readonly Error NotFound = new(new NotFoundResponse("Encja o podanym ID nie istnieje w bazie danych."));
        public static readonly Error SuchEntityExistInDb = new(new ConflictResponse("Encja o podanej nazwie już istnieje w bazie danych."));
        public static readonly Error EntityIsExpired = new(new BadRequestResponse("Nie można wykonać operacji, ponieważ encja wygasła."));
        public static readonly Error EntityIsDeleted = new(new BadRequestResponse("Nie można wykonać operacji, ponieważ encja została usunięta wcześniej."));
        public static readonly Error BadRequestType = new(new BadRequestResponse("Niepoprawny typ żądania."));
        public static readonly Error BadPhotoFileExtension = new(new BadRequestResponse("Niepoprawne rozszerzenie pliku. Spróbuj przesłać plik w formacie jpeg lub png."));
    }
}
