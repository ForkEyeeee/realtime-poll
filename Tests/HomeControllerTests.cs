using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using realTimePolls.Controllers;
using realTimePolls.Models;
using Xunit;

namespace ControllerUnitTests
{
    public class TestParty
    {
        [Fact]
        public void Test_Entry_GET_ReturnsViewResultNullModel()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var contextMock = new Mock<RealTimePollsContext>();

            var controller = new HomeController(loggerMock.Object, contextMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
        }
    }
}
