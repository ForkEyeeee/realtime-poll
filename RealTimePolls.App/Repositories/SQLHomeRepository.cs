using Microsoft.AspNetCore.Mvc;
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

        public Task<IActionResult> GetDropdownList()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetPolls()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserProfilePicture()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Poll>> Index()
        {
            var polls = await dbContext.Polls.Include("Genre").Include("User").ToListAsync();

            return polls;
        }
    }
}
