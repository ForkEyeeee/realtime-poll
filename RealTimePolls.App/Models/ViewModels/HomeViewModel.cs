using Microsoft.AspNetCore.Mvc;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;

namespace RealTimePolls.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Poll>? Polls { get; set; }
    }
}
