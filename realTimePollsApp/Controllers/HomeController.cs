using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        public async Task<IActionResult> Index([FromQuery] int page = 1)
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
                var viewModel = new PollsList { Polls = polls, };

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var profilePicture = await GetUserProfilePicture();
                    viewModel.UserProfilePicture = profilePicture;
                }

                return View(viewModel);
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        public async Task<string> GetUserProfilePicture()
        {
            var result = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            if (result.Principal == null)
                throw new Exception("Could not authenticate");

            var claims = result
                .Principal.Identities.FirstOrDefault()
                ?.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                })
                .ToList();

            User newUser;
            string? userName = null;
            string? userEmail = null;

            if (claims == null || claims.Count == 0)
            {
                throw new ArgumentOutOfRangeException("Claims count cannot be 0");
            }

            var googleId = claims
                .FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                )
                .Value;

            string profilePicture = _context
                .User.SingleOrDefault(user => user.GoogleId == googleId)
                .ProfilePicture;

            if (profilePicture != null)
                return profilePicture;
            else
                return string.Empty;
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
