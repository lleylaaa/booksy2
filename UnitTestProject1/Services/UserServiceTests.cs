using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Services;
using ServiceLibrary.Models;
using UnitTestProject1.Fakes;
using System;

namespace UnitTestProject1.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private UserService _userService = null!;
        private FakeUserRepository _fakeRepo = null!;

        [TestInitialize]
        public void Setup()
        {
            // Arrange
            _fakeRepo = new FakeUserRepository();
            _userService = new UserService(_fakeRepo);
        }

        [TestMethod]
        public void GetUserById_ReturnsCorrectUser()
        {
            // Arrange
            int expectedId = 1;
            string expectedName = "Yusuf";

            // Act
            var result = _userService.GetUserById(expectedId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedName, result.Name);
            Assert.AreEqual(expectedId, result.UserID);
        }

        [TestMethod]
        public void GetUserById_ReturnsNull_WhenNotFound()
        {
            // Arrange
            int nonExistingId = 99;

            // Act
            var result = _userService.GetUserById(nonExistingId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Register_CreatesUser_WhenValid()
        {
            // FR-11: registreren met e-mailadres en wachtwoord.
            // Act
            var user = _userService.Register("Nieuwe Gebruiker", "nieuw@test.nl", "geheim123");

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual("Nieuwe Gebruiker", user.Name);
            Assert.AreEqual("nieuw@test.nl", user.Email);
        }

        [TestMethod]
        public void Register_HashesPassword_NeverStoresPlaintext()
        {
            // Act
            _userService.Register("Nieuwe Gebruiker", "nieuw@test.nl", "geheim123");

            // Assert
            var stored = _fakeRepo.GetUserByEmail("nieuw@test.nl");
            Assert.IsNotNull(stored);
            Assert.AreNotEqual("geheim123", stored.PasswordHash);
        }

        [TestMethod]
        public void Register_ThrowsArgumentException_WhenPasswordTooShort()
        {
            // B-11-02: het wachtwoord moet minimaal 6 tekens bevatten.
            try
            {
                _userService.Register("Naam", "kort@test.nl", "12345");
                Assert.Fail("Verwachtte een ArgumentException.");
            }
            catch (ArgumentException)
            {
                // Test geslaagd
            }
        }

        [TestMethod]
        public void Register_ThrowsArgumentException_WhenEmailAlreadyExists()
        {
            // B-11-01: het e-mailadres moet uniek zijn.
            _userService.Register("Eerste", "dubbel@test.nl", "geheim123");
            try
            {
                _userService.Register("Tweede", "dubbel@test.nl", "geheim123");
                Assert.Fail("Verwachtte een ArgumentException.");
            }
            catch (ArgumentException)
            {
                // Test geslaagd
            }
        }

        [TestMethod]
        public void Login_ReturnsUser_WhenCredentialsCorrect()
        {
            // Arrange
            _userService.Register("Inlogger", "login@test.nl", "geheim123");

            // Act
            var result = _userService.Login("login@test.nl", "geheim123");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("login@test.nl", result.Email);
        }

        [TestMethod]
        public void Login_ReturnsNull_WhenPasswordWrong()
        {
            // Arrange
            _userService.Register("Inlogger", "login@test.nl", "geheim123");

            // Act
            var result = _userService.Login("login@test.nl", "foutwachtwoord");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Login_ReturnsNull_WhenUserUnknown()
        {
            // Act
            var result = _userService.Login("bestaatniet@test.nl", "geheim123");

            // Assert
            Assert.IsNull(result);
        }
    }
}
