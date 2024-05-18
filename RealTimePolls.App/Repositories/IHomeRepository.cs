using Microsoft.AspNetCore.Mvc;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public interface IHomeRepository
    {
        Task<List<Poll>> Index();
        Task<IActionResult> GetPolls();
        Task<IActionResult> GetDropdownList();
        Task<string> GetUserProfilePicture();
    }
}
