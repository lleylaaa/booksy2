using System.Collections.Generic;
using System.Linq;
using Interface;

namespace UnitTestProject1.Fakes
{
    public class FakeUserRepository : IUserRepository
    {
        private List<UserDTO> _users = new();
        private int _nextId = 1;

        public FakeUserRepository()
        {
            _users.Add(new UserDTO(1, "Yusuf", "yusuf@test.nl"));
            _nextId = 2;
        }

        public void AddUser(string name, string email)
        {
            _users.Add(new UserDTO(_nextId++, name, email));
        }

        public UserDTO? GetUserById(int id)
        {
            var index = _users.FindIndex(u => u.UserID == id);
            if (index >= 0) return _users[index];
            return null;
        }
    }
}
