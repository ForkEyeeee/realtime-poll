
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
            var polls = await dbContext.Polls.ToListAsync();

            return polls;
        }

        public async Task<List<Genre>> GetDropdownListAsync()
        {
            return await dbContext.Genre.OrderBy(g => g.Name).ToListAsync();
        }

    }
}
