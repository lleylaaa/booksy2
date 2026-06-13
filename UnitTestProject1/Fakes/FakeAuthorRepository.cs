using System.Collections.Generic;
using System.Linq;
using Interface;

namespace UnitTestProject1.Fakes
{
    public class FakeAuthorRepository : IAuthorRepository
    {
        private List<AuthorDTO> _authors = new();
        private int _nextId = 1;

        public FakeAuthorRepository()
        {
            _authors.Add(new AuthorDTO(1, "Auteur 1"));
            _authors.Add(new AuthorDTO(2, "Auteur 2"));
            _nextId = 3;
        }

        public List<AuthorDTO> GetAllAuthors()
        {
            return _authors.ToList();
        }

        public AuthorDTO? GetAuthorById(int id)
        {
            return _authors.FirstOrDefault(a => a.AuthorID == id);
        }

        public int AddAuthor(string name)
        {
            var id = _nextId++;
            _authors.Add(new AuthorDTO(id, name));
            return id;
        }
    }
}
