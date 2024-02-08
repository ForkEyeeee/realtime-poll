﻿using Microsoft.AspNetCore.Mvc;
using realTimePolls.Models;

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

        [HttpGet]
        public ActionResult Index(string data, string pollid, string userid)
        {
            var pollId = Int32.Parse(pollid);
            var userId = Int32.Parse(userid);
            var polls = _context.Polls.ToList();

            var poll = polls.FirstOrDefault(u => u.Title == data);
            var pollTitles = polls.ConvertAll<string>(poll => poll.Title);
            var userPolls = _context.UserPoll.ToList();
            var userPoll = userPolls.FirstOrDefault(userPoll =>
                userPoll.PollId == pollId && userPoll.UserId == userId
            );

            var viewModel = new PollsList
            {
                Poll = poll,
                PollTitles = pollTitles,
                UserPoll = userPoll
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var formValues = collection.ToList();
                var pollTitle = formValues[0].Value;
                var firstOption = formValues[1].Value;
                var secondOption = formValues[2].Value;

                var googleId =
                    HttpContext != null ? HttpContext.User.Claims.ToList()[0].Value : string.Empty;

                if (googleId == string.Empty)
                    throw new Exception("Could not find googleId");

                int userId = _context.User.FirstOrDefault(u => u.GoogleId == googleId).Id;

                Poll poll = new Poll
                {
                    UserId = userId,
                    Title = pollTitle,
                    FirstOption = firstOption,
                    SecondOption = secondOption
                };

                _context.Polls.Add(poll);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        [HttpPost]
        public ActionResult Delete(string pollid)
        {
            try
            {
                var pollId = Int32.Parse(pollid);
                var poll = _context.Polls.FirstOrDefault(poll => poll.Id == pollId);
                if (poll == null)
                {
                    throw new Exception("Could not find pollId");
                }
                _context.Polls.Remove(poll);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        [HttpPost]
        public ActionResult Vote(string vote, string pollid, string userid)
        {
            try
            {
                var pollId = Int32.Parse(pollid);
                var userId = Int32.Parse(userid);

                var polls = _context.Polls.ToList();
                var users = _context.User.ToList();
                var userPolls = _context.UserPoll.ToList();

                var poll = polls.FirstOrDefault(u => u.Id == pollId); //get the current poll
                var user = users.FirstOrDefault(user => user.Id == userId); //get the current user
                var userPoll = userPolls.FirstOrDefault(userPoll =>
                    userPoll.PollId == pollId && userPoll.UserId == userId
                ); // get the current userPoll

                var pollTitles = polls.ConvertAll<string>(poll => poll.Title);

                PollsList viewModel = new PollsList
                {
                    Polls = polls,
                    Poll = poll,
                    PollTitles = pollTitles,
                    FirstOption = poll.FirstOption,
                    SecondOption = poll.SecondOption,
                };

                UserPoll UserPoll = new UserPoll()
                {
                    UserId = userId,
                    PollId = poll.Id,
                    FirstVote = vote == "Vote First" ? true : false,
                    SecondVote = vote == "Vote Second" ? true : false
                };

                if (userPoll == null)
                {
                    _context.UserPoll.Add(UserPoll);
                }
                else
                {
                    userPoll.FirstVote = UserPoll.FirstVote;
                    userPoll.SecondVote = UserPoll.SecondVote;
                }

                _context.SaveChanges();

                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }
    }
}