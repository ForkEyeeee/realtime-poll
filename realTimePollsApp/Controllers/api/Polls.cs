using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index([FromQuery] int page = 1)
        {
            try
            {
                int take = 5;
                int skip = (page - 1) * take;

                var polls = _context
                    .Polls.Skip(skip)
                    .Take(take)
                    .Select(p => new PollItem
                    {
                        Poll = p,
                        FirstVoteCount = _context
                            .UserPoll.Where(up => up.PollId == p.Id && up.Vote == true)
                            .Count(),
                        SecondVoteCount = _context
                            .UserPoll.Where(up => up.PollId == p.Id && up.Vote == false)
                            .Count(),
                        UserName = _context.User.SingleOrDefault(user => user.Id == p.UserId).Name,
                        ProfilePicture = _context
                            .User.SingleOrDefault(user => user.Id == p.UserId)
                            .ProfilePicture
                    })
                    .ToList();

                int pollCount = _context.Polls.Count();

                var pollList = new PollsList { Polls = polls, PollCount = pollCount };

                return Json(pollList);
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        [HttpGet]
        public IActionResult GetDropdownList([FromQuery] int page = 1)
        {
            try
            {
                var dropdownList = _context.Genre.ToList();

                var options = new { options = dropdownList };

                return Json(options);
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        [HttpGet]
        public IActionResult GetSearchResults([FromQuery] string search = "")
        {
            try
            {
                int take = 5;
                int skip = (1 - 1) * take;

                var polls = _context
                    .Polls.Where(c => EF.Functions.Like(c.Title, search))
                    .Skip(skip)
                    .Take(take)
                    .Select(p => new PollItem
                    {
                        Poll = p,
                        FirstVoteCount = _context
                            .UserPoll.Where(up => up.PollId == p.Id && up.Vote == true)
                            .Count(),
                        SecondVoteCount = _context
                            .UserPoll.Where(up => up.PollId == p.Id && up.Vote == false)
                            .Count(),
                        UserName = _context.User.SingleOrDefault(user => user.Id == p.UserId).Name,
                        ProfilePicture = _context
                            .User.SingleOrDefault(user => user.Id == p.UserId)
                            .ProfilePicture
                    })
                    .ToList();

                return Json(polls);
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
