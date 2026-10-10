using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.ResultObject;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace EventFlowAPI.Logic.Helpers
{
    public static class SessionRequestHelper
    {
        // Reads a request stored in the session by a "create transaction" step and removes it
        public static Result<TRequestDto> TakeRequest<TRequestDto>(IHttpContextAccessor httpContextAccessor, string key) where TRequestDto : class
        {
            var session = httpContextAccessor.HttpContext!.Session;
            var requestString = session.GetString(key);
            if (requestString == null)
                return Result<TRequestDto>.Failure(Error.SessionError);
            session.Remove(key);

            var requestDto = JsonSerializer.Deserialize<TRequestDto>(requestString);
            if (requestDto == null)
                return Result<TRequestDto>.Failure(Error.SerializationError);

            return Result<TRequestDto>.Success(requestDto);
        }
    }
}
