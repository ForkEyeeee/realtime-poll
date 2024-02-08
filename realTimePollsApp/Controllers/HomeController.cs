using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

                var pollTitles = pollList.ConvertAll(poll => poll.Title);

                var viewModel = new PollsList { Polls = pollList, PollTitles = pollTitles };

                return View(viewModel);
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        [HttpPost]
        public IActionResult Poll(string pollName)
        {
            try
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
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetRawContent()
        {
            try
            {
                string rawContent = string.Empty;
                using (
                    var reader = new StreamReader(
                        Request.Body,
                        encoding: Encoding.UTF8,
                        detectEncodingFromByteOrderMarks: false
                    )
                )
                {
                    rawContent = await reader.ReadToEndAsync();
                }
                return Ok(rawContent);
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
