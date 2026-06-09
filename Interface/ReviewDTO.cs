using System;

namespace Interface
{
    public class ReviewDTO
    {
        public int ReviewID { get; }
        public int BookID { get; }
        public string Text { get; }
        public int Rating { get; }
        public DateTime Date { get; }

        public ReviewDTO(int reviewID, int bookID, string text, int rating, DateTime date)
        {
            ReviewID = reviewID;
            BookID = bookID;
            Text = text;
            Rating = rating;
            Date = date;
        }
    }
}
