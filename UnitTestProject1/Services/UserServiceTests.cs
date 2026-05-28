using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Services;
using ServiceLibrary.Models;
using UnitTestProject1.Fakes;

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
        public void AddUser_AddsUserCorrectly()
        {
            // Act
            _userService.AddUser("Nieuwe Gebruiker", "nieuw@test.nl");

            // Assert
            var result = _userService.GetUserById(2); // Fake repo wijst ID 2 toe aan de eerste toegevoegde user
            Assert.IsNotNull(result);
            Assert.AreEqual("Nieuwe Gebruiker", result.Name);
        }
    }
}
