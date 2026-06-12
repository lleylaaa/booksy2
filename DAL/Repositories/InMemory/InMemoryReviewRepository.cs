using Interface;

namespace DAL.Repositories.InMemory
{
    // In-memory variant van de reviews-repository. Zie InMemoryBookRepository
    // voor uitleg over de werking en het gebruik van de lock.
    public class InMemoryReviewRepository : IReviewRepository
    {
        private readonly object _lock = new();
        private readonly List<ReviewDTO> _reviews = new();
        private int _nextId = 1;

        public InMemoryReviewRepository()
        {
            // Een paar voorbeeldreviews bij het eerste boek (BookID 1).
            AddReview(1, "Prachtig geschreven, een echte aanrader.", 5);
            AddReview(1, "Mooi boek, maar wel aan de lange kant.", 4);
        }

        public List<ReviewDTO> GetReviewsByBookId(int bookId)
        {
            lock (_lock)
            {
                return _reviews.Where(r => r.BookID == bookId).ToList();
            }
        }

        public void AddReview(int bookId, string text, int rating)
        {
            lock (_lock)
            {
                _reviews.Add(new ReviewDTO(_nextId++, bookId, text, rating, DateTime.Now));
            }
        }

        public void DeleteReview(int id)
        {
            lock (_lock)
            {
                _reviews.RemoveAll(r => r.ReviewID == id);
            }
        }
    }
}
