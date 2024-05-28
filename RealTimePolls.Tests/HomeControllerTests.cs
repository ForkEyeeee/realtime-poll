using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealTimePolls.Controllers;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;
using Xunit;

namespace HomeUnitTests
{
    public class HomeControllerTests
    {
        private readonly HomeController _homeController;
        private readonly IHomeService _homeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeControllerTests()
        {
            _logger = A.Fake<ILogger<HomeController>>();
            _homeRepository = A.Fake<IHomeService>();
            _mapper = A.Fake<IMapper>();

            _homeController = new HomeController(_homeRepository, _mapper);
        }

        [Fact]
        public async Task HomeController_Index_ReturnsSuccess()
        {
            var polls = new List<Poll>
            {
                new Poll { Id = 1, UserId = 1, Title = "Title", FirstOption = "FirstOption", SecondOption = "SecondOption" }
            };

            var homeViewModels = polls.Select(p => new HomeViewModel { Id = p.Id, Title = p.Title, FirstOption = p.FirstOption, SecondOption = p.SecondOption }).ToList();

            A.CallTo(() => _homeRepository.Index()).Returns(Task.FromResult(polls));
            A.CallTo(() => _mapper.Map<HomeViewModel>(A<Poll>.That.Matches(p => p.Id == 1))).Returns(homeViewModels.First());

            var result = await _homeController.Index();

            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.Model.Should().BeEquivalentTo(homeViewModels);
        }
    }
}
