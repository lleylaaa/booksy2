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
        private readonly AuthorService _authors;
        private readonly GenreService _genres;

        public BookService(IBookRepository repo, AuthorService authors, GenreService genres)
        {
            _repo = repo;
            _authors = authors;
            _genres = genres;
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

        public void AddBook(string name, string authorName, IEnumerable<string> genreNames)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Titel is verplicht.");
            // B-14-02: een boek moet altijd gekoppeld zijn aan een auteur.
            if (string.IsNullOrWhiteSpace(authorName))
                throw new ArgumentException("Auteur is verplicht.");

            var authorId = _authors.GetOrCreateAuthor(authorName);
            var genreIds = ResolveGenres(genreNames);
            _repo.AddBook(name, authorId, genreIds);
        }

        public void UpdateBook(int id, string name, string authorName, IEnumerable<string> genreNames)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Titel is verplicht.");
            if (string.IsNullOrWhiteSpace(authorName))
                throw new ArgumentException("Auteur is verplicht.");

            var authorId = _authors.GetOrCreateAuthor(authorName);
            var genreIds = ResolveGenres(genreNames);
            _repo.UpdateBook(id, name, authorId, genreIds);
        }

        public void DeleteBook(int id)
        {
            _repo.DeleteBook(id);
        }

        // B-15-02: een boek kan aan meerdere genres gekoppeld worden. Lege namen
        // overslaan we, dubbele genres koppelen we maar een keer.
        private List<int> ResolveGenres(IEnumerable<string> genreNames)
        {
            return (genreNames ?? Enumerable.Empty<string>())
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => _genres.GetOrCreateGenre(n))
                .Distinct()
                .ToList();
        }
    }
}
