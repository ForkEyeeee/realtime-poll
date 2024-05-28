using RealTimePolls.Models.Domain;

namespace RealTimePolls.Models.DTO
{
    public class PollDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string FirstOption { get; set; }
        public string SecondOption { get; set; }
        public int FirstVoteCount { get; set; }
        public int SecondVoteCount { get; set; }   
        public int GenreId { get; set; }

        //Navigation properties
        public Genre Genre { get; set; }

        public User User { get; set; }
    }
}
