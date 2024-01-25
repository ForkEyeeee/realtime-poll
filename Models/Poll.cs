namespace realTimePolls.Models
{
    public class Poll
    {
        public int Id { get; set; } // auto-incremented pk
        public int UserId { get; set; } //User fk
        public string Title { get; set; }

        public string FirstOption { get; set; } 
        public string SecondOption { get; set; } 

    }
}
