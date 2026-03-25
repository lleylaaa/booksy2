namespace Interface
{
    public interface IUserRepository
    {
        (int UserID, string Name, string Email)? GetUserById(int id);
        void AddUser(string name, string email);
    }
}