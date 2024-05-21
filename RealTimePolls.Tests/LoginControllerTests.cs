using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealTimePolls.Controllers;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace RealTimePolls.Tests
{
    public class LoginControllerTests
    {
        private readonly LoginController _loginController;
        private readonly ILoginRepository _loginRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        public LoginControllerTests()
        {
            // Dependencies
            _loginRepository = A.Fake<ILoginRepository>();
            _httpContextAccessor = A.Fake<IHttpContextAccessor>();
            _httpContext = new DefaultHttpContext();
            _httpContextAccessor.HttpContext = _httpContext;

            // SUT
            _loginController = new LoginController(_loginRepository)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = _httpContext
                }
            };
        }

        [Fact]
        public async Task LoginController_Login_RedirectsToGoogle()
        {
            // Arrange
            A.CallTo(() => _httpContext.ChallengeAsync(
                GoogleDefaults.AuthenticationScheme,
                A<AuthenticationProperties>.Ignored)).Returns(Task.CompletedTask);

            // Act
            await _loginController.Login();

            // Assert
            A.CallTo(() => _httpContext.ChallengeAsync(
                GoogleDefaults.AuthenticationScheme,
                A<AuthenticationProperties>.That.Matches(p => p.RedirectUri == "/Login/GoogleResponse"))).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task LoginController_GoogleResponse_ReturnsRedirectToHome()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            var authenticateResult = AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, "Cookies"));

            A.CallTo(() => _httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme))
                .Returns(Task.FromResult(authenticateResult));
            A.CallTo(() => _loginRepository.GoogleResponse(authenticateResult))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _loginController.GoogleResponse();

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("Index");
            redirectResult.ControllerName.Should().Be("Home");
        }

        [Fact]
        public async Task LoginController_Logout_ReturnsRedirectToHome()
        {
            // Arrange
            A.CallTo(() => _httpContext.SignOutAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _loginController.Logout();

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("Index");
            redirectResult.ControllerName.Should().Be("Home");
        }
    }
}
