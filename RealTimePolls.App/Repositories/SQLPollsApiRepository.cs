
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public class SQLPollsApiRepository : IPollsApiRepository
    {
        private readonly RealTimePollsDbContext dbContext;
        private readonly IMapper mapper;

        public SQLPollsApiRepository(RealTimePollsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<List<Poll>> GetPollsListAsync()
        {
            var polls = await dbContext.Polls.Include(p => p.User).Include(p => p.Genre).ToListAsync();

            var userpolls = await dbContext.UserPoll.ToListAsync();

            foreach (var poll in polls)
            {
                poll.FirstVoteCount = userpolls.Where(up => up.PollId == poll.Id && up.Vote == true).Count();

                poll.SecondVoteCount = userpolls.Where(up => up.PollId == poll.Id && up.Vote == false).Count();
            }

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

            var user = await dbContext
                .User.SingleOrDefaultAsync(user => user.GoogleId == googleId);


            if (user != null)
                return user.ProfilePicture;
            else
                return string.Empty;
        }

        public async Task<List<Poll>> GetSearchResults(string query)
        {
            var pattern = $"%{query}%";

            var polls = await dbContext
                .Polls.Include(p => p.Genre).Include(p => p.User)
                .Where(c => EF.Functions.Like(c.Title.ToLower(), pattern.ToLower())).ToListAsync();

            int pollLength = dbContext
                .Polls.Where(c => EF.Functions.Like(c.Title, pattern))
                .Count();

            var homeViewModel = new List<HomeViewModel>();

            foreach (var poll in polls)
            {
                homeViewModel.Add(mapper.Map<HomeViewModel>(poll));
            }

            return polls;
        }

        public async Task<List<Poll>> GetGenreResults(int genreId)
        {

            var polls = await dbContext
                .Polls.Include(p => p.Genre).Include(p => p.User)
                .Where(p => p.GenreId == genreId).ToListAsync();

            int pollLength = dbContext
                .Polls.Where(p => p.GenreId == genreId)
                .Count();

            var homeViewModel = new List<HomeViewModel>();

            foreach (var poll in polls)
            {
                homeViewModel.Add(mapper.Map<HomeViewModel>(poll));
            }

            return polls;
        }
    }
}
