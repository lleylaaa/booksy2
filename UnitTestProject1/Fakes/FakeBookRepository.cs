using System.Collections.Generic;
using Interface;

namespace UnitTestProject1.Fakes
{
    public class FakeBookRepository : IBookRepository
    {
        private List<(int BookID, string Name, string Author, string Genre, int? Rating)> _books = new();
        private int _nextId = 1;

        public FakeBookRepository()
        {
            _books.Add((1, "Test Boek 1", "Auteur 1", "Fictie", 4));
            _books.Add((2, "Test Boek 2", "Auteur 2", "Non-Fictie", 5));
            _nextId = 3;
        }

        public void AddBook(string name, string author, string genre, int? rating)
        {
            _books.Add((_nextId++, name, author, genre, rating));
        }

        public void DeleteBook(int id)
        {
            var index = _books.FindIndex(b => b.BookID == id);
            if (index >= 0)
            {
                _books.RemoveAt(index);
            }
        }

        public List<(int BookID, string Name, string Author, string Genre, int? Rating)> GetAllBooks()
        {
            return _books;
        }

        public (int BookID, string Name, string Author, string Genre, int? Rating)? GetBookById(int id)
        {
            var index = _books.FindIndex(b => b.BookID == id);
            if (index >= 0) return _books[index];
            return null;
        }

        public void UpdateBook(int id, string name, string author, string genre, int? rating)
        {
            var index = _books.FindIndex(b => b.BookID == id);
            if (index >= 0)
            {
                _books[index] = (id, name, author, genre, rating);
            }
        }
    }
}
