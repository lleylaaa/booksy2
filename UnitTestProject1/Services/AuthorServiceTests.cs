using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Services;
using UnitTestProject1.Fakes;
using System;

namespace UnitTestProject1.Services
{
    [TestClass]
    public class AuthorServiceTests
    {
        private AuthorService _service = null!;
        private FakeAuthorRepository _repo = null!;

        [TestInitialize]
        public void Setup()
        {
            _repo = new FakeAuthorRepository();
            _service = new AuthorService(_repo);
        }

        [TestMethod]
        public void GetAllAuthors_ReturnsSeededAuthors()
        {
            var result = _service.GetAllAuthors();
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void AddAuthor_AddsAuthor_WhenValid()
        {
            var id = _service.AddAuthor("Tolkien");
            var author = _service.GetAuthorById(id);
            Assert.IsNotNull(author);
            Assert.AreEqual("Tolkien", author.Name);
        }

        [TestMethod]
        public void AddAuthor_ThrowsArgumentException_WhenNameTooShort()
        {
            // B-14-01: een auteur moet een naam hebben van minimaal 2 tekens.
            try
            {
                _service.AddAuthor("X");
                Assert.Fail("Verwachtte een ArgumentException.");
            }
            catch (ArgumentException)
            {
                // Test geslaagd
            }
        }

        [TestMethod]
        public void GetOrCreateAuthor_ReusesExistingAuthor_CaseInsensitive()
        {
            var id = _service.GetOrCreateAuthor("auteur 1");
            Assert.AreEqual(1, id); // bestaat al als "Auteur 1"
        }

        [TestMethod]
        public void GetOrCreateAuthor_CreatesNewAuthor_WhenUnknown()
        {
            var id = _service.GetOrCreateAuthor("Onbekende Auteur");
            Assert.IsTrue(id > 2);
        }
    }
}
