namespace Interface
{
    public interface IUserRepository
    {
        UserDTO? GetUserById(int id);
        UserDTO? GetUserByEmail(string email);
        int AddUser(string name, string email, string passwordHash);
    }
}
