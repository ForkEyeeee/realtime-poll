using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimePolls.Models.Domain
{
    public class Poll
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Title { get; set; }

        public string FirstOption { get; set; }

        public string SecondOption { get; set; }

        public int GenreId { get; set; }

        //Navigation properties
        public Genre Genre { get; set; }
    }
}
