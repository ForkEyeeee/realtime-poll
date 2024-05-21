using Microsoft.AspNetCore.Mvc;
using Moq;
using RealTimePolls.Controllers;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;
using Xunit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace HomeUnitTests
{
    public class LoginControllerTests
    {
        private readonly LoginController _loginController;
        private readonly Mock<ILoginRepository> _loginRepositoryMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;

        public LoginControllerTests()
        {
            _loginRepositoryMock = new Mock<ILoginRepository>();
            _httpContextMock = new Mock<HttpContext>();
            _serviceProviderMock = new Mock<IServiceProvider>();

            _loginController = new LoginController(_loginRepositoryMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContextMock.Object
                }
            };
        }

        [Fact]
        public async Task GoogleResponse_ShouldReturnErrorView_WhenExceptionIsThrown()
        {
            _loginRepositoryMock.Setup(repo => repo.GoogleResponse(It.IsAny<AuthenticateResult>()))
                .Throws<Exception>();

            var actionResult = await _loginController.GoogleResponse();

            Assert.IsType<ViewResult>(actionResult);
            var viewResult = actionResult as ViewResult;
            Assert.Equal("Error", viewResult.ViewName);
            var model = viewResult.Model as ErrorViewModel;
            Assert.NotNull(model);
            Assert.False(string.IsNullOrEmpty(model.RequestId));
        }
    }
}
