using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RealTimePolls.Data;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;
using RealTimePolls.Repositories;

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

           domainPolls = await this.GetProfilePictures(domainPolls);

            return domainPolls;
        }

        private async Task<List<Poll>> GetVoteCounts(List<Poll> polls)
        {
            var userpolls = await dbContext.UserPoll.ToListAsync();

            foreach (var poll in polls)
            {
                poll.FirstVoteCount = userpolls.Where(up => up.PollId == up.Id && up.Vote == true).Count();

                poll.SecondVoteCount = userpolls.Where(up => up.PollId == up.Id && up.Vote == false).Count();
            }

            return polls;
        }

        private async Task<List<Poll>> GetProfilePictures(List<Poll> polls)
        {

            foreach (var poll in polls)
            {
                var user = await dbContext.User.SingleOrDefaultAsync(user => user.Id == poll.UserId);
                poll.ProfilePicture = user.ProfilePicture;
            }

            return polls;
        }
    }
}
