using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Services;
using UnitTestProject1.Fakes;
using System;

namespace UnitTestProject1.Services
{
    [TestClass]
    public class ReviewServiceTests
    {
        private ReviewService _reviewService = null!;
        private FakeReviewRepository _fakeRepo = null!;

        [TestInitialize]
        public void Setup()
        {
            // Arrange (algemeen voor alle tests)
            // FakeReviewRepository start met 2 reviews voor BookID 1
            _fakeRepo = new FakeReviewRepository();
            _reviewService = new ReviewService(_fakeRepo);
        }

        [TestMethod]
        public void GetReviewsByBookId_ReturnsCorrectReviews()
        {
            // Act
            var result = _reviewService.GetReviewsByBookId(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetReviewsByBookId_ReturnsEmpty_WhenBookHasNoReviews()
        {
            // Act
            var result = _reviewService.GetReviewsByBookId(99);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void AddReview_AddsReview_WhenValid()
        {
            // Act
            _reviewService.AddReview(1, "Heel goed boek", 4);

            // Assert
            var result = _reviewService.GetReviewsByBookId(1);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void Reviews_HaveADate()
        {
            // Elke review heeft een datum waarop hij geplaatst is.
            var reviews = _reviewService.GetReviewsByBookId(1);
            foreach (var review in reviews)
            {
                Assert.AreNotEqual(default(System.DateTime), review.Date);
            }
        }

        [TestMethod]
        public void AddReview_SetsDate()
        {
            // Een nieuw toegevoegde review krijgt een datum.
            _reviewService.AddReview(1, "Heel goed boek", 4);
            var added = _reviewService.GetReviewsByBookId(1)[2];
            Assert.AreNotEqual(default(System.DateTime), added.Date);
        }

        [TestMethod]
        public void AddReview_ThrowsArgumentException_WhenTekstIsEmpty()
        {
            // Act
            try
            {
                _reviewService.AddReview(1, "", 4);
                Assert.Fail("Verwachtte een ArgumentException.");
            }
            catch (ArgumentException)
            {
                // Test geslaagd
            }
        }

        [TestMethod]
        public void AddReview_ThrowsArgumentException_WhenTekstIsWhitespace()
        {
            // Act
            try
            {
                _reviewService.AddReview(1, "   ", 4);
                Assert.Fail("Verwachtte een ArgumentException.");
            }
            catch (ArgumentException)
            {
                // Test geslaagd
            }
        }

        [TestMethod]
        public void AddReview_ThrowsArgumentException_WhenRatingTooLow()
        {
            // Act
            try
            {
                _reviewService.AddReview(1, "Goed boek", 0);
                Assert.Fail("Verwachtte een ArgumentException.");
            }
            catch (ArgumentException)
            {
                // Test geslaagd
            }
        }

        [TestMethod]
        public void AddReview_ThrowsArgumentException_WhenRatingTooHigh()
        {
            // Act
            try
            {
                _reviewService.AddReview(1, "Goed boek", 6);
                Assert.Fail("Verwachtte een ArgumentException.");
            }
            catch (ArgumentException)
            {
                // Test geslaagd
            }
        }

        [TestMethod]
        public void DeleteReview_RemovesReview_WhenExists()
        {
            // Act
            _reviewService.DeleteReview(1);

            // Assert
            var result = _reviewService.GetReviewsByBookId(1);
            Assert.AreEqual(1, result.Count);
        }
    }
}
