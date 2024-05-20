using Microsoft.AspNetCore.Mvc;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public interface IPollRepository {
    
        Task<PollViewModelDomain> GetPollAsync(string pollTitle, int pollTd, int userId);
        Task<Poll> CreatePollAsync(Poll poll);

    }
}
