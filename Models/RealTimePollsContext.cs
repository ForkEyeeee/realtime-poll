using Microsoft.EntityFrameworkCore;

namespace realTimePolls.Models
{
    public class RealTimePollsContext : DbContext
    {
        public RealTimePollsContext(DbContextOptions<RealTimePollsContext> options) : base(options)
        {
        }

        public DbSet<Poll> Polls { get; set; }
        public DbSet<User> User { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poll>().ToTable("poll"); // Maps the Poll entity to the "poll" table
            modelBuilder.Entity<User>().ToTable("user"); // Maps the User entity to the "poll" table

        }
    }
}