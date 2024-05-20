using RealTimePolls.Models.Domain;

namespace RealTimePolls.Repositories
{
    public interface IPollsApiRepository
    {
        Task<List<Poll>> GetPollsListAsync();

        Task<List<Genre>> GetDropdownListAsync();
    }
}
