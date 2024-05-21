using Microsoft.AspNetCore.Mvc.ModelBinding;
using RealTimePolls.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimePolls.Models.ViewModels
{
    public class AddPollRequest
    {

        public string Title { get; set; }

        public string FirstOption { get; set; }

        public string SecondOption { get; set; }

        public int GenreId { get; set; }

        [BindNever]
        public int? UserId { get; set; }

    }
}
