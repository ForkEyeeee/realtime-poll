using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Tracing;
using RealTimePolls.Models.Domain;

namespace RealTimePolls.Models
{
    public class PollItem
    {
        public Poll Poll { get; set; }
        public int FirstVoteCount { get; set; }
        public int SecondVoteCount { get; set; }

        [NotMapped]
        public virtual bool? Vote { get; set; }

        [NotMapped]
        public virtual string? UserName { get; set; }

        [NotMapped]
        public virtual string? ProfilePicture { get; set; }

        [NotMapped]
        public string? ErrorMsg { get; set; }

        [NotMapped]
        public string? Data { get; set; }

        [NotMapped]
        public virtual string EnvironmentName { get; set; }
    }
}
