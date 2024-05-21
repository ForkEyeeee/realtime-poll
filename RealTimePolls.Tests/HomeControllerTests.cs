using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealTimePolls.Controllers;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HomeUnitTests
{
    public class HomeControllerTests
    {
        private readonly HomeController _homeController;
        private readonly IHomeRepository _homeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeControllerTests()
        {
            //Dependencies
            _logger = A.Fake<ILogger<HomeController>>();
            _homeRepository = A.Fake<IHomeRepository>();
            _mapper = A.Fake<IMapper>();

            // SUT
            _homeController = new HomeController(_homeRepository, _mapper);
        }

        [Fact]
        public async Task HomeController_Index_ReturnsSuccess()
        {
            // Arrange
            var polls = new List<Poll>
            {
                new Poll { Id = 1, UserId = 1, Title = "Title", FirstOption = "FirstOption", SecondOption = "SecondOption" }
            };

            var homeViewModels = polls.Select(p => new HomeViewModel { Id = p.Id, Title = p.Title, FirstOption = p.FirstOption, SecondOption = p.SecondOption }).ToList();

            A.CallTo(() => _homeRepository.Index()).Returns(Task.FromResult(polls));
            A.CallTo(() => _mapper.Map<HomeViewModel>(A<Poll>.That.Matches(p => p.Id == 1))).Returns(homeViewModels.First());

            // Act
            var result = await _homeController.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.Model.Should().BeEquivalentTo(homeViewModels);
        }
    }
}
