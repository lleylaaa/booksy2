using System.Collections.Generic;

namespace Interface
{
    public interface IBookRepository
    {
        List<BookDTO> GetAllBooks();
        BookDTO? GetBookById(int id);
        void AddBook(string name, string author, string genre);
        void UpdateBook(int id, string name, string author, string genre);
        void DeleteBook(int id);
    }
}
