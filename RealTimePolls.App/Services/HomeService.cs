using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;


namespace RealTimePolls.Repositories
{
    public class HomeService : IHomeService
    {
        private readonly RealTimePollsDbContext dbContext;
        private readonly IMapper mapper;

        public HomeService(
            RealTimePollsDbContext dbContext, IMapper mapper
            
        )
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<HomeViewModel> Index()
        {
            var polls = await dbContext.Polls
                .Include(p => p.User)
                .Include(p => p.Genre)
                .ProjectTo<PollDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            var homeViewModel = new HomeViewModel
            {
                Polls = polls
            };

            return homeViewModel;
        }

        private async Task<List<Poll>> GetVoteCounts(List<Poll> polls)
        {
            var userpolls = await dbContext.UserPoll.ToListAsync();

            foreach (var poll in polls)
            {
                poll.FirstVoteCount = userpolls.Where(up => up.PollId == poll.Id && up.Vote == true).Count();

                poll.SecondVoteCount = userpolls.Where(up => up.PollId == poll.Id && up.Vote == false).Count();
            }

            return polls;
        }

    }
}
