using Microsoft.AspNetCore.Authentication;

namespace RealTimePolls.Repositories
{
    public interface IAuthRepository
    {
        Task GoogleResponse(AuthenticateResult result);
    }
}
