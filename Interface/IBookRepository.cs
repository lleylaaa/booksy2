using System.Collections.Generic;

namespace Interface
{
    public interface IBookRepository
    {
        List<BookDTO> GetAllBooks();
        BookDTO? GetBookById(int id);
        void AddBook(string name, int authorId, List<int> genreIds, string readingStatus, string? coverImage);
        void UpdateBook(int id, string name, int authorId, List<int> genreIds, string readingStatus, string? coverImage);
        void DeleteBook(int id);
    }
}
