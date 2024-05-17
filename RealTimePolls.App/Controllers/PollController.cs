//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Security.Cryptography.X509Certificates;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using realTimePolls.Models;
//using RealTimePolls.Data;
//using RealTimePolls.Models;
//using RealTimePolls.Models.Domain;
//using SignalRChat.Hubs;

//namespace realTimePolls.Controllers
//{
//    public class PollController : Controller
//    {
//        private readonly ILogger<PollController> _logger;

//        private readonly RealTimePollsContext _context; // Declare the DbContext variable

//        private static IHubContext<PollHub> _myHubContext;

//        private readonly IWebHostEnvironment _environment;

//        public PollController(
//            ILogger<PollController> logger,
//            RealTimePollsContext context,
//            IHubContext<PollHub> myHubContext,
//            IWebHostEnvironment environment
//        )
//        {
//            _logger = logger;
//            _context = context;
//            _myHubContext = myHubContext;
//            _environment = environment;
//        }

//        public async Task<int> GetUserId()
//        {
//            var result = await HttpContext.AuthenticateAsync(
//                CookieAuthenticationDefaults.AuthenticationScheme
//            );

//            if (result.Principal == null)
//                throw new Exception("Could not authenticate");

//            var claims = result
//                .Principal.Identities.FirstOrDefault()
//                ?.Claims.Select(claim => new
//                {
//                    claim.Issuer,
//                    claim.OriginalIssuer,
//                    claim.Type,
//                    claim.Value
//                })
//                .ToList();

//            User newUser;
//            string? userName = null;
//            string? userEmail = null;

//            if (claims == null || claims.Count == 0)
//            {
//                throw new ArgumentOutOfRangeException("Claims count cannot be 0");
//            }

//            var googleId = claims
//                .FirstOrDefault(c =>
//                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
//                )
//                .Value;

//            int userId = _context.User.SingleOrDefault(user => user.GoogleId == googleId).Id;
//            return userId;
//        }

//        public async Task<User> GetUser()
//        {
//            var result = await HttpContext.AuthenticateAsync(
//                CookieAuthenticationDefaults.AuthenticationScheme
//            );

//            if (result.Principal == null)
//                throw new Exception("Could not authenticate");

//            var claims = result
//                .Principal.Identities.FirstOrDefault()
//                ?.Claims.Select(claim => new
//                {
//                    claim.Issuer,
//                    claim.OriginalIssuer,
//                    claim.Type,
//                    claim.Value
//                })
//                .ToList();

//            User newUser;
//            string? userName = null;
//            string? userEmail = null;

//            if (claims == null || claims.Count == 0)
//            {
//                throw new ArgumentOutOfRangeException("Claims count cannot be 0");
//            }

//            var googleId = claims
//                .FirstOrDefault(c =>
//                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
//                )
//                .Value;

//            var user = _context.User.SingleOrDefault(user => user.GoogleId == googleId);
//            return user;
//        }

//        [HttpGet]
//        public async Task<ViewResult> Index([FromQuery] string polltitle, int pollid, int userid)
//        {
//            Poll poll = _context
//                .Polls.Include(p => p.Genre)
//                .FirstOrDefault(u => u.Id == pollid && u.UserId == userid && u.Title == polltitle);

//            if (poll == null)
//                throw new Exception("Poll cannot be found");

//            int firstVoteCount = _context
//                    .UserPoll.Where(up => up.PollId == pollid && up.Vote == true)
//                    .Count(),
//                secondVoteCount = _context
//                    .UserPoll.Where(up => up.PollId == pollid && up.Vote == false)
//                    .Count();

//            UserPoll userPoll =
//                _context.UserPoll.FirstOrDefault(up => up.UserId == userid && up.PollId == pollid)
//                ?? null;

