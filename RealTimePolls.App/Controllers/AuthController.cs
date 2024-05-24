using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;

namespace RealTimePolls.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRepository authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        public async Task Login()
        {
            try
            {
                await HttpContext.ChallengeAsync(
                    GoogleDefaults.AuthenticationScheme,
                    new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") }
                );
            }
            catch
            {
                RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        public async Task<IActionResult> GoogleResponse()
        {
            try
            {

                var result = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
                );

                await authRepository.GoogleResponse(result);

                return RedirectToAction("Index", "Home", new { area = "" });

            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel { RequestId = e.Message };
                return View("Error", errorViewModel);
            }
        }
    }
}
