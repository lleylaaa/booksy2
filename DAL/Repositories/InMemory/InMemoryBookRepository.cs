using Interface;

namespace DAL.Repositories.InMemory
{
    // In-memory variant van de boeken-repository. Slaat alles op in een lijst
    // in het geheugen, zodat de app kan draaien zonder SQL Server (bijvoorbeeld
    // op een server die de Fontys-database niet kan bereiken). De data verdwijnt
    // weer zodra de applicatie herstart.
    //
    // Deze klasse wordt als singleton geregistreerd, dus meerdere requests delen
    // dezelfde lijst. Daarom beschermen we elke bewerking met een lock. De auteur-
    // en genrenamen halen we op uit de bijbehorende repositories, want auteur en
    // genre zijn nu aparte entiteiten.
    public class InMemoryBookRepository : IBookRepository
    {
        // Interne opslag van een boek: het boek bewaart alleen de id's van zijn
        // auteur en genres, de namen leiden we af via de andere repositories.
        private class StoredBook
        {
            public int BookID;
            public string Name = "";
            public int AuthorID;
            public List<int> GenreIDs = new();
            public string ReadingStatus = "Wil ik lezen";
            public string? CoverImage;
        }

        private readonly object _lock = new();
        private readonly List<StoredBook> _books = new();
        private readonly IAuthorRepository _authors;
        private readonly IGenreRepository _genres;
        private int _nextId = 1;

        public InMemoryBookRepository(IAuthorRepository authors, IGenreRepository genres)
        {
            _authors = authors;
            _genres = genres;

            // De seed-auteurs/genres staan op id 1, 2 en 3 in hun repositories.
            AddBook("De ontdekking van de hemel", 1, new List<int> { 1 }, "Gelezen", null);
            AddBook("Het diner", 2, new List<int> { 2 }, "Bezig", null);
            AddBook("De avonden", 3, new List<int> { 3 }, "Wil ik lezen", null);
        }

        public List<BookDTO> GetAllBooks()
        {
            lock (_lock)
            {
                return _books.Select(ToDto).ToList();
            }
        }

        public BookDTO? GetBookById(int id)
        {
            lock (_lock)
            {
                var book = _books.FirstOrDefault(b => b.BookID == id);
                return book == null ? null : ToDto(book);
            }
        }

        public void AddBook(string name, int authorId, List<int> genreIds, string readingStatus, string? coverImage)
        {
            lock (_lock)
            {
                _books.Add(new StoredBook
                {
                    BookID = _nextId++,
                    Name = name,
                    AuthorID = authorId,
                    GenreIDs = genreIds.ToList(),
                    ReadingStatus = readingStatus,
                    CoverImage = coverImage
                });
            }
        }

        public void UpdateBook(int id, string name, int authorId, List<int> genreIds, string readingStatus, string? coverImage)
        {
            lock (_lock)
            {
                var book = _books.FirstOrDefault(b => b.BookID == id);
                if (book != null)
                {
                    book.Name = name;
                    book.AuthorID = authorId;
                    book.GenreIDs = genreIds.ToList();
                    book.ReadingStatus = readingStatus;
                    book.CoverImage = coverImage;
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

        private BookDTO ToDto(StoredBook book)
        {
            var authorName = _authors.GetAuthorById(book.AuthorID)?.Name ?? "";
            var genres = book.GenreIDs
                .Select(_genres.GetGenreById)
                .Where(g => g != null)
                .Select(g => g!)
                .ToList();
            return new BookDTO(book.BookID, book.Name, book.AuthorID, authorName, genres,
                book.ReadingStatus, book.CoverImage);
        }
    }
}
