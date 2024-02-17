using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace realTimePolls.Models
{
    public class Poll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [ForeignKey("user")]
        [Column("userid")]
        public int UserId { get; set; }

        [NotMapped] //navigation property
        public User User { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Column("firstoption")]
        public string FirstOption { get; set; }

        [Required]
        [Column("secondoption")]
        public string SecondOption { get; set; }

        [ForeignKey("genre")]
        [Column("genreid")]
        public int? GenreId { get; set; }

        public Genre Genre { get; set; }

        [NotMapped]
        public string? ErrorMsg { get; set; }
    }
}
