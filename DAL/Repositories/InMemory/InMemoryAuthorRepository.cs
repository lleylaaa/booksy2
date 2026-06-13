using Interface;

namespace DAL.Repositories.InMemory
{
    // In-memory variant van de auteurs-repository. Zie InMemoryBookRepository
    // voor uitleg over de werking en het gebruik van de lock.
    public class InMemoryAuthorRepository : IAuthorRepository
    {
        private readonly object _lock = new();
        private readonly List<AuthorDTO> _authors = new();
        private int _nextId = 1;

        public InMemoryAuthorRepository()
        {
            AddAuthor("Harry Mulisch");
            AddAuthor("Herman Koch");
            AddAuthor("Gerard Reve");
        }

        public List<AuthorDTO> GetAllAuthors()
        {
            lock (_lock)
            {
                return _authors.ToList();
            }
        }

        public AuthorDTO? GetAuthorById(int id)
        {
            lock (_lock)
            {
                return _authors.FirstOrDefault(a => a.AuthorID == id);
            }
        }

        public int AddAuthor(string name)
        {
            lock (_lock)
            {
                var id = _nextId++;
                _authors.Add(new AuthorDTO(id, name));
                return id;
            }
        }
    }
}
