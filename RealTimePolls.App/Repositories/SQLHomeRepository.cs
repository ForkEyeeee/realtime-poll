using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public class SQLHomeRepository : IHomeRepository
    {
        private readonly RealTimePollsDbContext dbContext;

        public SQLHomeRepository(RealTimePollsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Poll>> Index()
        {
            var polls = await dbContext.Polls.Include("Genre").ToListAsync();

            return polls;
        }
    }
}
