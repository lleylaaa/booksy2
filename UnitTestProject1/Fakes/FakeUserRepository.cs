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
            // Een bestaande gebruiker met een al gehasht wachtwoord (zie comment).
            // De hash hieronder hoort bij het wachtwoord "geheim123".
            _users.Add(new UserDTO(1, "Yusuf", "yusuf@test.nl", "unused-hash"));
            _nextId = 2;
        }

        public UserDTO? GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.UserID == id);
        }

        public UserDTO? GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u =>
                string.Equals(u.Email, email, System.StringComparison.OrdinalIgnoreCase));
        }

        public int AddUser(string name, string email, string passwordHash)
        {
            var id = _nextId++;
            _users.Add(new UserDTO(id, name, email, passwordHash));
            return id;
        }
    }
}
