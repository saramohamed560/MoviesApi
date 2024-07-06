
using AutoMapper;
using Movies.DAL.Entities;
using Movies.PL.DTOs;

namespace Movies.PL.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<MovieDto, Movie>().ForMember(d=>d.Poster,o=>o.Ignore());
            CreateMap<Movie, MoviesDetailsDto>().ForMember(d=>d.CategoryName,o=>o.MapFrom(s=>s.Category.Name));
        }
    }
}
