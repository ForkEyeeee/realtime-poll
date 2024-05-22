using Microsoft.AspNetCore.Authentication;
using RealTimePolls.Models.Domain;

namespace RealTimePolls.Repositories
{
    public interface IPollsApiRepository
    {
        Task<List<Poll>> GetPollsListAsync();

        Task<List<Genre>> GetDropdownListAsync();

        Task<string> GetUserProfilePicture(AuthenticateResult result);

        Task<List<Poll>> GetSearchResults(string query);

        Task<List<Poll>> GetGenreResults(int genreId);

    }
}
