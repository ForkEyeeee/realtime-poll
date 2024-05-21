using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Models;
using RealTimePolls.Models.Domain;
using SignalRChat.Hubs;

namespace RealTimePolls.Repositories
{
    public class SQLHelpersRepository : IHelpersRepository
    {
        private readonly RealTimePollsDbContext dbContext;
        private readonly IHubContext<PollHub> myHubContext;

        public SQLHelpersRepository(RealTimePollsDbContext dbContext, IHubContext<PollHub> myHubContext)
        {
            this.dbContext = dbContext;
            this.myHubContext = myHubContext;
        }
        public async Task SendMessage()
        {
            await myHubContext.Clients.All.SendAsync("ReceiveMessage");
            return;
        }

        public async Task<int> GetUserId(AuthenticateResult result)
        {

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

            var googleId = claims
                .FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                )
                .Value;

            var user = await dbContext.User.FirstOrDefaultAsync(u => u.GoogleId == googleId);
            return user.Id;
        }
    }
}

