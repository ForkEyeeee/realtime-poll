namespace realTimePolls.Models
{
    public class HomePolls
    {
        public List<Poll> Polls { get; set; }

        public Poll Poll { get; set; }
        public List<string> PollTitles { get; set; }
        public string? FirstOption { get; set; }
        public string? SecondOption { get; set; }

        public UserPoll? UserPoll { get; set; }
    }
}
