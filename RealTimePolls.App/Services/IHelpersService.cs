using Microsoft.AspNetCore.Authentication;

namespace RealTimePolls.Repositories
{
    public interface IHelpersService
    {
        Task<int> GetUserId(AuthenticateResult result);

        Task SendMessage();
    }
}
