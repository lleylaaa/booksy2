using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Services;
using UnitTestProject1.Fakes;
using System;

namespace UnitTestProject1.Services
{
    [TestClass]
    public class GenreServiceTests
    {
        private GenreService _service = null!;
        private FakeGenreRepository _repo = null!;

        [TestInitialize]
        public void Setup()
        {
            _repo = new FakeGenreRepository();
            _service = new GenreService(_repo);
        }

        [TestMethod]
        public void GetAllGenres_ReturnsSeededGenres()
        {
            var result = _service.GetAllGenres();
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void AddGenre_AddsGenre_WhenValid()
        {
            var id = _service.AddGenre("Fantasy");
            var genre = _service.GetGenreById(id);
            Assert.IsNotNull(genre);
            Assert.AreEqual("Fantasy", genre.Name);
        }

        [TestMethod]
        public void AddGenre_ThrowsArgumentException_WhenNameTooShort()
        {
            // B-15-01: een genre moet een naam hebben van minimaal 2 tekens.
            try
            {
                _service.AddGenre("a");
                Assert.Fail("Verwachtte een ArgumentException.");
            }
            catch (ArgumentException)
            {
                // Test geslaagd
            }
        }

        [TestMethod]
        public void GetOrCreateGenre_ReusesExistingGenre_CaseInsensitive()
        {
            var id = _service.GetOrCreateGenre("fictie");
            Assert.AreEqual(1, id); // bestaat al als "Fictie"
        }
    }
}
