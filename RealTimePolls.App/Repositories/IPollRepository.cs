using Microsoft.AspNetCore.Mvc;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public interface IPollRepository {
    
    Task<PollViewModelDomain> GetPollAsync(string pollTitle, int pollTd, int userId);
    }
}
