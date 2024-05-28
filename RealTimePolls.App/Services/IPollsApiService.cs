using Microsoft.AspNetCore.Authentication;
using RealTimePolls.App.Models.DTO;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;

namespace RealTimePolls.Repositories
{
    public interface IPollsApiService
    {
        Task<List<PollDto>> GetPollsListAsync();

        Task<List<GenreDto>> GetDropdownListAsync();

        Task<string> GetUserProfilePicture(AuthenticateResult result);

        Task<List<PollDto>> GetSearchResults(string query, int genreId);

    }
}
