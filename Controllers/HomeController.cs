using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using realTimePolls.Models;
using System.Diagnostics;
using System.Linq;

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
            var pollNames = polls.ConvertAll<string>(poll => poll.Name);

            // Pass the polls to the view
            return View(pollNames);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}