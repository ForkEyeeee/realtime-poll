using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;
using SignalRChat.Hubs;
using RealTimePolls.Helpers;

namespace realTimePolls.Controllers
{
    public class PollController : Controller
    {

        private readonly RealTimePollsDbContext dbContext;
        private static IHubContext<PollHub> _myHubContext;
        private readonly IPollRepository pollRepository;
        private readonly IHelpersRepository helpersRepository;
        private readonly IMapper mapper;

        public PollController(
            RealTimePollsDbContext dbContext,
            IHubContext<PollHub> myHubContext,
            IPollRepository pollRepository,
            IHelpersRepository helpersRepository,
            IMapper mapper
        )
        {
            this.dbContext = dbContext;
            _myHubContext = myHubContext;
            this.pollRepository = pollRepository;
            this.helpersRepository = helpersRepository;
            this.mapper = mapper;
        }

        //private async Task<int> GetUserId()
        //{
        //    var result = await HttpContext.AuthenticateAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme
        //    );

        //    if (result.Principal == null)
        //        throw new Exception("Could not authenticate");

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

        //    int userId = dbContext.User.SingleOrDefault(user => user.GoogleId == googleId).Id;
        //    return userId;
        //}

        // Get poll by id
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
        // <--refactor the rest of these methods -->

        //Create poll
        [HttpPost]
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
        // Delete poll
        public async Task<IActionResult> DeletePollAsync([FromQuery] int pollid)
        {

            await pollRepository.DeletePollAsync(pollid);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Vote([FromForm] int userid, int pollid, string vote)
        {
            return null;

        }

        // helper
        private static void SendMessage()
        {
            _myHubContext.Clients.All.SendAsync("ReceiveMessage");
        }
    }





}

