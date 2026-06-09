using System.Collections.Generic;

namespace Interface
{
    public interface IReviewRepository
    {
        List<ReviewDTO> GetReviewsByBookId(int bookId);
        void AddReview(int bookId, string text, int rating);
        void DeleteReview(int id);
    }
}
