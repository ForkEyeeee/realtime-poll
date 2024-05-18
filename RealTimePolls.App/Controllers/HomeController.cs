using System.Diagnostics;
using System.Text;
using AutoMapper;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;

namespace RealTimePolls.Controllers
{
    public class HomeController : Controller
    {
        private readonly RealTimePollsDbContext dbContext;

        private readonly IWebHostEnvironment environment;
        private readonly IHomeRepository homeRepository;
        private readonly IMapper mapper;

        public HomeController(
            RealTimePollsDbContext dbContext,
            IWebHostEnvironment environment,
            IHomeRepository homeRepository,
            IMapper mapper
        )
        {
            this.dbContext = dbContext;
            this.environment = environment;
            this.homeRepository = homeRepository;
            this.mapper = mapper;
        }

        // Get all polls
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var polls = await homeRepository.Index();

            //var homeViewModel = mapper.Map<HomeViewModel>(polls);

            //int pollCount = dbContext.Polls.Count();
            return View(polls);

            //var polls = new List<Poll>();

            //foreach (var poll in polls)
            //{
            //    polls.Add(poll);
            //}

            //{
            //    Polls = polls,
            //    EnvironmentName = _environment.EnvironmentName
            //};

            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    var profilePicture = await GetUserProfilePicture();
            //    viewModel.UserProfilePicture = profilePicture;
            //}
        }
    }

    //public async Task<string> GetUserProfilePicture()
    //{
    //    var result = await HttpContext.AuthenticateAsync(
    //        CookieAuthenticationDefaults.AuthenticationScheme
    //    );

    //    if (result.Principal == null)
    //        return string.Empty;

    //    var claims = result
    //        .Principal.Identities.FirstOrDefault()
    //        ?.Claims.Select(claim => new
    //        {
    //            claim.Issuer,
    //            claim.OriginalIssuer,
    //            claim.Type,
    //            claim.Value
    //        })
    //        .ToList();

    //    User newUser;
    //    string? userName = null;
    //    string? userEmail = null;

    //    if (claims == null || claims.Count == 0)
    //    {
    //        throw new ArgumentOutOfRangeException("Claims count cannot be 0");
    //    }

    //    var googleId = claims
    //        .FirstOrDefault(c =>
    //            c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
    //        )
    //        .Value;

    //    string profilePicture = _context
    //        .User.SingleOrDefault(user => user.GoogleId == googleId)
    //        .ProfilePicture;

    //    if (profilePicture != null)
    //        return profilePicture;
    //    else
    //        return string.Empty;
    //}

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

    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return View(
    //        new ErrorViewModel
    //        {
    //            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
    //        }
    //    );
    //}
    //}
}
