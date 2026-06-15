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

        // B-15-02: een boek kan aan meerdere genres gekoppeld worden.
        public List<GenreDTO> Genres { get; }

        // FR-10: leesstatus als tekst ("Wil ik lezen" / "Bezig" / "Gelezen").
        public string ReadingStatus { get; }

        // FR-13: verwijzing (pad/URL) naar de omslagafbeelding. Mag leeg zijn;
        // dan toont de UI een standaardafbeelding.
        public string? CoverImage { get; }

        public BookDTO(int bookID, string name, int authorID, string authorName,
            List<GenreDTO>? genres = null, string readingStatus = "Wil ik lezen", string? coverImage = null)
        {
            BookID = bookID;
            Name = name;
            AuthorID = authorID;
            AuthorName = authorName;
            Genres = genres ?? new List<GenreDTO>();
            ReadingStatus = readingStatus;
            CoverImage = coverImage;
        }
    }
}
