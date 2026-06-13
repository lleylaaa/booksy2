using System.Collections.Generic;
using System.Linq;
using Interface;

namespace UnitTestProject1.Fakes
{
    public class FakeGenreRepository : IGenreRepository
    {
        private List<GenreDTO> _genres = new();
        private int _nextId = 1;

        public FakeGenreRepository()
        {
            _genres.Add(new GenreDTO(1, "Fictie"));
            _genres.Add(new GenreDTO(2, "Non-Fictie"));
            _nextId = 3;
        }

        public List<GenreDTO> GetAllGenres()
        {
            return _genres.ToList();
        }

        public GenreDTO? GetGenreById(int id)
        {
            return _genres.FirstOrDefault(g => g.GenreID == id);
        }

        public int AddGenre(string name)
        {
            var id = _nextId++;
            _genres.Add(new GenreDTO(id, name));
            return id;
        }
    }
}
