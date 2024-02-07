using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace realTimePolls.Models
{
    public class Poll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column("userid")]
        public int UserId { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Column("firstoption")]
        public string FirstOption { get; set; }

        [Required]
        [Column("secondoption")]
        public string SecondOption { get; set; }

        [NotMapped]
        public virtual int FirstVotes { get; set; }

        [NotMapped]
        public virtual int SecondVotes { get; set; }

        [NotMapped]
        public virtual User User { get; set; }
    }
}
