using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Models;

namespace UnitTestProject1.Models
{
    [TestClass]
    public class UserModelTests
    {
        [TestMethod]
        public void UserModel_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var user = new UserModel(1, "Test Gebruiker", "test@test.nl");

            // Assert
            Assert.AreEqual(1, user.UserID);
            Assert.AreEqual("Test Gebruiker", user.Name);
            Assert.AreEqual("test@test.nl", user.Email);
        }
    }
}
