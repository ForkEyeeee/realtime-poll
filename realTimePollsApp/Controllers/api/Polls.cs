using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using realTimePolls.Models;

namespace realTimePolls.Controllers
{
    public class Polls : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly RealTimePollsContext _context; // Declare the DbContext variable

        public Polls(ILogger<HomeController> logger, RealTimePollsContext context) // Inject DbContext in the constructor
        {
            _logger = logger;
            _context = context; // Initialize the _context variable. This the DbContext instance.
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var polls = _context.Polls;

                var pollList = polls.ToList();

                foreach (var poll in pollList)
                {
                    int firstOptionCount = _context
                        .UserPoll.Where(userPoll =>
                            userPoll.PollId == poll.Id && userPoll.FirstVote == true
                        )
                        .Count();

                    int secondOptionCount = _context
                        .UserPoll.Where(userPoll =>
                            userPoll.PollId == poll.Id && userPoll.SecondVote == true
                        )
                        .Count();

                    poll.FirstVotes = firstOptionCount;
                    poll.SecondVotes = secondOptionCount;
                }

                return Json(pollList);
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
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
