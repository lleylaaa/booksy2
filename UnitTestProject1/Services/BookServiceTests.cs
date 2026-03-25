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
            // Arrange
            var newBook = new BookModel { Name = "Nieuw Boek", Author = "Auteur X" };

            // Act
            _bookService.AddBook(newBook);

            // Assert
            var books = _bookService.GetAllBooks();
            Assert.AreEqual(3, books.Count);
            Assert.AreEqual("Nieuw Boek", books.Last().Name);
        }

        [TestMethod]
        public void AddBook_ThrowsArgumentException_WhenNameIsNull()
        {
            // Arrange
            var newBook = new BookModel { Name = "", Author = "Auteur X" };

            // Act
            try
            {
                _bookService.AddBook(newBook);
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
            // Arrange
            var newBook = new BookModel { Name = "Nieuw Boek", Author = "" };

            // Act
            try
            {
                _bookService.AddBook(newBook);
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
            // Arrange
            var bookToUpdate = new BookModel { BookID = 1, Name = "Aangepast Boek", Author = "Auteur 1", Genre = "Fictie", Rating = 4 };

            // Act
            _bookService.UpdateBook(bookToUpdate);

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

        [TestMethod]
        public void SetRating_UpdatesRating_WhenValid()
        {
            // Arrange
            int bookId = 1;
            int newRating = 5;

            // Act
            _bookService.SetRating(bookId, newRating);

            // Assert
            var updatedBook = _bookService.GetBookById(bookId);
            Assert.AreEqual(5, updatedBook?.Rating);
        }

        [TestMethod]
        public void SetRating_ThrowsArgumentException_WhenRatingIsTooHigh()
        {
            // Arrange
            int bookId = 1;
            int invalidRating = 6; // Mag maximaal 5 zijn

            // Act
            try
            {
                _bookService.SetRating(bookId, invalidRating);
                Assert.Fail("Verwachtte een ArgumentException.");
            }
            catch (ArgumentException)
            {
                // Test geslaagd
            }
        }
    }
}
