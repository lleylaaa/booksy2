using Interface;

namespace DAL.Repositories.InMemory
{
    // In-memory variant van de gebruikers-repository. Zie InMemoryBookRepository
    // voor uitleg over de werking en het gebruik van de lock. Het hashen van
    // wachtwoorden gebeurt in de servicelaag, deze repository bewaart alleen wat
    // hij binnenkrijgt. De eerste gebruiker wordt bij het opstarten via de service
    // aangemaakt (zie Program.cs), zodat het wachtwoord netjes gehasht wordt.
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly object _lock = new();
        private readonly List<UserDTO> _users = new();
        private int _nextId = 1;

        public UserDTO? GetUserById(int id)
        {
            lock (_lock)
            {
                return _users.FirstOrDefault(u => u.UserID == id);
            }
        }

        public UserDTO? GetUserByEmail(string email)
        {
            lock (_lock)
            {
                return _users.FirstOrDefault(u =>
                    string.Equals(u.Email, email, System.StringComparison.OrdinalIgnoreCase));
            }
        }

        public int AddUser(string name, string email, string passwordHash)
        {
            lock (_lock)
            {
                var id = _nextId++;
                _users.Add(new UserDTO(id, name, email, passwordHash));
                return id;
            }
        }
    }
}
