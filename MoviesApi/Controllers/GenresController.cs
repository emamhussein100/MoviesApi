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
    public class GenresController : ControllerBase
    {

        private readonly IGenreService _genreService;
        public GenresController( IGenreService genreService)
        {
          _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genreService.GetAll();
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateGenresDto dto)
        {
            var genre = new Genre { Name = dto.Name };
            await _genreService.Add(genre);
            return Ok(genre);
        }
        [HttpPut(template:"{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] CreateGenresDto dto)
        {
            var genre = await _genreService.GetById(id);
            if(genre == null)
            {
                return NotFound(value:$"No genre was found with id: {id}");
            }
            genre.Name = dto.Name;
            _genreService.Update(genre);
            return Ok(genre);
        }

        [HttpDelete(template:"{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
            {
                return NotFound(value: $"No genre was found with id: {id}");
            }
            _genreService.Delete(genre);
            return Ok(genre);
        }

    }
}
