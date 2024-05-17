using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimePolls.Models.Domain
{
    public class UserPoll
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PollId { get; set; }

        public bool? Vote { get; set; }
    }
}
