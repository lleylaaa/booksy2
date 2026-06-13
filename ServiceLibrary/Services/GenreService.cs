using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using ServiceLibrary.Extensions;
using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public class GenreService
    {
        private readonly IGenreRepository _repo;

        public GenreService(IGenreRepository repo)
        {
            _repo = repo;
        }

        public List<GenreModel> GetAllGenres()
        {
            return _repo.GetAllGenres()
                .Select(dto => dto.ToModel())
                .ToList();
        }

        public GenreModel? GetGenreById(int id)
        {
            var dto = _repo.GetGenreById(id);
            if (dto == null) return null;
            return dto.ToModel();
        }

        // B-15-01: een genre moet een naam hebben van minimaal 2 tekens.
        public int AddGenre(string name)
        {
            var trimmed = (name ?? "").Trim();
            if (trimmed.Length < 2)
                throw new ArgumentException("Genrenaam moet minimaal 2 tekens bevatten.");
            return _repo.AddGenre(trimmed);
        }

        public int GetOrCreateGenre(string name)
        {
            var trimmed = (name ?? "").Trim();
            var existing = _repo.GetAllGenres()
                .FirstOrDefault(g => string.Equals(g.Name, trimmed, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
                return existing.GenreID;
            return AddGenre(trimmed);
        }
    }
}
