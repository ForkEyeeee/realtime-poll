using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;

namespace RealTimePolls.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHomeRepository homeRepository;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(
            IHomeRepository homeRepository,
            IMapper mapper
        )
        {
            this.homeRepository = homeRepository;
            this.mapper = mapper;
        }

        // Get home view
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var domainPolls = await homeRepository.Index();

            var homeViewModel = new List<HomeViewModel>();

            foreach (var poll in domainPolls)
            {
                homeViewModel.Add(mapper.Map<HomeViewModel>(poll));
            }

            return View(homeViewModel);
        }


        //[HttpGet]
        //public async Task<string> GetUserProfilePicture()
        //{
        //    AuthenticateResult result = await HttpContext.AuthenticateAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme
        //    );

        //    var profilePicture = await homeRepository.GetUserProfilePicture(result);

        //    return profilePicture;
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }
    }

    //[HttpPost]
    //public IActionResult Poll(string pollName)
    //{
    //    try
    //    {
    //        Poll poll = _context.Polls.FirstOrDefault(u => u.Title == pollName);

    //        if (poll == null)
    //            throw new Exception("Poll cannot be found");

    //        int firstVoteCount = _context.UserPoll.Count(up =>
    //                up.PollId == poll.Id && up.Vote == true
    //            ),
    //            secondVoteCount = _context.UserPoll.Count(up =>
    //                up.PollId == poll.Id && up.Vote == true
    //            );

    //        var viewModel = _context
    //            .Polls.Select(p => new PollItem
    //            {
    //                Poll = poll,
    //                FirstVoteCount = firstVoteCount,
    //                SecondVoteCount = secondVoteCount
    //            })
    //            .ToList();

    //        return View("index", viewModel);
    //    }
    //    catch (Exception e)
    //    {
    //        var errorViewModel = new ErrorViewModel { RequestId = e.Message };
    //        return View("Error", errorViewModel);
    //    }
    //}

    //[HttpPost]
    //public async Task<IActionResult> GetRawContent()
    //{
    //    try
    //    {
    //        string rawContent = string.Empty;
    //        using (
    //            var reader = new StreamReader(
    //                Request.Body,
    //                encoding: Encoding.UTF8,
    //                detectEncodingFromByteOrderMarks: false
    //            )
    //        )
    //        {
    //            rawContent = await reader.ReadToEndAsync();
    //        }
    //        return Ok(rawContent);
    //    }
    //    catch (Exception e)
    //    {
    //        var errorViewModel = new ErrorViewModel { RequestId = e.Message };
    //        return View("Error", errorViewModel);
    //    }
    //}

    //}
}