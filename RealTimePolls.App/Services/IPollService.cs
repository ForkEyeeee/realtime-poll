using Microsoft.AspNetCore.Authentication;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public interface IPollService
    {
        Task<PollViewModelDomain> GetPollAsync(string pollTitle, int pollTd, int userId);
        Task<Poll> CreatePollAsync(Poll poll);
        Task DeletePollAsync(int pollId);
        Task<UserPoll> VoteAsync(AuthenticateResult result, AddVoteRequest addVoteRequest);
    }
}
