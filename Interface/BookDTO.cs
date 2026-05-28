namespace Interface
{
    public class BookDTO
    {
        public int BookID { get; }
        public string Name { get; }
        public string Author { get; }
        public string Genre { get; }

        public BookDTO(int bookID, string name, string author, string genre)
        {
            BookID = bookID;
            Name = name;
            Author = author;
            Genre = genre;
        }
    }
}
