using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public class SQLPollRepository : IPollRepository {
        private readonly RealTimePollsDbContext dbContext;

        public SQLPollRepository(RealTimePollsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PollViewModelDomain> GetPollAsync([FromQuery] string pollTitle, int pollId, int userId)
        {
            Poll poll = await dbContext
            .Polls.Include("Genre").FirstOrDefaultAsync(p => p.Id == pollId && p.UserId == userId && p.Title == pollTitle);

            if (poll == null)
                return null;

            int firstVoteCount = await dbContext
                    .UserPoll.Where(up => up.PollId == pollId && up.Vote == true)
                    .CountAsync(),
                secondVoteCount = await dbContext
            .UserPoll.Where(up => up.PollId == pollId && up.Vote == false)
            .CountAsync();

            UserPoll userPoll = await
                dbContext.UserPoll.FirstOrDefaultAsync(up => up.UserId == userId && up.PollId == pollId)
                ?? null;

            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    var userId = await GetUserId();
            //    var currentUser = dbContext.UserPoll.FirstOrDefault(up =>
            //        up.UserId == userId && up.PollId == pollid
            //    );
            //    ViewData["UserId"] = userId;
            //    ViewData["CurrentVote"] = currentUser == null ? null : currentUser.Vote;
            //}

            var pollViewModelDomain = new PollViewModelDomain
            {
                Poll = poll,
                FirstVoteCount = firstVoteCount,
                SecondVoteCount = secondVoteCount,
                Vote = userPoll == null ? null : userPoll.Vote,
            };
        
            return pollViewModelDomain;
        }
    }
}
