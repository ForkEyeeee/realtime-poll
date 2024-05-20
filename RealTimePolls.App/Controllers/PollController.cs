using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private static IHubContext<PollHub> _myHubContext;
        private readonly IPollRepository pollRepository;
        private readonly IMapper mapper;

        public PollController(
            RealTimePollsDbContext dbContext,
            IHubContext<PollHub> myHubContext,
            IPollRepository pollRepository,
            IMapper mapper
        )
        {
            this.dbContext = dbContext;
            _myHubContext = myHubContext;
            this.pollRepository = pollRepository;
            this.mapper = mapper;
        }

        private async Task<int> GetUserId()
        {
            var result = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            if (result.Principal == null)
                throw new Exception("Could not authenticate");

            var claims = result
                .Principal.Identities.FirstOrDefault()
                ?.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                })
                .ToList();

            User newUser;
            string? userName = null;
            string? userEmail = null;

            if (claims == null || claims.Count == 0)
            {
                throw new ArgumentOutOfRangeException("Claims count cannot be 0");
            }

            var googleId = claims
                .FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                )
                .Value;

            int userId = dbContext.User.SingleOrDefault(user => user.GoogleId == googleId).Id;
            return userId;
        }

        //public async Task<User> GetUser()
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

        //    var user = dbContext.User.SingleOrDefault(user => user.GoogleId == googleId);
        //    return user;
        //}

        // Get poll by id
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string pollTitle, int pollId, int userId)
        {
            var pollViewModelDomain = await pollRepository.GetPollAsync(pollTitle, pollId, userId);

            if(pollViewModelDomain == null)
            {
                return View(null);
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

        //Create poll
        [HttpPost]
        public ActionResult Create(
            [FromForm] string title,
            [FromForm] string firstOption,
            [FromForm] string secondOption,
            [FromForm] int genre
        )
        {
            try
            {
                var googleId =
                    HttpContext != null ? HttpContext.User.Claims.ToList()[0].Value : string.Empty;

                if (googleId == string.Empty)
                    throw new Exception("Could not find googleId");

                int userId = dbContext.User.SingleOrDefault(u => u.GoogleId == googleId).Id;

                var pollGenre = dbContext.Genre.SingleOrDefault(g => g.Id == genre);

                Poll poll = new Poll
                {
                    UserId = userId,
                    Title = title,
                    FirstOption = firstOption,
                    SecondOption = secondOption,
                    GenreId = pollGenre.Id
                };

                dbContext.Polls.Add(poll);
                dbContext.SaveChanges();

                return RedirectToAction(
                    "Index",
                    new
                    {
                        polltitle = poll.Title,
                        pollid = poll.Id,
                        userid = poll.UserId,
                        genreName = pollGenre.Name,
                    }
                );
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        // Delete poll
        public ActionResult Delete([FromQuery] int pollid)
        {
            try
            {
                var userPolls = dbContext.UserPoll.Where(up => up.PollId == pollid).ToList();
                if (userPolls.Any())
                {
                    dbContext.UserPoll.RemoveRange(userPolls);
                    dbContext.SaveChanges();
                }

                var poll = dbContext.Polls.SingleOrDefault(p => p.Id == pollid);
                if (poll != null)
                {
                    dbContext.Polls.Remove(poll);
                    dbContext.SaveChanges();
                }

                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        // helper
        private static void SendMessage()
        {
            _myHubContext.Clients.All.SendAsync("ReceiveMessage");
        }

        [HttpPost]
        public async Task<IActionResult> Vote([FromForm] int userid, int pollid, string vote)
        {
            try
            {
                bool userVoter;
                var currentUserId = await GetUserId();

                if (vote == "Vote First")
                    userVoter = true;
                else if (vote == "Vote Second")
                    userVoter = false;
                else
                    throw new Exception("Unable to vote");

                var currentUserPoll = dbContext.UserPoll.FirstOrDefault(up =>
                    up.PollId == pollid && up.UserId == currentUserId
                );

                if (currentUserPoll == null)
                {
                    dbContext.UserPoll.Add(
                        new UserPoll
                        {
                            UserId = currentUserId,
                            PollId = pollid,
                            Vote = userVoter
                        }
                    );
                }
                else
                {
                    currentUserPoll.Vote = userVoter;
                }

                dbContext.SaveChanges();
                SendMessage();
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }
    }
}
