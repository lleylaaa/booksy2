using System.Collections.Generic;

namespace Interface
{
    public interface IGenreRepository
    {
        List<GenreDTO> GetAllGenres();
        GenreDTO? GetGenreById(int id);
        int AddGenre(string name);
    }
}
