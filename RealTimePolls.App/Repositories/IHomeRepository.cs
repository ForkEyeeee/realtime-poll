using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public interface IHomeRepository
    {
        Task<List<Poll>> Index();
        Task<IActionResult> GetPolls();
        Task<List<Genre>> GetDropdownList();
        Task<string> GetUserProfilePicture(AuthenticateResult result);
    }
}
