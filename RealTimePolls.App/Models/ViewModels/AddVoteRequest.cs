namespace RealTimePolls.Models.ViewModels
{
    public class AddVoteRequest
    {
        public int UserId { get; set; }

        public int PollId { get; set; }

        public string Vote { get; set; }

    }
}
