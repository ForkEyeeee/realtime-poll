using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.ViewModels;

namespace realTimePolls.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        private readonly RealTimePollsDbContext _context; // Declare the DbContext variable

        public LoginController(ILogger<LoginController> logger, RealTimePollsDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Login() //redirects user to google login page
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

        public async Task<IActionResult> GoogleResponse() //authentication
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                if (result.Principal == null)
                    throw new Exception("Could not authenticate");

                var claims = result
                    .Principal.Identities.FirstOrDefault()
                    ?.Claims.Select(claim => new
                    {
                        claim.Issuer,
                        claim.OriginalIssuer,
                        claim.Type,
                        claim.Value
                    })
                    .ToList();

                User newUser;
                string? userName = null;
                string? userEmail = null;

                if (claims == null || claims.Count == 0)
                {
                    throw new ArgumentOutOfRangeException("Claims count cannot be 0");
                }

                var googleIdClaim = claims.FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                );

                var profilePicture = result.Principal.FindFirstValue("urn:google:picture");

                if (googleIdClaim != null)
                {
                    var googleId = googleIdClaim.Value;
                    User userWithGoogleId = _context.User.SingleOrDefault(user =>
                        user.GoogleId == googleId
                    );

                    if (userWithGoogleId == null)
                    {
                        userName = claims
                            .FirstOrDefault(c =>
                                c.Type
                                == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
                            )
                            ?.Value;
                        userEmail = claims
                            .FirstOrDefault(c =>
                                c.Type
                                == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                            )
                            ?.Value;

                        newUser = new User
                        {
                            GoogleId = googleId,
                            Name = userName,
                            Email = userEmail,
                            ProfilePicture = profilePicture
                        };
                        _context.User.Add(newUser);
                    }
                    else
                    {
                        var existingUser = new
                        {
                            UserName = userWithGoogleId.Name,
                            UserEmail = userWithGoogleId.Email,
                            UserProfilePicture = profilePicture
                        };

                        _context.SaveChanges();

                        User viewModel =
                            new()
                            {
                                GoogleId = googleId,
                                Name = existingUser.UserName,
                                Email = existingUser.UserEmail,
                                ProfilePicture = existingUser.UserProfilePicture
                            };

                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                }
                else
                {
                    throw new Exception("GoogleId cannot be found");
                }

                _context.SaveChanges();
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