//            if (HttpContext.User.Identity.IsAuthenticated)
//            {
//                var userId = await GetUserId();
//                var currentUser = _context.UserPoll.FirstOrDefault(up =>
//                    up.UserId == userId && up.PollId == pollid
//                );
//                ViewData["UserId"] = userId;
//                ViewData["CurrentVote"] = currentUser == null ? null : currentUser.Vote;
//            }

//            var viewModel = _context
//                .Polls.Select(p => new PollItem
//                {
//                    Poll = poll,
//                    FirstVoteCount = firstVoteCount,
//                    SecondVoteCount = secondVoteCount,
//                    Vote = userPoll == null ? null : userPoll.Vote,
//                })
//                .FirstOrDefault();

//            viewModel.EnvironmentName = _environment.EnvironmentName;
//            return View(viewModel);
//        }

//        [HttpPost]
//        public ActionResult Create(
//            [FromForm] string title,
//            [FromForm] string firstOption,
//            [FromForm] string secondOption,
//            [FromForm] int genre
//        )
//        {
//            try
//            {
//                var googleId =
//                    HttpContext != null ? HttpContext.User.Claims.ToList()[0].Value : string.Empty;

//                if (googleId == string.Empty)
//                    throw new Exception("Could not find googleId");

//                int userId = _context.User.SingleOrDefault(u => u.GoogleId == googleId).Id;

//                var pollGenre = _context.Genre.SingleOrDefault(g => g.Id == genre);

//                Poll poll = new Poll
//                {
//                    UserId = userId,
//                    Title = title,
//                    FirstOption = firstOption,
//                    SecondOption = secondOption,
//                    GenreId = pollGenre.Id
//                };

//                _context.Polls.Add(poll);
//                _context.SaveChanges();

//                return RedirectToAction(
//                    "Index",
//                    new
//                    {
//                        polltitle = poll.Title,
//                        pollid = poll.Id,
//                        userid = poll.UserId,
//                        genreName = pollGenre.Name,
//                    }
//                );
//            }
//            catch (Exception e)
//            {
//                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
//                return View("Error", errorViewModel);
//            }
//        }

//        public ActionResult Delete([FromQuery] int pollid)
//        {
//            try
//            {
//                var userPolls = _context.UserPoll.Where(up => up.PollId == pollid).ToList();
//                if (userPolls.Any())
//                {
//                    _context.UserPoll.RemoveRange(userPolls);
//                    _context.SaveChanges();
//                }

//                var poll = _context.Polls.SingleOrDefault(p => p.Id == pollid);
//                if (poll != null)
//                {
//                    _context.Polls.Remove(poll);
//                    _context.SaveChanges();
//                }

//                return RedirectToAction("Index", "Home", new { area = "" });
//            }
//            catch (Exception e)
//            {
//                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
//                return View("Error", errorViewModel);
//            }
//        }

//        public static void SendMessage()
//        {
//            _myHubContext.Clients.All.SendAsync("ReceiveMessage");
//        }

//        [HttpPost]
//        public async Task<IActionResult> Vote([FromForm] int userid, int pollid, string vote)
//        {
//            try
//            {
//                bool userVoter;
//                var currentUserId = await GetUserId();

//                if (vote == "Vote First")
//                    userVoter = true;
//                else if (vote == "Vote Second")
//                    userVoter = false;
//                else
//                    throw new Exception("Unable to vote");

//                var currentUserPoll = _context.UserPoll.FirstOrDefault(up =>
//                    up.PollId == pollid && up.UserId == currentUserId
//                );

//                if (currentUserPoll == null)
//                {
//                    _context.UserPoll.Add(
//                        new UserPoll
//                        {
//                            UserId = currentUserId,
//                            PollId = pollid,
//                            Vote = userVoter
//                        }
//                    );
//                }
//                else
//                {
//                    currentUserPoll.Vote = userVoter;
//                }

//                _context.SaveChanges();
//                SendMessage();
//                return RedirectToAction("Index", "Home", new { area = "" });
//            }
//            catch (Exception e)
//            {
//                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
//                return View("Error", errorViewModel);
//            }
//        }
//    }
//}
