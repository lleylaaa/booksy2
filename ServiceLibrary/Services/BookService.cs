 using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using ServiceLibrary.Extensions;
using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public class BookService
    {
        private readonly IBookRepository _repo;

        public BookService(IBookRepository repo)
        {
            _repo = repo;
        }

        public List<BookModel> GetAllBooks()
        {
            return _repo.GetAllBooks()
                .Select(dto => dto.ToModel())
                .ToList();
        }

        public BookModel? GetBookById(int id)
        {
            var dto = _repo.GetBookById(id);
            if (dto == null) return null;
            return dto.ToModel();
        }

        public void AddBook(string name, string author, string genre)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Titel is verplicht.");
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Auteur is verplicht.");
            _repo.AddBook(name, author, genre);
        }

        public void UpdateBook(int id, string name, string author, string genre)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Titel is verplicht.");
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Auteur is verplicht.");
            _repo.UpdateBook(id, name, author, genre);
        }

        public void DeleteBook(int id)
        {
            _repo.DeleteBook(id);
        }
    }
}