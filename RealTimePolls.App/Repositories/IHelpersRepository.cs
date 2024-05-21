using Microsoft.AspNetCore.Authentication;

namespace RealTimePolls.Repositories
{
    public interface IHelpersRepository
    {
        Task<int> GetUserId(AuthenticateResult result);
    }
}
