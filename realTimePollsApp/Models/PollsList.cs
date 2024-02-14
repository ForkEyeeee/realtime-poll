using System.ComponentModel.DataAnnotations.Schema;

namespace realTimePolls.Models
{
    public class PollsList
    {
        public List<PollItem> Polls { get; set; }

        [NotMapped]
        public virtual int PollCount { get; set; }

        [NotMapped]
        public virtual string? UserProfilePicture { get; set; }
    }
}
