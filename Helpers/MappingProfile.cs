﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NETCoreMoviesAPI.Dtos;
using NETCoreMoviesAPI.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile(GeometryFactory geometryFactory)
        {
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<GenreCreationDto,Genre>();

            CreateMap<Review, ReviewDto>()
                .ForMember(rdto => rdto.UserName, r => r.MapFrom(r => r.User.UserName));

            CreateMap<ReviewDto,Review >();
            CreateMap<ReviewCreationDto,Review >();

            CreateMap<IdentityUser, UserDto>();

            CreateMap<Theater, TheaterDto>()
                .ForMember(tdto => tdto.Latitude, tdto => tdto.MapFrom(t => t.Location.Y))
                .ForMember(tdto => tdto.Longitude, tdto => tdto.MapFrom(t => t.Location.X));

            CreateMap<TheaterDto, Theater>()
                .ForMember(t => t.Location, t => t.MapFrom(tdto => geometryFactory.CreatePoint(new Coordinate(tdto.Longitude, tdto.Latitude))));

            CreateMap<TheaterCreationDto, Theater>()
                .ForMember(t => t.Location, t => t.MapFrom(tdto => geometryFactory.CreatePoint(new Coordinate(tdto.Longitude, tdto.Latitude))));

            CreateMap<Actor,ActorDto>().ReverseMap();
            CreateMap<ActorCreationDto, Actor>()
                .ForMember(x => x.Photo, opt => opt.Ignore());
            CreateMap<Actor, ActorPatchDto>().ReverseMap();

            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<MovieCreationDto, Movie>()
                .ForMember(x => x.Poster, opt => opt.Ignore())
                .ForMember(x => x.MoviesGenres, opt => opt.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MoviesActors, opt => opt.MapFrom(MapMoviesActors));

            CreateMap<Movie, MoviesDetailsDto>()
                .ForMember(g => g.Genres, opt => opt.MapFrom(MapMoviesGenres))
                .ForMember(a => a.Actors, opt => opt.MapFrom(MapMoviesActors));

            CreateMap<Movie, MoviePatchDto>().ReverseMap();
        }

        private List<MoviesGenres> MapMoviesGenres(MovieCreationDto movieCreationDto, Movie movie)
        {
            var result = new List<MoviesGenres>();

            if (movieCreationDto.GenresId == null)
                return result;

            foreach (var id in movieCreationDto.GenresId)
            {
                result.Add(new MoviesGenres() { GenreId = id });
            }

            return result;
        }        
        
        private List<MoviesActors> MapMoviesActors(MovieCreationDto movieCreationDto, Movie movie)
        {
            var result = new List<MoviesActors>();

            if (movieCreationDto.Actors == null)
                return result;

            foreach (var actor in movieCreationDto.Actors)
            {
                result.Add(new MoviesActors() { ActorId = actor.ActorId, Character = actor.Character});
            }

            return result;
        }
    
        private List<GenreDto> MapMoviesGenres(Movie movie,MoviesDetailsDto moviesDetailsDto)
        {
            var result = new List<GenreDto>();

            if (movie.MoviesGenres == null)
                return result;

            foreach (var genre in movie.MoviesGenres)
            {
                result.Add(new GenreDto { Id = genre.Genre.Id, Name = genre.Genre.Name });
            }

            return result;
        }    
        private List<ActorMovieDetailsDto> MapMoviesActors(Movie movie, MoviesDetailsDto moviesDetailsDto)
        {
            var result = new List<ActorMovieDetailsDto>();

            if (movie.MoviesActors == null)
                return result;

            foreach (var actors in movie.MoviesActors)
            {
                result.Add(new ActorMovieDetailsDto
                {
                    ActorId = actors.Actor.Id,
                    Character = actors.Character,
                    Name = actors.Actor.Name
                });
            }

            return result;
        }

    }
}
