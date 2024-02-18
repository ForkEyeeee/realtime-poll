using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using realTimePolls.Models;
using SignalRChat.Hubs;

namespace realTimePolls.Controllers
{
    public class PollController : Controller
    {
        private readonly ILogger<PollController> _logger;

        private readonly RealTimePollsContext _context; // Declare the DbContext variable
        private readonly IHubContext<PollHub> _myHubContext;

        public PollController(
            ILogger<PollController> logger,
            RealTimePollsContext context,
            IHubContext<PollHub> myHubContext
        )
        {
            _logger = logger;
            _context = context;
            _myHubContext = myHubContext;
        }

        public class JsonRequestItem
        {
            public string jsonRequest { get; set; }
        }

        public async Task<int> GetUserId()
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

            int userId = _context.User.SingleOrDefault(user => user.GoogleId == googleId).Id;
            return userId;
        }

        [HttpGet]
        public async Task<ViewResult> Index([FromQuery] string polltitle, int pollid, int userid)
        {
            Poll poll = _context
                .Polls.Include(p => p.Genre) //maybe do the same ting here for user and poll
                .FirstOrDefault(u => u.Id == pollid && u.UserId == userid && u.Title == polltitle);

            if (poll == null)
                throw new Exception("Poll cannot be found");

            int firstVoteCount = _context
                    .UserPoll.Where(up => up.PollId == pollid && up.Vote == true)
                    .Count(),
                secondVoteCount = _context
                    .UserPoll.Where(up => up.PollId == pollid && up.Vote == false)
                    .Count();

            UserPoll userPoll =
                _context.UserPoll.FirstOrDefault(up => up.UserId == userid && up.PollId == pollid)
                ?? null;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = await GetUserId();
                ViewData["UserId"] = userId;
            }

            var viewModel = _context
                .Polls.Select(p => new PollItem
                {
                    Poll = poll,
                    FirstVoteCount = firstVoteCount,
                    SecondVoteCount = secondVoteCount,
                    Vote = userPoll == null ? null : userPoll.Vote,
                })
                .FirstOrDefault();

            return View(viewModel);
        }

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
                //if (_context.Polls.Count() >= 25)
                //    return "test" as ActionResult;

                var googleId =
                    HttpContext != null ? HttpContext.User.Claims.ToList()[0].Value : string.Empty;

                if (googleId == string.Empty)
                    throw new Exception("Could not find googleId");

                int userId = _context.User.SingleOrDefault(u => u.GoogleId == googleId).Id;

                var pollGenre = _context.Genre.SingleOrDefault(g => g.Id == genre);

                Poll poll = new Poll
                {
                    UserId = userId,
                    Title = title,
                    FirstOption = firstOption,
                    SecondOption = secondOption,
                    GenreId = pollGenre.Id,
                    ErrorMsg = string.Empty
                };

                _context.Polls.Add(poll);
                _context.SaveChanges();

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

        public ActionResult Delete([FromQuery] int pollid)
        {
            try
            {
                var userPolls = _context.UserPoll.Where(up => up.PollId == pollid).ToList();
                if (userPolls.Any())
                {
                    _context.UserPoll.RemoveRange(userPolls);
                }

                var poll = _context.Polls.SingleOrDefault(p => p.Id == pollid);
                if (poll == null)
                {
                    throw new Exception("Could not find pollId");
                }

                _context.Polls.Remove(poll);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        public void SendMessage() //this can receive a string msg, then pass it to sendasync
        {
            _myHubContext.Clients.All.SendAsync("ReceiveMessage");
        }

        [HttpPost]
        public ActionResult Vote([FromForm] int userid, int pollid, string vote)
        {
            try
            {
                bool userVoter;

                if (vote == "Vote First")
                    userVoter = true;
                else if (vote == "Vote Second")
                    userVoter = false;
                else
                    throw new Exception("Unable to vote");

                var userPoll = _context.UserPoll.FirstOrDefault(userPoll =>
                    userPoll.PollId == pollid && userPoll.UserId == userid
                );

                UserPoll UserPoll = new UserPoll()
                {
                    UserId = userid,
                    PollId = pollid,
                    Vote = userVoter
                };

                if (userPoll == null)
                    _context.UserPoll.Add(UserPoll);
                else
                    userPoll.Vote = userVoter;

                _context.SaveChanges();

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
