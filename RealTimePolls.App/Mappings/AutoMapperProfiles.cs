using AutoMapper;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Poll, PollDto>().ReverseMap();
            CreateMap<List<PollDto>, HomeViewModel>().ReverseMap();
            CreateMap<List<Poll>, HomeViewModel>().ReverseMap();
            CreateMap<Poll, HomeViewModel>().ReverseMap();
        }
    }
}
