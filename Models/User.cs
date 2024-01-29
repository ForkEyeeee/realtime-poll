namespace realTimePolls.Models
{
    public class User
    {
        public int Id { get; set; } // auto-incremented pk
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? GoogleId { get; set; }

        public int? Polls { get; set; }
    }
}
