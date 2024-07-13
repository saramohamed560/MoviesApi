using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.BLL.Interfaces;
using Movies.DAL.Entities;
using Movies.PL.DTOs;

namespace Movies.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedLength = 1048576;

        public MoviesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoviesDetailsDto>>> GetAllMovies()
        {
            var movies = await _unitOfWork.MovieRepository.GetAllAsync();
            var mappedMovies = _mapper.Map<IEnumerable<Movie>, IEnumerable<MoviesDetailsDto>>(movies);
            return Ok(mappedMovies);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetByIdAsync(int id )
        {
            var movie = await _unitOfWork.MovieRepository.GetByIdAsync(id);
            if (movie == null)
                return NotFound($"Movie With Id {id} Not Found");
            var mappedMovie = _mapper.Map<Movie, MoviesDetailsDto>(movie);
            return Ok(mappedMovie);
        }

        [HttpGet("GetByCategoryId")]
        public async Task<ActionResult<IEnumerable<MoviesDetailsDto>>> GetByCategoryIdAsync([FromQuery]int id)
        {
            var movies = await _unitOfWork.MovieRepository.GetByCategoryIdAsync(id);
            var mappedMovies = _mapper.Map<IEnumerable<Movie>, IEnumerable<MoviesDetailsDto>>(movies);
            return Ok(mappedMovies);
        }
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<Movie>> CreateMovie([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster Field is required");
            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .jpg , .png images are allowed");
            if(dto.Poster.Length>_maxAllowedLength)
                return BadRequest("Max Allowed Size is 1MB");
            var isValidCategory = await _unitOfWork.CategoryRepository.IsValidCategory(dto.CategoryId);
            if(! isValidCategory)
                return BadRequest("Not Valid Category");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);
            var mappedMovie = _mapper.Map<MovieDto, Movie>(dto);
            mappedMovie.Poster = dataStream.ToArray();
            await _unitOfWork.MovieRepository.AddAsync(mappedMovie);
            await _unitOfWork.Complete();
            return Ok(mappedMovie);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]

        public async Task<ActionResult<Movie>> UpdateAsync (int id,[FromForm]MovieDto dto)
        {
            var movie = await _unitOfWork.MovieRepository.GetByIdAsync(id);
            if (movie == null)
                return NotFound($"movie With Id {id} Not Found");
            var isValidCategory = await _unitOfWork.CategoryRepository.IsValidCategory(dto.CategoryId);
            if (!isValidCategory)
                return BadRequest("Not Valid Category");
           if(dto.Poster != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .jpg , .png images are allowed");
                if (dto.Poster.Length > _maxAllowedLength)
                    return BadRequest("Max Allowed Size is 1MB");
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster=dataStream.ToArray();
            }
          
            movie.Title = dto.Title;
            movie.CategoryId = dto.CategoryId;
            movie.Year = dto.Year;
            movie.StoreLine = dto.StoreLine;
            movie.Rate = dto.Rate;
            _unitOfWork.MovieRepository.Update(movie);
            await _unitOfWork.Complete();
            return (movie);

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteAsync(int id)
        {
            var movie = await _unitOfWork.MovieRepository.GetByIdAsync(id);
            if (movie == null)
                return NotFound($"movie With Id {id} Not Found");
            _unitOfWork.MovieRepository.Remove(movie);
            await _unitOfWork.Complete();
            return Ok(movie);

        }
    }
}
