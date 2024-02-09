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
                var polls = _context
                    .Polls.Select(p => new
                    {
                        Poll = p,
                        FirstVoteCount = _context
                            .UserPoll.Where(up => up.Poll.Id == p.Id && up.Vote == true)
                            .Count(),
                        SecondVoteCount = _context
                            .UserPoll.Where(up => up.Poll.Id == p.Id && up.Vote == false)
                            .Count()
                    })
                    .ToList();

                var pollTitles = polls.ConvertAll(poll => poll.Poll.Title);

                var pollList = new { Polls = polls, PollTitles = pollTitles };

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
