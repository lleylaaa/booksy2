using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using ServiceLibrary.Extensions;
using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public class AuthorService
    {
        private readonly IAuthorRepository _repo;

        public AuthorService(IAuthorRepository repo)
        {
            _repo = repo;
        }

        public List<AuthorModel> GetAllAuthors()
        {
            return _repo.GetAllAuthors()
                .Select(dto => dto.ToModel())
                .ToList();
        }

        public AuthorModel? GetAuthorById(int id)
        {
            var dto = _repo.GetAuthorById(id);
            if (dto == null) return null;
            return dto.ToModel();
        }

        // B-14-01: een auteur moet een naam hebben van minimaal 2 tekens.
        public int AddAuthor(string name)
        {
            var trimmed = (name ?? "").Trim();
            if (trimmed.Length < 2)
                throw new ArgumentException("Auteursnaam moet minimaal 2 tekens bevatten.");
            return _repo.AddAuthor(trimmed);
        }

        // Zoekt een bestaande auteur op naam, of maakt er een aan als die nog niet bestaat.
        public int GetOrCreateAuthor(string name)
        {
            var trimmed = (name ?? "").Trim();
            var existing = _repo.GetAllAuthors()
                .FirstOrDefault(a => string.Equals(a.Name, trimmed, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
                return existing.AuthorID;
            return AddAuthor(trimmed);
        }
    }
}
