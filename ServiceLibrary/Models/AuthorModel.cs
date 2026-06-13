namespace ServiceLibrary.Models
{
    public class AuthorModel
    {
        public int AuthorID { get; }
        public string Name { get; }

        public AuthorModel(int authorID, string name)
        {
            AuthorID = authorID;
            Name = name;
        }
    }
}
