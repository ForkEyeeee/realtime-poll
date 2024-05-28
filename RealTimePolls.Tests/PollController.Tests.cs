using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealTimePolls.Controllers;
using RealTimePolls.Data;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;
using SignalRChat.Hubs;
using Xunit;

namespace PollUnitTests
{
    public class PollControllerTests
    {
        private readonly PollController _pollController;
        private readonly RealTimePollsDbContext _dbContext;
        private readonly IHubContext<PollHub> _myHubContext;
        private readonly IPollService _pollService;
        private readonly IHelpersService _helpersService;
        private readonly ILogger<PollController> _logger;

        public PollControllerTests()
        {
            var options = new DbContextOptionsBuilder<RealTimePollsDbContext>()
                .UseInMemoryDatabase(databaseName: "RealTimePollsTestDb")
                .Options;
            _dbContext = new RealTimePollsDbContext(options);

            _myHubContext = A.Fake<IHubContext<PollHub>>();
            _pollService = A.Fake<IPollService>();
            _helpersService = A.Fake<IHelpersService>();
            _logger = A.Fake<ILogger<PollController>>();

            _pollController = new PollController(_dbContext, _myHubContext, _pollService, _helpersService, _logger);

            var httpContext = new DefaultHttpContext();
            _pollController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task Index_ShouldReturnViewResult()
        {
            var pollViewModel = new PollViewModel
            {
                Poll = new PollDto { Id = 1, Title = "Test Poll", FirstOption = "Option 1", SecondOption = "Option 2" },
                FirstVoteCount = 0,
                SecondVoteCount = 0,
                Vote = null
            };

            A.CallTo(() => _pollService.Index("Test Poll", 1, 1)).Returns(Task.FromResult(pollViewModel));

            var result = await _pollController.Index("Test Poll", 1, 1);

            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.Model.Should().BeEquivalentTo(pollViewModel);
        }  

        [Fact]
        public async Task DeletePollAsync_ShouldReturnOkResult()
        {
            A.CallTo(() => _pollService.DeletePollAsync(1)).Returns(Task.CompletedTask);

            var result = await _pollController.DeletePollAsync(1);

            result.Should().BeOfType<OkResult>();
        }

    }
}
