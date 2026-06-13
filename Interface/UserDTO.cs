namespace Interface
{
    public class UserDTO
    {
        public int UserID { get; }
        public string Name { get; }
        public string Email { get; }

        // Het wachtwoord slaan we nooit als platte tekst op, alleen als hash.
        public string PasswordHash { get; }

        public UserDTO(int userID, string name, string email, string passwordHash)
        {
            UserID = userID;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}
