using System;

namespace ServiceLibrary.Models
{
    public class ReviewModel
    {
        public int ReviewID { get; }
        public int BookID { get; }
        public string Tekst { get; }
        public int Rating { get; }
        public DateTime Datum { get; }

        public ReviewModel(int reviewID, int bookID, string tekst, int rating, DateTime datum)
        {
            ReviewID = reviewID;
            BookID = bookID;
            Tekst = tekst;
            Rating = rating;
            Datum = datum;
        }
    }
}
