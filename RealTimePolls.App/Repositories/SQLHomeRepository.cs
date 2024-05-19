using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RealTimePolls.Data;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;

namespace RealTimePolls.Repositories
{
    public class SQLHomeRepository : IHomeRepository
    {
        private readonly RealTimePollsDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SQLHomeRepository(
            RealTimePollsDbContext dbContext,
            IHttpContextAccessor httpContextAccessor
        )
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Genre>> GetDropdownList()
        {
            var dropdownList = await dbContext.Genre.OrderBy(g => g.Name).ToListAsync();

            return dropdownList;
        }

        public Task<IActionResult> GetPolls()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetUserProfilePicture(AuthenticateResult result)
        {
            if (result.Principal == null)
                return string.Empty;

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

            if (claims == null || !claims.Any())
            {
                throw new ArgumentOutOfRangeException("Claims count cannot be 0");
            }

            var googleId = claims
                .FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                )
                .Value;

            string profilePicture = dbContext
                .User.SingleOrDefault(user => user.GoogleId == googleId)
                .ProfilePicture;

            if (profilePicture != null)
                return profilePicture;
            else
                return string.Empty;
        }

        public async Task<List<Poll>> Index()
        {
            var polls = await dbContext.Polls.Include("Genre").Include("User").ToListAsync();

            return polls;
        }
    }
}
