using Interface;

namespace DAL.Repositories.InMemory
{
    // In-memory variant van de gebruikers-repository. Zie InMemoryBookRepository
    // voor uitleg over de werking en het gebruik van de lock.
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly object _lock = new();
        private readonly List<UserDTO> _users = new();
        private int _nextId = 1;

        public InMemoryUserRepository()
        {
            AddUser("Leyla", "leyla@booksy.nl");
        }

        public UserDTO? GetUserById(int id)
        {
            lock (_lock)
            {
                return _users.FirstOrDefault(u => u.UserID == id);
            }
        }

        public void AddUser(string name, string email)
        {
            lock (_lock)
            {
                _users.Add(new UserDTO(_nextId++, name, email));
            }
        }
    }
}
