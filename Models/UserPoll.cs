namespace realTimePolls.Models
{
    public class UserPoll
    {
        public int Id { get; set; } // auto-incremented pk
        public int UserId { get; set; }

        public int Poll { get; set; }
    }
}
