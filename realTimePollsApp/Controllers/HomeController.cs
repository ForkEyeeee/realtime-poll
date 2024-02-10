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
        public IActionResult Index([FromQuery] int page = 1)
        {
            try
            {
                var pageQueryParam = HttpContext.Request.Query["page"].ToString();
                bool isPage = pageQueryParam == "1";

                if (page == 1 && !isPage)
                {
                    return RedirectToAction("Home", new { page });
                }

                var polls = _context
                    .Polls.Select(p => new PollItem
                    {
                        Poll = p,
                        FirstVoteCount = _context
                            .UserPoll.Where(up => up.PollId == p.Id && up.Vote == true)
                            .Count(),
                        SecondVoteCount = _context
                            .UserPoll.Where(up => up.PollId == p.Id && up.Vote == false)
                            .Count()
                    })
                    .ToList();

                var viewModel = new PollsList { Polls = polls };

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
                Poll poll = _context.Polls.FirstOrDefault(u => u.Title == pollName);

                if (poll == null)
                    throw new Exception("Poll cannot be found");

                int firstVoteCount = _context.UserPoll.Count(up =>
                        up.PollId == poll.Id && up.Vote == true
                    ),
                    secondVoteCount = _context.UserPoll.Count(up =>
                        up.PollId == poll.Id && up.Vote == true
                    );

                var viewModel = _context
                    .Polls.Select(p => new PollItem
                    {
                        Poll = poll,
                        FirstVoteCount = firstVoteCount,
                        SecondVoteCount = secondVoteCount
                    })
                    .ToList();

                return View("index", viewModel);
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
