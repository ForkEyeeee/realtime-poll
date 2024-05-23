using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RealTimePolls.Repositories;

namespace realTimePolls.Controllers
{
    public class Polls : Controller
    {
        private readonly IPollsApiRepository pollsApiRepository;

        public Polls(IPollsApiRepository pollsApiRepository)
        {
            this.pollsApiRepository = pollsApiRepository;
        }

        [HttpGet]
        [Route("api/[action]")]
        public async Task<JsonResult> GetPollsListAsync()
        {
            var polls = await pollsApiRepository.GetPollsListAsync();

            return Json(polls);
        }

        [HttpGet]
        [Route("api/[action]")]
        public async Task<JsonResult> GetDropdownListAsync()
        {
            var genreOptions = await pollsApiRepository.GetDropdownListAsync();

            return Json(genreOptions);
        }

        [HttpGet]
        [Route("api/[action]")]
        public async Task<string> GetUserProfilePicture()
        {
            AuthenticateResult result = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var profilePicture = await pollsApiRepository.GetUserProfilePicture(result);

            return profilePicture;
        }


        [HttpGet]
        [Route("api/[action]")]
        public async Task<JsonResult> GetSearchResults([FromQuery] string search, int genreId)
        {

            var polls = await pollsApiRepository.GetSearchResults(search, genreId);

            return Json(polls);

        }

    }
}




