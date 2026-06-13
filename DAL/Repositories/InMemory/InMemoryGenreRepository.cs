using Interface;

namespace DAL.Repositories.InMemory
{
    // In-memory variant van de genre-repository. Zie InMemoryBookRepository
    // voor uitleg over de werking en het gebruik van de lock.
    public class InMemoryGenreRepository : IGenreRepository
    {
        private readonly object _lock = new();
        private readonly List<GenreDTO> _genres = new();
        private int _nextId = 1;

        public InMemoryGenreRepository()
        {
            AddGenre("Roman");
            AddGenre("Thriller");
            AddGenre("Klassieker");
        }

        public List<GenreDTO> GetAllGenres()
        {
            lock (_lock)
            {
                return _genres.ToList();
            }
        }

        public GenreDTO? GetGenreById(int id)
        {
            lock (_lock)
            {
                return _genres.FirstOrDefault(g => g.GenreID == id);
            }
        }

        public int AddGenre(string name)
        {
            lock (_lock)
            {
                var id = _nextId++;
                _genres.Add(new GenreDTO(id, name));
                return id;
            }
        }
    }
}
