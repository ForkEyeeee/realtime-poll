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

        [ForeignKey("UserId")] //fk
        [Column("userid")]
        public int UserId { get; set; }

        [NotMapped] //navigation property
        public User? User { get; set; }

        [ForeignKey("PollId")] //fk
        [Column("pollid")]
        public int PollId { get; set; }

        [NotMapped] //navigation property
        public Poll? Poll { get; set; }

        [Column("vote")]
        public bool? Vote { get; set; }
    }
}
