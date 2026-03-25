using System.Collections.Generic;
using Interface;

namespace UnitTestProject1.Fakes
{
    public class FakeUserRepository : IUserRepository
    {
        private List<(int UserID, string Name, string Email)> _users = new();
        private int _nextId = 1;

        public FakeUserRepository()
        {
            _users.Add((1, "Yusuf", "yusuf@test.nl"));
            _nextId = 2;
        }

        public void AddUser(string name, string email)
        {
            _users.Add((_nextId++, name, email));
        }

        public (int UserID, string Name, string Email)? GetUserById(int id)
        {
            var index = _users.FindIndex(u => u.UserID == id);
            if (index >= 0) return _users[index];
            return null;
        }
    }
}
