using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Models;

namespace UnitTestProject1.Models
{
    [TestClass]
    public class BookModelTests
    {
        [TestMethod]
        public void BookModel_SetProperties_WorksCorrectly()
        {
            // Arrange
            var book = new BookModel();

            // Act
            book.BookID = 1;
            book.Name = "Test Titel";
            book.Author = "Test Auteur";
            book.Genre = "Fantasy";
            book.Rating = 4;

            // Assert
            Assert.AreEqual(1, book.BookID);
            Assert.AreEqual("Test Titel", book.Name);
            Assert.AreEqual("Test Auteur", book.Author);
            Assert.AreEqual("Fantasy", book.Genre);
            Assert.AreEqual(4, book.Rating);
        }
    }
}
