using AutoMapper;
using RealTimePolls.App.Models.DTO;
using RealTimePolls.Models.Domain;
using RealTimePolls.Models.DTO;
using RealTimePolls.Models.ViewModels;

namespace RealTimePolls.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Poll, PollDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<List<PollDto>, HomeViewModel>();
            CreateMap<AddPollRequest, Poll >();
        }
    }
}
