using MoviesApi.Models;

namespace MoviesApi.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAll(byte generId = 0);
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie movie);
        Movie Delet(Movie movie);
        Movie Update(Movie movie);

    }
}
