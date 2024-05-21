using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using realTimePolls.Controllers;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public class SQLPollRepository : IPollRepository
    {
        private readonly RealTimePollsDbContext dbContext;
        private readonly IHelpersRepository helpersRepository;

        public SQLPollRepository(RealTimePollsDbContext dbContext, IHelpersRepository helpersRepository)
        {
            this.dbContext = dbContext;
            this.helpersRepository = helpersRepository;
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

        public async Task<Poll> CreatePollAsync(Poll poll)
        {
            await dbContext.AddAsync(poll);
            await dbContext.SaveChangesAsync();
            return poll;
        }

        public async Task<Poll> DeletePollAsync(int pollId)
        {
            var userPolls = await dbContext.UserPoll.Where(up => up.PollId == pollId).ToListAsync();

            if (userPolls.Any())
            {
                dbContext.UserPoll.RemoveRange(userPolls);
                await dbContext.SaveChangesAsync();
            }

            var poll = dbContext.Polls.SingleOrDefault(p => p.Id == pollId);
            if (poll == null)
                return null;

            dbContext.Polls.Remove(poll);
            await dbContext.SaveChangesAsync();
            return poll;

        }

        public async Task<UserPoll> VoteAsync(AuthenticateResult result, AddVoteRequest addVoteRequest)
        {
            bool userVoter;
            var currentUserId = await helpersRepository.GetUserId(result);
            var userPoll = new UserPoll();

            if (addVoteRequest.Vote == "Vote First")
                userVoter = true;
            else if (addVoteRequest.Vote == "Vote Second")
                userVoter = false;
            else
                throw new Exception("Unable to vote");

            var userPolls = await dbContext.UserPoll.ToListAsync();

            var currentUserPoll = userPolls.FirstOrDefault(up =>
                up.PollId == addVoteRequest.PollId && up.UserId == currentUserId);

            if (currentUserPoll == null)
            {
                userPoll.UserId = currentUserId;
                userPoll.PollId = addVoteRequest.PollId;
                userPoll.Vote = userVoter;
                await dbContext.UserPoll.AddAsync(userPoll);
            }
            else
            {
                currentUserPoll.Vote = userVoter;
            }

            await dbContext.SaveChangesAsync();
            await helpersRepository.SendMessage();

            return userPoll;
        }


    }
}
