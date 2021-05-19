using AutoMapper;
using NETCoreMoviesAPI.Dtos;
using NETCoreMoviesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<GenreCreationDto,Genre>();

            CreateMap<Actor,ActorDto>().ReverseMap();
            CreateMap<ActorCreationDto, Actor>()
                .ForMember(x => x.Photo, opt => opt.Ignore());
        }
    }
}
