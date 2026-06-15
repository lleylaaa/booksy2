using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Services;
using ServiceLibrary.Models;
using UnitTestProject1.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject1.Services
{
    [TestClass]
    public class BookServiceTests
    {
        private BookService _bookService = null!;
        private FakeBookRepository _fakeRepo = null!;
        private FakeAuthorRepository _authorRepo = null!;
        private FakeGenreRepository _genreRepo = null!;

        [TestInitialize]
        public void Setup()
        {
            // Arrange (algemeen voor alle tests)
            _authorRepo = new FakeAuthorRepository();
            _genreRepo = new FakeGenreRepository();
            _fakeRepo = new FakeBookRepository(_authorRepo, _genreRepo);
            var authorService = new AuthorService(_authorRepo);
            var genreService = new GenreService(_genreRepo);
            _bookService = new BookService(_fakeRepo, authorService, genreService);
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
            Assert.AreEqual("Auteur 1", result.AuthorName);
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
            _bookService.AddBook("Nieuw Boek", "Auteur X", new List<string> { "Fantasy" });

            // Assert
            var books = _bookService.GetAllBooks();
            Assert.AreEqual(3, books.Count);
            Assert.AreEqual("Nieuw Boek", books.Last().Name);
            Assert.AreEqual("Auteur X", books.Last().AuthorName);
        }

        [TestMethod]
        public void AddBook_LinksMultipleGenres()
        {
            // B-15-02: een boek kan aan meerdere genres gekoppeld worden.
            // Act
            _bookService.AddBook("Nieuw Boek", "Auteur X", new List<string> { "Roman", "Thriller" });

            // Assert
            var book = _bookService.GetAllBooks().Last();
            Assert.AreEqual(2, book.Genres.Count);
        }

        [TestMethod]
        public void AddBook_ReusesExistingAuthor()
        {
            // Een bestaande auteur mag niet dubbel worden aangemaakt.
            // Act
            _bookService.AddBook("Nieuw Boek", "Auteur 1", new List<string>());

            // Assert
            var book = _bookService.GetAllBooks().Last();
            Assert.AreEqual(1, book.AuthorID); // hergebruikt id 1 uit de fake
        }

        [TestMethod]
        public void AddBook_ThrowsArgumentException_WhenNameIsNull()
        {
            // Act
            try
            {
                _bookService.AddBook("", "Auteur X", new List<string> { "Fantasy" });
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
            // B-14-02: een boek moet altijd gekoppeld zijn aan een auteur.
            // Act
            try
            {
                _bookService.AddBook("Nieuw Boek", "", new List<string> { "Fantasy" });
                Assert.Fail("Verwachtte een ArgumentException.");
            }
            catch (ArgumentException)
            {
                // Test geslaagd
            }
        }

        [TestMethod]
        public void AddBook_StoresReadingStatus()
        {
            // FR-10: leesstatus wordt bewaard bij het boek.
            _bookService.AddBook("Nieuw Boek", "Auteur X", new List<string>(), "Gelezen", null);
            var book = _bookService.GetAllBooks().Last();
            Assert.AreEqual(ServiceLibrary.Models.ReadingStatus.Gelezen, book.ReadingStatus);
        }

        [TestMethod]
        public void AddBook_FallsBackToWilIkLezen_WhenStatusInvalid()
        {
            // B-10-02: een boek moet altijd een geldige status hebben.
            _bookService.AddBook("Nieuw Boek", "Auteur X", new List<string>(), "onzin", null);
            var book = _bookService.GetAllBooks().Last();
            Assert.AreEqual(ServiceLibrary.Models.ReadingStatus.WilIkLezen, book.ReadingStatus);
        }

        [TestMethod]
        public void AddBook_StoresCoverImageReference()
        {
            // FR-13: de verwijzing naar de omslag wordt bewaard.
            _bookService.AddBook("Nieuw Boek", "Auteur X", new List<string>(), "Bezig", "https://example.com/x.jpg");
            var book = _bookService.GetAllBooks().Last();
            Assert.AreEqual("https://example.com/x.jpg", book.CoverImage);
        }

        [TestMethod]
        public void AddBook_LeavesCoverNull_WhenNotProvided()
        {
            // B-13-01: zonder omslag blijft de verwijzing leeg (UI toont dan een standaard).
            _bookService.AddBook("Nieuw Boek", "Auteur X", new List<string>(), "Bezig", null);
            var book = _bookService.GetAllBooks().Last();
            Assert.IsNull(book.CoverImage);
        }

        [TestMethod]
        public void UpdateBook_UpdatesCorrectly()
        {
            // Act
            _bookService.UpdateBook(1, "Aangepast Boek", "Auteur 1", new List<string> { "Fictie" });

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
