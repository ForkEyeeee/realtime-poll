
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;

namespace RealTimePolls.Repositories
{
    public class SQLPollsApiRepository : IPollsApiRepository
    {
        private readonly RealTimePollsDbContext dbContext;

        public SQLPollsApiRepository(RealTimePollsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Poll>> GetPollsListAsync()
        {
            var polls = await dbContext.Polls.Include(p => p.User).Include(p => p.Genre).ToListAsync();

            return polls;
        }

        public async Task<List<Genre>> GetDropdownListAsync()
        {
            return await dbContext.Genre.OrderBy(g => g.Name).ToListAsync();
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

    }
}
