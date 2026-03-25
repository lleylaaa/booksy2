using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public interface IBookService
    {
        List<BookModel> GetAllBooks();
        BookModel? GetBookById(int id);
        void AddBook(BookModel book);
        void UpdateBook(BookModel book);
        void DeleteBook(int id);
        void SetRating(int bookId, int rating);
    }
}
