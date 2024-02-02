using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace realTimePolls.Models
{
    public class UserPoll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column("userid")]
        public int UserId { get; set; }

        [ForeignKey("Poll")]
        [Column("poll")]
        public int PollId { get; set; }

        [Column("firstvote")]
        public bool? FirstVote { get; set; }

        [Column("secondvote")]
        public bool? SecondVote { get; set; }
    }
}
