using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Repositories
{
    public class PollService : IPollService
    {
        private readonly RealTimePollsDbContext dbContext;
        private readonly IHelpersService helpersRepository;

        public PollService(RealTimePollsDbContext dbContext, IHelpersService helpersRepository)
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

        public async Task DeletePollAsync(int pollId)
        {
            var userPolls = await dbContext.UserPoll.Where(up => up.PollId == pollId).ToListAsync();

            if (userPolls.Any())
            {
                dbContext.UserPoll.RemoveRange(userPolls);
            }

            var poll = dbContext.Polls.SingleOrDefault(p => p.Id == pollId);

            dbContext.Polls.Remove(poll);
            return;
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
