using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace RealTimePolls.Models.ViewModels
{
    public class AddPollRequest
    {

        [Required]
        public string Title { get; set; }

        [Required]
        public string FirstOption { get; set; }

        [Required]
        public string SecondOption { get; set; }

        [Required]
        public int GenreId { get; set; }

        [BindNever]
        public int? UserId { get; set; }

    }
}
