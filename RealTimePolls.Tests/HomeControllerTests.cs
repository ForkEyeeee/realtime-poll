using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealTimePolls.Controllers;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealTimePolls.Models.DTO;

namespace HomeUnitTests
{
    public class HomeControllerTests
    {
        private readonly HomeController _homeController;
        private readonly IHomeService _homeService;
        private readonly ILogger<HomeController> _logger;

        public HomeControllerTests()
        {
            _logger = A.Fake<ILogger<HomeController>>();
            _homeService = A.Fake<IHomeService>();

            _homeController = new HomeController(_homeService);
        }

        [Fact]
        public async Task HomeController_Index_ReturnsSuccess()
        {
            var homeViewModel = new HomeViewModel
            {
                Polls = new List<PollDto>
                {
                    new PollDto { Id = 1, Title = "Title", FirstOption = "FirstOption", SecondOption = "SecondOption" }
                }
            };

            A.CallTo(() => _homeService.Index()).Returns(Task.FromResult(homeViewModel));

            var result = await _homeController.Index();

            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.Model.Should().BeEquivalentTo(homeViewModel);
        }
    }
}
