using System;

namespace Interface
{
    public class ReviewDTO
    {
        public int ReviewID { get; }
        public int BookID { get; }
        public string Tekst { get; }
        public int Rating { get; }
        public DateTime Datum { get; }

        public ReviewDTO(int reviewID, int bookID, string tekst, int rating, DateTime datum)
        {
            ReviewID = reviewID;
            BookID = bookID;
            Tekst = tekst;
            Rating = rating;
            Datum = datum;
        }
    }
}
