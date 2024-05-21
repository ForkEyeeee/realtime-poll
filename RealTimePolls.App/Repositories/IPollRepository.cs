using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public interface IPollRepository
    {

        Task<PollViewModelDomain> GetPollAsync(string pollTitle, int pollTd, int userId);
        Task<Poll> CreatePollAsync(Poll poll);

        Task<Poll> DeletePollAsync(int pollId);

        Task<UserPoll> VoteAsync(AuthenticateResult result, int userId, int pollId, string vote);
    }
}
