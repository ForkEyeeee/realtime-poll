using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;

namespace RealTimePolls.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHomeRepository homeRepository;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(
            IHomeRepository homeRepository,
            IMapper mapper
        )
        {
            this.homeRepository = homeRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var domainPolls = await homeRepository.Index();

            var homeViewModel = new List<HomeViewModel>();

            foreach (var poll in domainPolls)
            {
                homeViewModel.Add(mapper.Map<HomeViewModel>(poll));
            }

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