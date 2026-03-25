using Interface;
using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public UserModel? GetUserById(int id)
        {
            var tuple = _repo.GetUserById(id);
            if (tuple == null) return null;

            var t = tuple.Value;
            return new UserModel
            {
                UserID = t.UserID,
                Name = t.Name,
                Email = t.Email
            };
        }

        public void AddUser(UserModel user)
        {
            _repo.AddUser(user.Name ?? "", user.Email ?? "");
        }
    }
}