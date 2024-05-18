//using System.Security.Claims;
//using FakeItEasy;
//using FluentAssertions;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using realTimePolls.Controllers;
//using RealTimePolls.Data;
//using Xunit;

//namespace realTimePolls.Tests
//{
//    public class LoginControllerTests
//    {
//        private LoginController _LoginController;
//        private ILogger<LoginController> _logger;
//        private RealTimePollsContext _context; //declare variables

//        public LoginControllerTests() //constructor
//        {
//            //Dependencies
//            _logger = A.Fake<ILogger<LoginController>>();

//            var options = new DbContextOptionsBuilder<RealTimePollsContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;

//            _context = new RealTimePollsContext(options);

//            //SUT
//            _LoginController = new LoginController(_logger, _context);
//        }

//        [Fact]
//        public void LoginController_Index_ReturnsSuccess()
//        {
//            //Arrange - What do i need to bring in?

//            //Act
//            var result = _LoginController.Login();

//            //Assert - check the object
//            result.IsCompleted.Equals(true);
//        }

//        [Fact]
//        public async Task LoginController_GoogleResponse_ReturnsSuccess()
//        {
//            //Arrange - What do i need to bring in?
//            var fakeClaimsPrincipal = new ClaimsPrincipal(
//                new ClaimsIdentity(
//                    new List<Claim>
//                    {
//                        new Claim(ClaimTypes.NameIdentifier, "GoogleId123"),
//                        new Claim(ClaimTypes.Name, "Test User"),
//                        new Claim(ClaimTypes.Email, "test@example.com"),
//                    },
//                    "TestAuthentication"
//                )
//            );

//            var contextMock = new DefaultHttpContext();
//            contextMock.User = fakeClaimsPrincipal;

//            var controllerContext = new ControllerContext { HttpContext = contextMock };

//            _LoginController.ControllerContext = controllerContext;

//            // Act
//            var result = await _LoginController.GoogleResponse();

//            //Assert - check the object
//            result.Should().BeOfType<ViewResult>();
//        }
//    }
//}
