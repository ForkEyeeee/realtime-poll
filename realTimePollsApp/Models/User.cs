using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace realTimePolls.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; } // auto-incremented pk

        [Column("email")]
        public string? Email { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [NotMapped] //navigation property
        public Poll Poll { get; set; }

        [Column("googleid")]
        public string? GoogleId { get; set; }

        [Column("profilepicture")]
        public string? ProfilePicture { get; set; }
    }
}
