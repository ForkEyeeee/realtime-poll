using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using realTimePolls.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace realTimePolls.Controllers
{
    public class PollController : Controller
    {
        private readonly ILogger<PollController> _logger;

        private readonly RealTimePollsContext _context; // Declare the DbContext variable

        public PollController(ILogger<PollController> logger, RealTimePollsContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: PollController
        public ActionResult Index(string data)
        {
            var polls = _context.Polls.ToList();
            var poll = polls.FirstOrDefault(u => u.Title == data);
            var pollTitles = polls.ConvertAll<string>(poll => poll.Title);

            var viewModel = new PollsViewModel { Poll = poll, PollTitles = pollTitles };
            return View(viewModel);
        }

        // GET: PollController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Vote(string vote, string hidden, string userId)
        {
            Debug.WriteLine(vote);
            var pollId = Int32.Parse(hidden);
            var polls = _context.Polls.ToList();
            var poll = polls.FirstOrDefault(u => u.Id == pollId);
            if (vote == "Vote First")
            {
                if (poll.FirstVotes == null)
                {
                    poll.FirstVotes = 1;
                }
                else
                {
                    poll.FirstVotes += 1;
                }
            }
            else
            {
                if (poll.SecondVotes == null)
                {
                    poll.SecondVotes = 1;
                }
                else
                {
                    poll.SecondVotes += 1;
                }
            }
            _context.SaveChanges();
            var users = _context.User.ToList();
            var user = users.FirstOrDefault(user => user.Id == Int32.Parse(userId));
            //user.Polls
            // when the user votes, check if there is a record in UserPolls that has
            // combination of the current userId and pollid.
            // ^ if so, then reject the vote. if not, then allow it
            //when a user votes, add the poll id to the users poll column. like an array
            return View();
        }
    }
}
