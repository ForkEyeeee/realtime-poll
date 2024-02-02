using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            _context = context; // Initialize the _context variable. This is my DbContext instance.
        }

        public IActionResult Index()
        {
            var polls = _context.Polls.ToList();

            //foreach (Poll poll in polls)
            //{
            //   var ages = people.Select(person => person.Age).ToArray();
            //}

            var pollTitles = polls.ConvertAll<string>(poll => poll.Title);
            //var pollOptions = polls.ConvertAll<string>(poll => poll.Name )
            // Pass the polls to the view

            var viewModel = new PollsViewModel { Polls = polls, PollTitles = pollTitles, };
            return View(viewModel);
        }

        public IActionResult Poll(string pollName)
        {
            //var polls = _context.Polls.Find(pollName);
            //Poll poll = polls.Where(p => p.PollName == pollName).FirstOrDefault();
            //Poll poll = polls.
            //    (c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            var polls = _context.Polls.ToList();

            var poll = polls.FirstOrDefault(u => u.Title == pollName);
            var pollTitles = polls.ConvertAll<string>(poll => poll.Title);

            if (poll != null)
            {
                //Poll viewModel = poll;
                PollsViewModel viewModel = new PollsViewModel
                {
                    Polls = polls,
                    PollTitles = pollTitles,
                    FirstOption = poll.FirstOption,
                    SecondOption = poll.SecondOption,
                };
                //return PartialView("index");

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
