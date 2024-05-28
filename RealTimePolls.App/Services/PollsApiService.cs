
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.App.Models.DTO;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;

namespace RealTimePolls.Repositories
{
    public class PollsApiService : IPollsApiService
    {
        private readonly RealTimePollsDbContext dbContext;
        private readonly IMapper mapper;

        public PollsApiService(RealTimePollsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<List<PollDto>> GetPollsListAsync()
        {
            var userPolls = await dbContext.UserPoll.ToListAsync();

            var polls = await dbContext.Polls.ProjectTo<PollDto>(mapper.ConfigurationProvider).ToListAsync();

            foreach (var poll in polls)
            {
                poll.FirstVoteCount = userPolls.Where(up => up.PollId == poll.Id && up.Vote == true).Count();
                poll.SecondVoteCount = userPolls.Where(up => up.PollId == poll.Id && up.Vote == false).Count();
            }

            return polls;
        }

        public async Task<List<GenreDto>> GetDropdownListAsync()
        {
            return await dbContext.Genre.ProjectTo<GenreDto>(mapper.ConfigurationProvider).ToListAsync();
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

            var user = await dbContext
                .User.SingleOrDefaultAsync(user => user.GoogleId == googleId);


            if (user != null)
                return user.ProfilePicture;
            else
                return string.Empty;
        }
        public async Task<List<PollDto>> GetSearchResults(string query, int genreId)
        {
            var pattern = $"%{query}%";

            var pollsQuery = dbContext.Polls
                .Include(p => p.Genre)
                .Include(p => p.User)
                .Where(p => (query != string.Empty ? EF.Functions.Like(p.Title.ToLower(), pattern.ToLower()) : true) &&
                            (genreId != 0 ? p.GenreId == genreId : true));

            var polls = await pollsQuery
                .ProjectTo<PollDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            var userPolls = await dbContext.UserPoll.ToListAsync();

            foreach (var poll in polls)
            {
                poll.FirstVoteCount = userPolls.Count(up => up.PollId == poll.Id && up.Vote == true);
                poll.SecondVoteCount = userPolls.Count(up => up.PollId == poll.Id && up.Vote == false);
            }

            return polls;
        }


    }
}
