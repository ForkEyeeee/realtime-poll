using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


        // GET: PollController
        public ActionResult Index(string data)
        {
            var polls = _context.Polls.ToList();
            var poll = polls.FirstOrDefault(u => u.Title == data);
            var pollTitles = polls.ConvertAll<string>(poll => poll.Title);
            
            var viewModel = new PollsViewModel
            {
                Poll = poll,
                PollTitles = pollTitles
            };
            return View(viewModel);
        }

        // GET: PollController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PollController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PollController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PollController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PollController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PollController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PollController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
