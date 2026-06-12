using Interface;

namespace DAL.Repositories.InMemory
{
    // In-memory variant van de boeken-repository. Slaat alles op in een lijst
    // in het geheugen, zodat de app kan draaien zonder SQL Server (bijvoorbeeld
    // op een server die de Fontys-database niet kan bereiken). De data verdwijnt
    // weer zodra de applicatie herstart.
    //
    // Deze klasse wordt als singleton geregistreerd, dus meerdere requests delen
    // dezelfde lijst. Daarom beschermen we elke bewerking met een lock.
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly object _lock = new();
        private readonly List<BookDTO> _books = new();
        private int _nextId = 1;

        public InMemoryBookRepository()
        {
            AddBook("De ontdekking van de hemel", "Harry Mulisch", "Roman");
            AddBook("Het diner", "Herman Koch", "Thriller");
            AddBook("De avonden", "Gerard Reve", "Klassieker");
        }

        public List<BookDTO> GetAllBooks()
        {
            lock (_lock)
            {
                return _books.ToList();
            }
        }

        public BookDTO? GetBookById(int id)
        {
            lock (_lock)
            {
                return _books.FirstOrDefault(b => b.BookID == id);
            }
        }

        public void AddBook(string name, string author, string genre)
        {
            lock (_lock)
            {
                _books.Add(new BookDTO(_nextId++, name, author, genre));
            }
        }

        public void UpdateBook(int id, string name, string author, string genre)
        {
            lock (_lock)
            {
                var index = _books.FindIndex(b => b.BookID == id);
                if (index >= 0)
                {
                    _books[index] = new BookDTO(id, name, author, genre);
                }
            }
        }

        public void DeleteBook(int id)
        {
            lock (_lock)
            {
                _books.RemoveAll(b => b.BookID == id);
            }
        }
    }
}
