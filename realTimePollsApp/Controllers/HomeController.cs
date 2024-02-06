using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using realTimePolls.Models;

namespace realTimePolls.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly RealTimePollsContext _context; // Declare the DbContext variable

        public HomeController(ILogger<HomeController> logger, RealTimePollsContext context) // Inject DbContext in the constructor
        {
            _logger = logger;
            _context = context; // Initialize the _context variable. This the DbContext instance.
        }

        public IActionResult Index()
        {
            var polls = _context.Polls.ToList();

            // Add vote counts to list of polls
            foreach (var poll in polls)
            {
                int firstOptionCount = _context
                        .UserPoll.Where(userPoll =>
                            userPoll.PollId == poll.Id && userPoll.FirstVote == true
                        )
                        .Count(),
                    secondOptionCount = _context
                        .UserPoll.Where(userPoll =>
                            userPoll.PollId == poll.Id && userPoll.SecondVote == true
                        )
                        .Count();

                poll.FirstVotes = firstOptionCount;
                poll.SecondVotes = secondOptionCount;
            }

            var pollTitles = polls.ConvertAll(poll => poll.Title);

            var viewModel = new PollsList { Polls = polls, PollTitles = pollTitles, };
            return View(viewModel);
        }

        public List<Poll> RefreshData()
        {
            var polls = _context.Polls.ToList();

            foreach (var poll in polls)
            {
                int firstOptionCount = _context
                        .UserPoll.Where(userPoll =>
                            userPoll.PollId == poll.Id && userPoll.FirstVote == true
                        )
                        .Count(),
                    secondOptionCount = _context
                        .UserPoll.Where(userPoll =>
                            userPoll.PollId == poll.Id && userPoll.SecondVote == true
                        )
                        .Count();

                poll.FirstVotes = firstOptionCount;
                poll.SecondVotes = secondOptionCount;
            }

            var pollTitles = polls.ConvertAll(poll => poll.Title);

            return polls;
        }

        public IActionResult Poll(string pollName)
        {
            var polls = _context.Polls.ToList();

            var poll = polls.FirstOrDefault(u => u.Title == pollName);
            var pollTitles = polls.ConvertAll(poll => poll.Title);

            if (poll != null)
            {
                PollsList viewModel = new PollsList
                {
                    Polls = polls,
                    PollTitles = pollTitles,
                    FirstOption = poll.FirstOption,
                    SecondOption = poll.SecondOption,
                };

                return View("index", viewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
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
}
