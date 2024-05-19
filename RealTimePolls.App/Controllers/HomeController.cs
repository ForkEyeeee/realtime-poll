using System.Diagnostics;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> userManager;
        private readonly IHttpContextAccessor httpAccessor;

        public HomeController(
            RealTimePollsDbContext dbContext,
            IWebHostEnvironment environment,
            IHomeRepository homeRepository,
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpAccessor
        )
        {
            this.dbContext = dbContext;
            this.environment = environment;
            this.homeRepository = homeRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.httpAccessor = httpAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var domainPolls = await homeRepository.Index();

            var polls = domainPolls.Select(p => new HomeViewModel
            {
                Poll = mapper.Map<PollDto>(p),
                FirstVoteCount = dbContext
                    .UserPoll.Where(up => up.PollId == p.Id && up.Vote == true)
                    .Count(),
                SecondVoteCount = dbContext
                    .UserPoll.Where(up => up.PollId == p.Id && up.Vote == false)
                    .Count(),
                UserName = dbContext.User.SingleOrDefault(user => user.Id == p.UserId).Name,
                ProfilePicture = dbContext
                    .User.SingleOrDefault(user => user.Id == p.UserId)
                    .ProfilePicture
            });

            var homeViewModel = new List<HomeViewModel>();

            foreach (var poll in polls)
            {
                homeViewModel.Add(mapper.Map<HomeViewModel>(poll));
            }

            return View(homeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetPolls()
        {
            var polls = await homeRepository.Index();

            var homeViewModel = new List<HomeViewModel>();

            foreach (var poll in polls)
            {
                homeViewModel.Add(mapper.Map<HomeViewModel>(poll));
            }

            return Json(homeViewModel);
        }

        [HttpGet]
        public async Task<List<Genre>> GetDropdownList()
        {
            var genreOptions = await homeRepository.GetDropdownList();

            return genreOptions;
        }

        [HttpGet]
        public async Task<string> GetUserProfilePicture()
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            //if (result.Principal == null)
            //    return string.Empty;


            var profilePicture = await homeRepository.GetUserProfilePicture(result);

            return profilePicture;
        }

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
