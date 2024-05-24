using Microsoft.AspNetCore.Mvc.ModelBinding;

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
