namespace ServiceLibrary.Models
{
    public class UserModel
    {
        public int UserID { get; }
        public string Name { get; }
        public string Email { get; }

        public UserModel(int userID, string name, string email)
        {
            UserID = userID;
            Name = name;
            Email = email;
        }
    }
}
