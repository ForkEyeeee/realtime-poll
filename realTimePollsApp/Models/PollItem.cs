using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Tracing;

namespace realTimePolls.Models
{
    public class PollItem
    {
        public Poll Poll { get; set; }
        public int FirstVoteCount { get; set; }
        public int SecondVoteCount { get; set; }

        [NotMapped]
        public virtual bool? Vote { get; set; }
    }
}
