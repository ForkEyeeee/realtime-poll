//using FakeItEasy;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using realTimePolls.Controllers;
//using realTimePolls.Models;
//using RealTimePolls.Data;
//using Xunit;

//namespace HomeUnitTests
//{
//    public class HomeControllerTests
//    {
//        private HomeController _HomeController;
//        private ILogger<HomeController> _logger;
//        private RealTimePollsContext _context; //declare variables

//        public HomeControllerTests() //constructor
//        {
//            //Dependencies
//            _logger = A.Fake<ILogger<HomeController>>();

//            var options = new DbContextOptionsBuilder<RealTimePollsContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;

//            _context = new RealTimePollsContext(options);

//            //SUT
//            _HomeController = new HomeController(_logger, _context, null);
//        }

//        [Fact]
//        public void HomeController_Index_ReturnsSuccess()
//        {
//            //Arrange - What do i need to bring in?

//            //Act
//            var result = _HomeController.Index();

//            //Assert - check the object
//            result.Should().BeOfType<Task<IActionResult>>();
//        }

//        [Fact]
//        public void HomeController_Poll_ReturnsSuccess()
//        {
//            //Arrange - What do i need to bring in?

//            var poll = new Poll
//            {
//                UserId = 1,
//                Title = "Title",
//                FirstOption = "FirstOption",
//                SecondOption = "SecondOption"
//            };

//            //Act
//            _context.Polls.Add(poll);
//            _context.SaveChanges();

//            var result = _HomeController.Poll(poll.Title);

//            //Assert - check the object
//            result.Should().BeOfType<ViewResult>();
//        }
//    }
//}
