using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NZWalks.API.CustomActionFilters;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;
using SignalRChat.Hubs;

namespace realTimePolls.Controllers
{
    public class PollController : Controller
    {

        private readonly RealTimePollsDbContext dbContext;
        private readonly IPollRepository pollRepository;
        private readonly IHelpersRepository helpersRepository;
        private readonly IMapper mapper;
        private readonly ILogger<PollController> logger;

        public PollController(
            RealTimePollsDbContext dbContext,
            IHubContext<PollHub> myHubContext,
            IPollRepository pollRepository,
            IHelpersRepository helpersRepository,
            IMapper mapper,
            ILogger<PollController> logger

        )
        {
            this.dbContext = dbContext;
            this.pollRepository = pollRepository;
            this.helpersRepository = helpersRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string pollTitle, int pollId, int userId)
        {
            var pollViewModelDomain = await pollRepository.GetPollAsync(pollTitle, pollId, userId);

            if (pollViewModelDomain == null)
            {
                return null;
            }

            PollViewModel pollViewModel = new PollViewModel()
            {
                Poll = mapper.Map<PollDto>(pollViewModelDomain.Poll),
                FirstVoteCount = pollViewModelDomain.FirstVoteCount,
                SecondVoteCount = pollViewModelDomain.SecondVoteCount,
                Vote = pollViewModelDomain.Vote
            };

            return View(pollViewModel);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create(AddPollRequest addPollRequest)
        {

            AuthenticateResult result = await HttpContext.AuthenticateAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme
              );

            addPollRequest.UserId = await helpersRepository.GetUserId(result);

            var domainPoll = mapper.Map<Poll>(addPollRequest);
            await pollRepository.CreatePollAsync(domainPoll);
            return RedirectToAction("Index", "Home");

        }

        [HttpDelete]
        public async Task<IActionResult> DeletePollAsync([FromQuery] int pollid)
        {

            await pollRepository.DeletePollAsync(pollid);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> VoteAsync(AddVoteRequest addVoteRequest)
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme
             );

            await pollRepository.VoteAsync(result, addVoteRequest);
            return RedirectToAction("Index", "Home", new { area = "" });

        }


    }

}

