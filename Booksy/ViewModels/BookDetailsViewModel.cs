using System.Collections.Generic;

namespace Booksy.ViewModels
{
    public class BookDetailsViewModel
    {
        public BookViewModel Book { get; set; } = new();
        public List<ReviewViewModel> Reviews { get; set; } = new();
        public ReviewViewModel NewReview { get; set; } = new();
    }
}
