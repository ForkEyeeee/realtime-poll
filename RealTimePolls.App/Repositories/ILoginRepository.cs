using Microsoft.AspNetCore.Authentication;

namespace RealTimePolls.Repositories
{
    public interface ILoginRepository
    {
        Task GoogleResponse(AuthenticateResult result);
    }
}
