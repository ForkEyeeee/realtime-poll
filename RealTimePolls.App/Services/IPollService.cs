using Microsoft.AspNetCore.Authentication;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public interface IPollService
    {
        Task<PollViewModel> Index(string pollTitle, int pollTd, int userId, int currentUserId);
        Task<PollDto> CreatePollAsync(AddPollRequest addPollRequest);
        Task DeletePollAsync(int pollId);
        Task<UserPoll> VoteAsync(AuthenticateResult result, AddVoteRequest addVoteRequest);
    }
}
