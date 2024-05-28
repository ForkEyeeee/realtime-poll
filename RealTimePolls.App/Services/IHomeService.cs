using RealTimePolls.Models.Domain;


namespace RealTimePolls.Repositories
{
    public interface IHomeService
    {
        Task<List<Poll>> Index();
    }
}
