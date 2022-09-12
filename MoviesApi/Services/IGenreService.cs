using MoviesApi.Models;

namespace MoviesApi.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAll();

        Task<Genre> GetById(byte id);
        Task<Genre> Add(Genre genre);
        Genre Delete(Genre genre);
        Genre Update(Genre genre);

        Task<bool> IsValid(byte id);
    }
}
