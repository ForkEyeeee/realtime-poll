using System.ComponentModel.DataAnnotations.Schema;
using RealTimePolls.Models;

namespace realTimePolls.Models
{
    public class PollsList
    {
        public List<PollItem> Polls { get; set; }

        [NotMapped]
        public virtual int PollCount { get; set; }

        [NotMapped]
        public virtual string? UserProfilePicture { get; set; }

        [NotMapped]
        public virtual string? EnvironmentName { get; set; }
    }
}
