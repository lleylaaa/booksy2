using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
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
            var genres = new List<GenreModel> { new GenreModel(1, "Fantasy") };
            var book = new BookModel(1, "Test Titel", 5, "Test Auteur", genres);

            // Assert
            Assert.AreEqual(1, book.BookID);
            Assert.AreEqual("Test Titel", book.Name);
            Assert.AreEqual(5, book.AuthorID);
            Assert.AreEqual("Test Auteur", book.AuthorName);
            Assert.AreEqual("Fantasy", book.Genres.Single().Name);
        }

        [TestMethod]
        public void BookModel_WithoutGenres_HasEmptyGenreList()
        {
            // Arrange & Act
            var book = new BookModel(1, "Test Titel", 5, "Test Auteur");

            // Assert
            Assert.AreEqual(0, book.Genres.Count);
        }

        [TestMethod]
        public void BookModel_CanHaveMultipleGenres()
        {
            // B-15-02: een boek kan aan meerdere genres gekoppeld worden.
            // Arrange & Act
            var genres = new List<GenreModel>
            {
                new GenreModel(1, "Roman"),
                new GenreModel(2, "Thriller")
            };
            var book = new BookModel(1, "Test Titel", 5, "Test Auteur", genres);

            // Assert
            Assert.AreEqual(2, book.Genres.Count);
        }
    }
}
