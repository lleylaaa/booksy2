using Interface;
using ServiceLibrary.Extensions;
using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public UserModel? GetUserById(int id)
        {
            var dto = _repo.GetUserById(id);
            if (dto == null) return null;
            return dto.ToModel();
        }

        public void AddUser(string name, string email)
        {
            _repo.AddUser(name, email);
        }
    }
}