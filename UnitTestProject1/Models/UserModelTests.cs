using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Models;

namespace UnitTestProject1.Models
{
    [TestClass]
    public class UserModelTests
    {
        [TestMethod]
        public void UserModel_SetProperties_WorksCorrectly()
        {
            // Arrange
            var user = new UserModel();

            // Act
            user.UserID = 1;
            user.Name = "Test Gebruiker";
            user.Email = "test@test.nl";

            // Assert
            Assert.AreEqual(1, user.UserID);
            Assert.AreEqual("Test Gebruiker", user.Name);
            Assert.AreEqual("test@test.nl", user.Email);
        }
    }
}
