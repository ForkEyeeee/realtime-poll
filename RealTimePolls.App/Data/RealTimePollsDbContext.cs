using Microsoft.EntityFrameworkCore;
using RealTimePolls.Models.Domain;

namespace RealTimePolls.Data
{
    public class RealTimePollsDbContext : DbContext
    {
        public RealTimePollsDbContext(DbContextOptions<RealTimePollsDbContext> options)
            : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<UserPoll> UserPoll { get; set; }
        public DbSet<Genre> Genre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Technology" },
                new Genre { Id = 2, Name = "Science" },
                new Genre { Id = 3, Name = "Health & Wellness" },
                new Genre { Id = 4, Name = "Sports" },
                new Genre { Id = 5, Name = "Music" },
                new Genre { Id = 6, Name = "Literature" },
                new Genre { Id = 7, Name = "Travel" },
                new Genre { Id = 8, Name = "Food & Cooking" },
                new Genre { Id = 9, Name = "Fashion" },
                new Genre { Id = 10, Name = "Art & Design" },
                new Genre { Id = 11, Name = "Gaming" },
                new Genre { Id = 12, Name = "Education" },
                new Genre { Id = 13, Name = "Anime" },
                new Genre { Id = 14, Name = "Environment" },
                new Genre { Id = 15, Name = "Business & Finance" },
                new Genre { Id = 16, Name = "Movies & TV" },
                new Genre { Id = 17, Name = "Comedy" },
                new Genre { Id = 18, Name = "Lifestyle" },
                new Genre { Id = 19, Name = "History" },
                new Genre { Id = 20, Name = "DIY & Crafts" }
            };

            var polls = new List<Poll>
                {
                    new Poll
                    {
                        Id = 1,
                        FirstOption = "Chicken",
                        SecondOption = "Egg",
                        Title = "Which came first?",
                        GenreId = 4,
                        UserId = 1,
                    },
                    new Poll
                    {
                        Id = 2,
                        FirstOption = "First choice",
                        SecondOption = "Second choice",
                        Title = "What is your option?",
                        GenreId = 6,
                        UserId = 1,
                    },
                    new Poll
                    {
                        Id = 3,
                        FirstOption = "Summer",
                        SecondOption = "Winter",
                        Title = "Which season do you prefer?",
                        GenreId = 18,
                        UserId = 2,
                    },
                    new Poll
                    {
                        Id = 4,
                        FirstOption = "Apple",
                        SecondOption = "Android",
                        Title = "Which smartphone brand do you prefer?",
                        GenreId = 1,
                        UserId = 2,
                    },
                    new Poll
                    {
                        Id = 5,
                        FirstOption = "Pizza",
                        SecondOption = "Burger",
                        Title = "Favorite fast food?",
                        GenreId = 8,
                        UserId = 3,
                    }
                };

            var users = new List<User>
{
            new User
            {
                Id = 1,
                Email = "user1@gmail.com",
                GoogleId = "1111111",
                Name = "User One",
                ProfilePicture = "https://picsum.photos/500"
            },
            new User
            {
                Id = 2,
                Email = "user2@gmail.com",
                GoogleId = "2222222",
                Name = "User Two",
                ProfilePicture = "https://picsum.photos/500"
            },
            new User
            {
                Id = 3,
                Email = "user3@gmail.com",
                GoogleId = "3333333",
                Name = "User Three",
                ProfilePicture = "https://picsum.photos/500"
            }
        };

            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Genre>().HasData(genres);
            modelBuilder.Entity<Poll>().HasData(polls);

        }
    }
}
