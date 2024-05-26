using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealTimePolls.Controllers;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;
using Xunit;

namespace PollUnitTests
{
    public class PollControllerTests
    {
        private readonly PollController _pollController;
        private readonly RealTimePollsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PollController> _logger;

        public PollControllerTests()
        {
            var options = new DbContextOptionsBuilder<RealTimePollsDbContext>()
                .UseInMemoryDatabase(databaseName: "RealTimePollsTestDb")
                .Options;
            _dbContext = new RealTimePollsDbContext(options);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Poll, PollDto>());
            _mapper = config.CreateMapper();

            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PollController>();

            _pollController = new PollController(_dbContext, null, new TestPollRepository(), null, _mapper, _logger);
        }

        [Fact]
        public async Task Index_ShouldReturnViewResult()
        {
            var poll = new Poll
            {
                Id = 1,
                Title = "Test Poll",
                UserId = 1,
                FirstOption = "Option 1",
                SecondOption = "Option 2"
            };
            await _dbContext.Polls.AddAsync(poll);
            await _dbContext.SaveChangesAsync();

            var result = await _pollController.Index("Test Poll", 1, 1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PollViewModel>(viewResult.Model);
            Assert.NotNull(model);
        }

        private class TestPollRepository : IPollRepository
        {
            public Task<PollViewModelDomain> GetPollAsync(string pollTitle, int pollId, int userId)
            {
                return Task.FromResult(new PollViewModelDomain
                {
                    Poll = new Poll
                    {
                        Id = pollId,
                        Title = pollTitle,
                        UserId = userId,
                        FirstOption = "Option 1",
                        SecondOption = "Option 2"
                    },
                    FirstVoteCount = 0,
                    SecondVoteCount = 0,
                    Vote = null
                });
            }

            public Task<Poll> CreatePollAsync(Poll poll) => Task.FromResult(poll);
            public Task DeletePollAsync(int pollId) => Task.FromResult(new Poll { Id = pollId });
            public Task<UserPoll> VoteAsync(AuthenticateResult result, AddVoteRequest addVoteRequest) => Task.FromResult(new UserPoll());
        }
    }
}
