using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RealTimePolls.Data
{
    public class RealTimePollsAuthDbContext : IdentityDbContext
    {
        public RealTimePollsAuthDbContext(DbContextOptions<RealTimePollsAuthDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var userRoleId = "a71a55d6-99d7-4123-b4e0-1218ecb90e3e";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                }

            };

            builder.Entity<IdentityRole>().HasData(roles);

        }
    }
}