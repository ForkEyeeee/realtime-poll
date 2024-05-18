using RealTimePolls.Models.Domain;

namespace RealTimePolls.Models.DTO
{
    public class PollDto
    {
        public Poll Poll { get; set; }

        public int FirstVoteCount { get; set; }

        public int SecondVoteCount { get; set; }

        public string UserName { get; set; }

        public string ProfilePicture { get; set; }
    }
}
