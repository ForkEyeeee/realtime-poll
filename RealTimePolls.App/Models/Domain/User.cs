using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimePolls.Models.Domain
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string GoogleId { get; set; }

        public string ProfilePicture { get; set; }
    }
}
