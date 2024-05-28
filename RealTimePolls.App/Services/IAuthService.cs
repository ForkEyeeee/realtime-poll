using Microsoft.AspNetCore.Authentication;

namespace RealTimePolls.Repositories
{
    public interface IAuthService
    {
        Task GoogleResponse(AuthenticateResult result);
    }
}
