using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RealTimePolls.Data
{
    public class RealTimePollsAuthDbContext : IdentityDbContext
    {
        public RealTimePollsAuthDbContext(DbContextOptions<RealTimePollsAuthDbContext> options)
            : base(options) { }
    }
}
