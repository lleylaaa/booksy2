using System.Collections.Generic;

namespace Interface
{
    public class BookDTO
    {
        public int BookID { get; }
        public string Name { get; }

        // B-14-02: een boek is altijd gekoppeld aan precies een auteur.
        public int AuthorID { get; }
        public string AuthorName { get; }

        // B-15-02: een boek kan aan meerdere genres gekoppeld zijn.
        public List<GenreDTO> Genres { get; }

        public BookDTO(int bookID, string name, int authorID, string authorName, List<GenreDTO>? genres = null)
        {
            BookID = bookID;
            Name = name;
            AuthorID = authorID;
            AuthorName = authorName;
            Genres = genres ?? new List<GenreDTO>();
        }
    }
}
