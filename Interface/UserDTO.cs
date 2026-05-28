namespace Interface
{
    public class UserDTO
    {
        public int UserID { get; }
        public string Name { get; }
        public string Email { get; }

        public UserDTO(int userID, string name, string email)
        {
            UserID = userID;
            Name = name;
            Email = email;
        }
    }
}
