using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Repositories;

namespace RealTimePolls.Helpers
{
    public class GoogleId
    {
        private readonly SQLHelpersRepository helpersRepository;

        public GoogleId(SQLHelpersRepository helpersRepository)
        {
            this.helpersRepository = helpersRepository;
        }

        public async Task SendMessage()
        {
            await helpersRepository.SendMessage();
            return;
        }

        public async Task<int> GetUserId(AuthenticateResult result)
        {
            return await helpersRepository.GetUserId(result);
        }
    }
}



