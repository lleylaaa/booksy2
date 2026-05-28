using System.Collections.Generic;
using System.Linq;
using Interface;

namespace UnitTestProject1.Fakes
{
    public class FakeBookRepository : IBookRepository
    {
        private List<BookDTO> _books = new();
        private int _nextId = 1;

        public FakeBookRepository()
        {
            _books.Add(new BookDTO(1, "Test Boek 1", "Auteur 1", "Fictie"));
            _books.Add(new BookDTO(2, "Test Boek 2", "Auteur 2", "Non-Fictie"));
            _nextId = 3;
        }

        public void AddBook(string name, string author, string genre)
        {
            _books.Add(new BookDTO(_nextId++, name, author, genre));
        }

        public void DeleteBook(int id)
        {
            var index = _books.FindIndex(b => b.BookID == id);
            if (index >= 0)
            {
                _books.RemoveAt(index);
            }
        }

        public List<BookDTO> GetAllBooks()
        {
            return _books.ToList();
        }

        public BookDTO? GetBookById(int id)
        {
            var index = _books.FindIndex(b => b.BookID == id);
            if (index >= 0) return _books[index];
            return null;
        }

        public void UpdateBook(int id, string name, string author, string genre)
        {
            var index = _books.FindIndex(b => b.BookID == id);
            if (index >= 0)
            {
                _books[index] = new BookDTO(id, name, author, genre);
            }
        }
    }
}
