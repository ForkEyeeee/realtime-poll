using Microsoft.EntityFrameworkCore;

namespace realTimePolls.Models
{
    public class RealTimePollsContext : DbContext
    {
        public RealTimePollsContext(DbContextOptions<RealTimePollsContext> options) : base(options)
        {
        }

        public DbSet<Poll> Polls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poll>().ToTable("poll"); // Maps the Poll entity to the "poll" table
        }
    }
}
