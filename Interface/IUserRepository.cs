namespace Interface
{
    public interface IUserRepository
    {
        UserDTO? GetUserById(int id);
        void AddUser(string name, string email);
    }
}