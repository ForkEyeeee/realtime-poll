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
            CreateMap<User, UserDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<List<Genre>, GenreDto>();
            CreateMap<List<PollDto>, HomeViewModel>();
            CreateMap<Poll, HomeViewModel>();
            CreateMap<Poll, AddPollRequest>();
        }
    }
}
