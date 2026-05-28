using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Models;

namespace UnitTestProject1.Models
{
    [TestClass]
    public class BookModelTests
    {
        [TestMethod]
        public void BookModel_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var book = new BookModel(1, "Test Titel", "Test Auteur", "Fantasy");

            // Assert
            Assert.AreEqual(1, book.BookID);
            Assert.AreEqual("Test Titel", book.Name);
            Assert.AreEqual("Test Auteur", book.Author);
            Assert.AreEqual("Fantasy", book.Genre);
        }
    }
}
