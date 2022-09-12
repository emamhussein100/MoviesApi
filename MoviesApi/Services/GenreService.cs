﻿using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public class GenreService : IGenreService
    {

        private readonly ApplicationDbContext _context;
        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> Add(Genre genre)
        {
            await _context.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        public Genre Delete(Genre genre)
        {
            _context.Remove(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
          return  await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        }

        public  Task<Genre> GetById(byte id)
        {
            return _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> IsValid(byte id)
        {
          return await  _context.Genres.AnyAsync(g => g.Id == id);
        }

        public Genre Update(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChanges();
            return genre;
        }
    }
}
