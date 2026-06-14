using System.Collections.Generic;
using System.Linq;
using Booksy.ViewModels;
using ServiceLibrary.Models;

namespace Booksy.Extensions
{
    public static class ModelExtensions
    {
        public static BookViewModel ToViewModel(this BookModel model)
        {
            var genreNames = model.Genres.Select(g => g.Name).ToList();
            return new BookViewModel
            {
                BookID = model.BookID,
                Name = model.Name,
                Author = model.AuthorName,
                GenreNames = genreNames,
                Genres = string.Join(", ", genreNames),
                ReadingStatus = model.ReadingStatus.ToText(),
                CoverImage = model.CoverImage
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
