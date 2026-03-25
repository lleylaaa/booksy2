using System.Collections.Generic;

namespace Interface
{
    public interface IBookRepository
    {
        List<(int BookID, string Name, string Author, string Genre, int? Rating)> GetAllBooks();
        (int BookID, string Name, string Author, string Genre, int? Rating)? GetBookById(int id);
        void AddBook(string name, string author, string genre, int? rating);
        void UpdateBook(int id, string name, string author, string genre, int? rating);
        void DeleteBook(int id);
    }
}
