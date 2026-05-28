using System;
using System.Collections.Generic;
using System.Linq;
using Interface;

namespace UnitTestProject1.Fakes
{
    public class FakeReviewRepository : IReviewRepository
    {
        private List<ReviewDTO> _reviews = new();
        private int _nextId = 1;

        public FakeReviewRepository()
        {
            _reviews.Add(new ReviewDTO(1, 1, "Geweldig boek!", 5, new DateTime(2026, 5, 1)));
            _reviews.Add(new ReviewDTO(2, 1, "Best aardig", 3, new DateTime(2026, 5, 2)));
            _nextId = 3;
        }

        public List<ReviewDTO> GetReviewsByBookId(int bookId)
        {
            return _reviews.Where(r => r.BookID == bookId).ToList();
        }

        public void AddReview(int bookId, string tekst, int rating)
        {
            _reviews.Add(new ReviewDTO(_nextId++, bookId, tekst, rating, DateTime.Now));
        }

        public void DeleteReview(int id)
        {
            var index = _reviews.FindIndex(r => r.ReviewID == id);
            if (index >= 0)
            {
                _reviews.RemoveAt(index);
            }
        }
    }
}
