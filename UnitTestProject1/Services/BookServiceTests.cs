using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Services;
using ServiceLibrary.Models;
using UnitTestProject1.Fakes;
using System;
using System.Linq;

namespace UnitTestProject1.Services
{
    [TestClass]
    public class BookServiceTests
    {
        private BookService _bookService = null!;
        private FakeBookRepository _fakeRepo = null!;

        [TestInitialize]
        public void Setup()
        {
            // Arrange (algemeen voor alle tests)
            _fakeRepo = new FakeBookRepository();
            _bookService = new BookService(_fakeRepo);
        }

        [TestMethod]
        public void GetAllBooks_ReturnsAllBooks()
        {
            // Act
            var result = _bookService.GetAllBooks();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetBookById_ReturnsCorrectBook()
        {
            // Arrange
            int expectedId = 1;
            string expectedName = "Test Boek 1";

            // Act
            var result = _bookService.GetBookById(expectedId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedName, result.Name);
            Assert.AreEqual(expectedId, result.BookID);
        }

        [TestMethod]
        public void GetBookById_ReturnsNull_WhenNotFound()
        {
            // Arrange
            int nonExistingId = 99;

            // Act
            var result = _bookService.GetBookById(nonExistingId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddBook_DoesNotThrow_WhenValid()
        {
            // Act
            _bookService.AddBook("Nieuw Boek", "Auteur X", "Fantasy");

            // Assert
            var books = _bookService.GetAllBooks();
            Assert.AreEqual(3, books.Count);
            Assert.AreEqual("Nieuw Boek", books.Last().Name);
        }

        [TestMethod]
        public void AddBook_ThrowsArgumentException_WhenNameIsNull()
        {
            // Act
            try
            {
                _bookService.AddBook("", "Auteur X", "Fantasy");
                Assert.Fail("Verwachtte een ArgumentException.");
            }
            catch (ArgumentException)
            {
                // Test geslaagd
            }
        }

        [TestMethod]
        public void AddBook_ThrowsArgumentException_WhenAuthorIsNull()
        {
            // Act
            try
            {
                _bookService.AddBook("Nieuw Boek", "", "Fantasy");
                Assert.Fail("Verwachtte een ArgumentException.");
            }
            catch (ArgumentException)
            {
                // Test geslaagd
            }
        }

        [TestMethod]
        public void UpdateBook_UpdatesCorrectly()
        {
            // Act
            _bookService.UpdateBook(1, "Aangepast Boek", "Auteur 1", "Fictie");

            // Assert
            var updatedBook = _bookService.GetBookById(1);
            Assert.AreEqual("Aangepast Boek", updatedBook?.Name);
        }

        [TestMethod]
        public void DeleteBook_RemovesBookCorrectly()
        {
            // Arrange
            int idToDelete = 1;

            // Act
            _bookService.DeleteBook(idToDelete);

            // Assert
            var result = _bookService.GetBookById(idToDelete);
            Assert.IsNull(result);
        }
    }
}
