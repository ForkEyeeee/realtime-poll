using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;


namespace RealTimePolls.Repositories
{
    public class SQLHomeRepository : IHomeRepository
    {
        private readonly RealTimePollsDbContext dbContext;

        public SQLHomeRepository(
            RealTimePollsDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Poll>> Index()
        {
            var domainPolls = await dbContext.Polls.Include(p => p.User).Include(p => p.Genre).ToListAsync();

            domainPolls = await this.GetVoteCounts(domainPolls);

            return domainPolls;
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
