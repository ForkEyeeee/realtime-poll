using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NZWalks.API.CustomActionFilters;
using RealTimePolls.Data;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;
using SignalRChat.Hubs;

namespace RealTimePolls.Controllers
{
    public class PollController : Controller
    {

        private readonly RealTimePollsDbContext dbContext;
        private readonly IHubContext<PollHub> myHubContext;
        private readonly IPollService pollService;
        private readonly IHelpersService helpersService;
        private readonly ILogger<PollController> logger;

        public PollController(
            RealTimePollsDbContext dbContext,
            IHubContext<PollHub> myHubContext,
            IPollService pollService,
            IHelpersService helpersService,
            ILogger<PollController> logger

        )
        {
            this.dbContext = dbContext;
            this.myHubContext = myHubContext;
            this.pollService = pollService;
            this.helpersService = helpersService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string pollTitle, int pollId, int userId)
        {
            var pollViewModel = await pollService.Index(pollTitle, pollId, userId);

            if (pollViewModel == null)
                return NotFound();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                AuthenticateResult result = await HttpContext.AuthenticateAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme
                );
                var currentUserId = await helpersService.GetUserId(result);
                ViewBag.UserId = currentUserId;
            }

            return View(pollViewModel);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create(AddPollRequest addPollRequest)
        {

            AuthenticateResult result = await HttpContext.AuthenticateAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme
              );

            addPollRequest.UserId = await helpersService.GetUserId(result);
            await pollService.CreatePollAsync(addPollRequest);
            return RedirectToAction("Index", "Home");

        }

        [HttpDelete]
        public async Task<IActionResult> DeletePollAsync([FromQuery] int pollid)
        {
            await pollService.DeletePollAsync(pollid);
            return Ok();
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> VoteAsync(AddVoteRequest addVoteRequest)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme
             );

            await pollService.VoteAsync(result, addVoteRequest);
            return RedirectToAction("Index", "Home");
        }
    }
}

