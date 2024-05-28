using System.Collections.Generic;
using RealTimePolls.Models.DTO;

namespace RealTimePolls.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<PollDto> Polls { get; set; }
    }
}
