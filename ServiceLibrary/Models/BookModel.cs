using System.Collections.Generic;

namespace ServiceLibrary.Models
{
    public class BookModel
    {
        public int BookID { get; }
        public string Name { get; }
        public int AuthorID { get; }
        public string AuthorName { get; }
        public List<GenreModel> Genres { get; }

        public BookModel(int bookID, string name, int authorID, string authorName, List<GenreModel>? genres = null)
        {
            BookID = bookID;
            Name = name;
            AuthorID = authorID;
            AuthorName = authorName;
            Genres = genres ?? new List<GenreModel>();
        }
    }
}
