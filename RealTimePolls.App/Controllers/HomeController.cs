using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;

namespace RealTimePolls.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService homeService;
        private readonly IMapper mapper;

        public HomeController(
            IHomeService homeService,
            IMapper mapper
        )
        {
            this.homeService = homeService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var homeViewModel = await homeService.Index();
            return View(homeViewModel);
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