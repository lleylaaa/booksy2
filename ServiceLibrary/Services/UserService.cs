using System;
using Interface;
using ServiceLibrary.Extensions;
using ServiceLibrary.Models;
using ServiceLibrary.Security;

namespace ServiceLibrary.Services
{
    public class UserService
    {
        private const int MinPasswordLength = 6;

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

        // FR-11: registreren met e-mailadres en wachtwoord.
        public UserModel Register(string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Naam is verplicht.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("E-mailadres is verplicht.");
            // B-11-02: het wachtwoord moet minimaal 6 tekens bevatten.
            if (password == null || password.Length < MinPasswordLength)
                throw new ArgumentException($"Het wachtwoord moet minimaal {MinPasswordLength} tekens bevatten.");
            // B-11-01: het e-mailadres moet uniek zijn.
            if (_repo.GetUserByEmail(email) != null)
                throw new ArgumentException("Er bestaat al een account met dit e-mailadres.");

            var passwordHash = PasswordHasher.Hash(password);
            var id = _repo.AddUser(name, email, passwordHash);
            return new UserModel(id, name, email);
        }

        // FR-11: inloggen. Geeft de gebruiker terug bij een juist wachtwoord, anders null.
        public UserModel? Login(string email, string password)
        {
            var dto = _repo.GetUserByEmail(email);
            if (dto == null)
                return null;
            if (!PasswordHasher.Verify(password ?? "", dto.PasswordHash))
                return null;
            return dto.ToModel();
        }
    }
}
