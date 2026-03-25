using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;

        public BookService(IBookRepository repo)
        {
            _repo = repo;
        }

        public List<BookModel> GetAllBooks()
        {
            return _repo.GetAllBooks().Select(tuple => new BookModel
            {
                BookID = tuple.BookID,
                Name = tuple.Name,
                Author = tuple.Author,
                Genre = tuple.Genre,
                Rating = tuple.Rating
            }).ToList();
        }

        public BookModel? GetBookById(int id)
        {
            var tuple = _repo.GetBookById(id);
            if (tuple == null) return null;

            var t = tuple.Value;
            return new BookModel
            {
                BookID = t.BookID,
                Name = t.Name,
                Author = t.Author,
                Genre = t.Genre,
                Rating = t.Rating
            };
        }

        public void AddBook(BookModel book)
        {
            ValidateBook(book);
            _repo.AddBook(book.Name ?? "", book.Author ?? "", book.Genre ?? "", book.Rating);
        }

        public void UpdateBook(BookModel book)
        {
            ValidateBook(book);
            _repo.UpdateBook(book.BookID, book.Name ?? "", book.Author ?? "", book.Genre ?? "", book.Rating);
        }

        public void DeleteBook(int id)
        {
            _repo.DeleteBook(id);
        }

        public void SetRating(int bookId, int rating)
        {
            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Rating moet tussen 1 en 5 zijn.");
            }

            var book = GetBookById(bookId);
            if (book != null)
            {
                book.Rating = rating;
                UpdateBook(book);
            }
        }

        private void ValidateBook(BookModel book)
        {
            if (string.IsNullOrWhiteSpace(book.Name))
            {
                throw new ArgumentException("Titel is verplicht.");
            }
            if (string.IsNullOrWhiteSpace(book.Author))
            {
                throw new ArgumentException("Auteur is verplicht.");
            }
        }
    }
}