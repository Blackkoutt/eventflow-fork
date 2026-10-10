using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.ResultObject;

namespace EventFlowAPI.Logic.Identity.Services.Interfaces.BaseInterfaces
{
    public interface IBaseAuthService
    {
        Result<string> GetCurrentUserId();
        IList<string>? GetRolesForCurrentUser(User user);
    }
}
