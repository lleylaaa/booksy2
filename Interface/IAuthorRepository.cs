using System.Collections.Generic;

namespace Interface
{
    public interface IAuthorRepository
    {
        List<AuthorDTO> GetAllAuthors();
        AuthorDTO? GetAuthorById(int id);
        int AddAuthor(string name);
    }
}
