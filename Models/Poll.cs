namespace realTimePolls.Models
{
    public class Poll
    {
        public int Id { get; set; } // auto-incremented pk
        public string Name { get; set; } 
        public string FirstOption { get; set; } 
        public string SecondOption { get; set; } 

    }
}
