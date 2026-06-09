using Booksy.ViewModels;
using ServiceLibrary.Models;

namespace Booksy.Extensions
{
    public static class ModelExtensions
    {
        public static BookViewModel ToViewModel(this BookModel model)
        {
            return new BookViewModel
            {
                BookID = model.BookID,
                Name = model.Name,
                Author = model.Author,
                Genre = model.Genre
            };
        }

        public static ReviewViewModel ToViewModel(this ReviewModel model)
        {
            return new ReviewViewModel
            {
                ReviewID = model.ReviewID,
                BookID = model.BookID,
                Text = model.Text,
                Rating = model.Rating,
                Date = model.Date
            };
        }
    }
}
