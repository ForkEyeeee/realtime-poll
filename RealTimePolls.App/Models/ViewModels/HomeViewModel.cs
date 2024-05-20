using Microsoft.AspNetCore.Mvc;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimePolls.Models.ViewModels
{
    public class HomeViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string FirstOption { get; set; }

        public string SecondOption { get; set; }

        [NotMapped]
        public int FirstVoteCount { get; set; }
        [NotMapped]
        public int SecondVoteCount { get; set; }
        [NotMapped]
        public string ProfilePicture { get; set; }

        public int GenreId { get; set; }
        public int UserId { get; set; }

        //Navigation properties
        public User User { get; set; }
        public Genre Genre { get; set; }
    }
}
