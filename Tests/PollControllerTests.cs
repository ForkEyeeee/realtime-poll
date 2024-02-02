using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using realTimePolls.Controllers;
using realTimePolls.Models;
using Xunit;

namespace realTimePolls.Tests
{
    public class PollControllerTests
    {
        private readonly ILogger<PollController> _logger;

        private readonly RealTimePollsContext _context; // Declare the DbContext variable

        private PollController _PollController;

        public PollControllerTests()
        {
            _logger = A.Fake<ILogger<PollController>>();
            var options = new DbContextOptionsBuilder<RealTimePollsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new RealTimePollsContext(options);

            _PollController = new PollController(_logger, _context);
        }

        [Fact]
        public void PollController_Index_ReturnsSuccess()
        {
            //Arrange - What do i need to bring in?
            var poll = new Poll
            {
                UserId = 1,
                Title = "Title",
                FirstOption = "FirstOption",
                SecondOption = "SecondOption"
            };

            //Act
            _context.Polls.Add(poll);
            _context.SaveChanges();
            var pollId = (
                _context.Polls.FirstOrDefault(poll => poll.UserId == poll.UserId).Id
            ).ToString();
            var result = _PollController.Index(poll.Title, pollId, (poll.UserId).ToString());

            //Assert - check the object
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void PollController_Create_ReturnsSuccess()
        {
            //Arrange - What do i need to bring in?

            var formData = new Dictionary<string, StringValues>
            {
                { "Title", "Title" },
                { "FirstOption", "Option 1" },
                { "SecondOption", "Option 2" }
            };

            User user = new User
            {
                Id = 1,
                Name = "Title",
                Email = "Email",
                GoogleId = string.Empty
            };

            IFormCollection formCollection = new FormCollection(formData);

            //Act
            _context.User.Add(user);
            _context.SaveChanges();
            var result = _PollController.Create(formCollection);

            //Assert - check the object
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}
