using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public interface IHomeRepository
    {
        Task<List<Poll>> Index();
    }
}
