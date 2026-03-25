using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public interface IUserService
    {
        UserModel? GetUserById(int id);
        void AddUser(UserModel user);
    }
}
