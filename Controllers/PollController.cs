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

        public ActionResult Vote(string vote, string pollid, string userid)
        {
            var pollId = Int32.Parse(pollid);
            var userId = Int32.Parse(userid);

            var polls = _context.Polls.ToList();
            var users = _context.User.ToList();
            var userPolls = _context.UserPoll.ToList();

            var poll = polls.FirstOrDefault(u => u.Id == pollId); //get the current poll
            var user = users.FirstOrDefault(user => user.Id == userId); //get the current user
            var userPoll = userPolls.FirstOrDefault(userPoll =>
                userPoll.Poll == pollId && userPoll.UserId == pollId
            ); // get the current userPoll

            if (vote == "Vote First" && poll.FirstVotes <= 0)
            {
                poll.FirstVotes += 1;
            }
            else if (vote == "Vote Second" && poll.SecondVotes <= 0)
            {
                poll.FirstVotes -= 1;
            }
            else
            {
                Debug.WriteLine("Cannot update");
            }
            _context.SaveChanges();

            //var existingPollId = userPoll.Poll;
            //var existingPoll = polls.FirstOrDefault(u => u.Id == existingPollId);
            //if (existingPoll.FirstVotes == 1 == null)
            //    if (vote == "Vote First")
            //    {
            //        if (poll.FirstVotes == null)
            //        {
            //            poll.FirstVotes = 1;
            //        }
            //        else
            //        {
            //            poll.FirstVotes += 1;
            //        }
            //    }
            //    else
            //    {
            //        if (poll.SecondVotes == null)
            //        {
            //            poll.SecondVotes = 1;
            //        }
            //        else
            //        {
            //            poll.SecondVotes += 1;
            //        }
            //    }
            //_context.SaveChanges();



            //var userPoll =
            // when the user votes, check if there is a record in UserPolls that has
            // combination of the current userId and pollid.
            // ^ if so, then reject the vote. if not, then allow it
            //when a user votes, add the poll id to the users poll column. like an array
            return View();
        }
    }
}
