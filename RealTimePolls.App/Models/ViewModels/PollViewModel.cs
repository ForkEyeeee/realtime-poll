using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;

namespace RealTimePolls.Models.ViewModels
{
    public class PollViewModel
    {
        public int FirstVoteCount { get; set; }
        public int SecondVoteCount { get; set; }
        public bool? Vote { get; set; }

        //Navigation properties
        public PollDto Poll { get; set; }
    }
}
