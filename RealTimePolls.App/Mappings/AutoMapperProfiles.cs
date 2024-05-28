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
            CreateMap<Poll, PollDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<List<Genre>, GenreDto>().ReverseMap();
            CreateMap<List<PollDto>, HomeViewModel>().ReverseMap();
            CreateMap<Poll, HomeViewModel>().ReverseMap();
            CreateMap<Poll, AddPollRequest>().ReverseMap();
        }
    }
}
