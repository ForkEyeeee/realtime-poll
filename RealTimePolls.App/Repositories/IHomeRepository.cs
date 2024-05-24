using RealTimePolls.Models.Domain;


namespace RealTimePolls.Repositories
{
    public interface IHomeRepository
    {
        Task<List<Poll>> Index();
    }
}
