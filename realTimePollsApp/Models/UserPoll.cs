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

        [Column("userid")] //fk
        public int UserId { get; set; }

        [NotMapped]
        [ForeignKey("UserId")] //navigation property
        [Column("userid")]
        public User User { get; set; }

        [Column("pollid")] //fk
        public int PollId { get; set; }

        [NotMapped]
        [ForeignKey("PollId")] //navigation property
        [Column("pollid")]
        public Poll Poll { get; set; }

        [Column("vote")]
        public bool? Vote { get; set; }
    }
}
