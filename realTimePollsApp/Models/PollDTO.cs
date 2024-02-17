using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Tracing;

namespace realTimePolls.Models
{
    public class PollDTO
    {
        public string Title { get; set; }

        public string FirstOption { get; set; }

        public string SecondOption { get; set; }

        public int? GenreId { get; set; }

        public string? ErrorMsg { get; set; }
    }
}
