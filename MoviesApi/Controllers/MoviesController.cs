using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Dtos;
using MoviesApi.Models;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;

        private new List<string> _allowedExtentions = new List<string> {".jpg",".png"};
        private long _maxAllowedSize = 1048576;
        public MoviesController(IMovieService movieService, IGenreService genreService, IMapper mapper)
        {
            _movieService = movieService;
            _genreService = genreService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _movieService.GetAll();
            
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);

            return Ok(data);
        }

        [HttpGet(template: "{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _movieService.GetById(id);
            if (movie == null)
            {
                return NotFound("The Id Not Founded");
            }
            var dto = _mapper.Map<MovieDetailsDto>(movie);
            return Ok(dto);
        }
        
        [HttpGet(template:"GetByGenreId")]
        public async Task<IActionResult> GetGenreIdAsync(byte genreId)
        {
            var movies = await _movieService.GetAll(genreId);
            // Todo: Map Movie To DTO
            return Ok(movies);
        }
        

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if(dto.Poster == null)
            {
                return BadRequest("The Poster Is Required!");
            }
            if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))         
                return BadRequest(error: "Only jpg and png images are allowed!");
           
            if(dto.Poster.Length > _maxAllowedSize)
               return BadRequest(error: "Max size is 1MB!");


            var IsValidGenre = await _genreService.IsValid(dto.GenreId);

            if (!IsValidGenre)
                 return BadRequest(error: "Invalid Genre ID!");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();

            _movieService.Add(movie);
            return Ok(movie);
        }

        [HttpPut(template:"{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _movieService.GetById(id);

            var IsValidGenre = await _genreService.IsValid(dto.GenreId);

            if (!IsValidGenre)
                return BadRequest(error: "Invalid Genre ID!");

            if(dto.Poster != null)
            {
                if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest(error: "Only jpg and png images are allowed!");

                if (dto.Poster.Length > _maxAllowedSize)
                    return BadRequest(error: "Max size is 1MB!");
                
                using var dataStream = new MemoryStream();

                await dto.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();
            }

            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.StoryLine = dto.StoryLine;
            movie.Year = dto.Year;
            movie.Rate = dto.Rate;

            _movieService.Update(movie);

            return Ok(movie);

        }


        [HttpDelete(template:"{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _movieService.GetById(id);

            if (movie == null)
                return NotFound(value:$"No Movie Was Found With Id : {id}");
            _movieService.Delet(movie);
            return Ok(movie);
        }

    }
}
