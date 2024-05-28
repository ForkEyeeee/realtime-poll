using RealTimePolls.Models.ViewModels;


namespace RealTimePolls.Repositories
{
    public interface IHomeService
    {
        Task<HomeViewModel> Index();
    }
}
