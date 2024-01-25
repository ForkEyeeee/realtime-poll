using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Diagnostics;
using realTimePolls.Models;
using System.Net;
using System.Threading.Tasks;

namespace realTimePolls.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        private readonly RealTimePollsContext _context; // Declare the DbContext variable

        public LoginController(ILogger<LoginController> logger, RealTimePollsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task Login() //redirects user to google login page
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }

        public async Task<IActionResult> GoogleResponse() //authentication
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            }).ToList();

            User newUser = null;
            string userName = null;
            string userEmail = null;

            if (claims.Count != 0)
            {
                var googleIdClaim = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

                if (googleIdClaim != null)
                {
                    var googleId = googleIdClaim.Value;
                    var userWithGoogleId = _context.User.SingleOrDefault(user => user.GoogleId == googleId);

                    if (userWithGoogleId == null)
                    {
                        userName = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                        userEmail = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

                        newUser = new User
                        {
                            GoogleId = googleId,
                            Name = userName,
                            Email = userEmail,
                        };
                        _context.User.Add(newUser);
                    }
                    else
                    {
                        
                        var existingUser = new
                        {
                            UserName = userWithGoogleId.Name,  
                            UserEmail = userWithGoogleId.Email
                        };
                        _context.SaveChanges();
                        var viewModel = new User()
                        {
                            GoogleId = googleId,
                            Name = existingUser.UserName,
                            Email = existingUser.UserEmail
                        };
                        return RedirectToAction("Index", "Home", new { area = "" });
                       // return View("../Login/index", viewModel);
                    }
                }
                else
                {
                    Debug.WriteLine("Google id === null");
                    return View();
                }
                _context.SaveChanges();

                //return RedirectToAction("Index", "Home", newUser);
                return RedirectToAction("Index", "Home", new { area = "" });

                //return View("../Login/index", newUser);

            }
            Debug.WriteLine("No claims found after logging in");
            return View("../Home/index");

        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

    }
}
  

