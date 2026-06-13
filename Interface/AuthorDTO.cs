namespace Interface
{
    public class AuthorDTO
    {
        public int AuthorID { get; }
        public string Name { get; }

        public AuthorDTO(int authorID, string name)
        {
            AuthorID = authorID;
            Name = name;
        }
    }
}
