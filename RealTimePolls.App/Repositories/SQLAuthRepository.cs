using Microsoft.AspNetCore.Authentication;
using RealTimePolls.Data;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Models.Domain;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace RealTimePolls.Repositories
{
    public class SQLAuthRepository : IAuthRepository
    {
        private readonly RealTimePollsDbContext dbContext;

        public SQLAuthRepository(RealTimePollsDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        public async Task GoogleResponse(AuthenticateResult result)
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

            var googleIdClaim = claims.FirstOrDefault(c =>
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
            );

            var profilePicture = result.Principal.FindFirstValue("urn:google:picture");

            if (googleIdClaim != null)
            {
                var googleId = googleIdClaim.Value;
                User userWithGoogleId = await dbContext.User.SingleOrDefaultAsync(user =>
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
                    await dbContext.User.AddAsync(newUser);
                }
                else
                {
                    var existingUser = new
                    {
                        UserName = userWithGoogleId.Name,
                        UserEmail = userWithGoogleId.Email,
                        UserProfilePicture = profilePicture
                    };



                    await dbContext.SaveChangesAsync();

                    User viewModel =
                        new()
                        {
                            GoogleId = googleId,
                            Name = existingUser.UserName,
                            Email = existingUser.UserEmail,
                            ProfilePicture = existingUser.UserProfilePicture
                        };
                }
            }
            else
            {
                throw new Exception("GoogleId cannot be found");
            }
            await dbContext.SaveChangesAsync();
        }

    }
}
