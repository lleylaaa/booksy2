using System.Collections.Generic;
using System.Linq;
using Interface;

namespace UnitTestProject1.Fakes
{
    public class FakeBookRepository : IBookRepository
    {
        private class StoredBook
        {
            public int BookID;
            public string Name = "";
            public int AuthorID;
            public List<int> GenreIDs = new();
        }

        private List<StoredBook> _books = new();
        private readonly IAuthorRepository _authors;
        private readonly IGenreRepository _genres;
        private int _nextId = 1;

        public FakeBookRepository(IAuthorRepository authors, IGenreRepository genres)
        {
            _authors = authors;
            _genres = genres;
            _books.Add(new StoredBook { BookID = 1, Name = "Test Boek 1", AuthorID = 1, GenreIDs = new List<int> { 1 } });
            _books.Add(new StoredBook { BookID = 2, Name = "Test Boek 2", AuthorID = 2, GenreIDs = new List<int> { 2 } });
            _nextId = 3;
        }

        public void AddBook(string name, int authorId, List<int> genreIds)
        {
            _books.Add(new StoredBook
            {
                BookID = _nextId++,
                Name = name,
                AuthorID = authorId,
                GenreIDs = genreIds.ToList()
            });
        }

        public void DeleteBook(int id)
        {
            _books.RemoveAll(b => b.BookID == id);
        }

        public List<BookDTO> GetAllBooks()
        {
            return _books.Select(ToDto).ToList();
        }

        public BookDTO? GetBookById(int id)
        {
            var book = _books.FirstOrDefault(b => b.BookID == id);
            return book == null ? null : ToDto(book);
        }

        public void UpdateBook(int id, string name, int authorId, List<int> genreIds)
        {
            var book = _books.FirstOrDefault(b => b.BookID == id);
            if (book != null)
            {
                book.Name = name;
                book.AuthorID = authorId;
                book.GenreIDs = genreIds.ToList();
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
            return new BookDTO(book.BookID, book.Name, book.AuthorID, authorName, genres);
        }
    }
}
