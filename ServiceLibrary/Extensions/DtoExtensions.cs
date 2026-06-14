using System.Linq;
using Interface;
using ServiceLibrary.Models;

namespace ServiceLibrary.Extensions
{
    public static class DtoExtensions
    {
        public static BookModel ToModel(this BookDTO dto)
        {
            return new BookModel(
                dto.BookID,
                dto.Name,
                dto.AuthorID,
                dto.AuthorName,
                dto.Genres.Select(g => g.ToModel()).ToList(),
                ReadingStatusExtensions.FromText(dto.ReadingStatus),
                dto.CoverImage);
        }

        public static AuthorModel ToModel(this AuthorDTO dto)
        {
            return new AuthorModel(dto.AuthorID, dto.Name);
        }

        public static GenreModel ToModel(this GenreDTO dto)
        {
            return new GenreModel(dto.GenreID, dto.Name);
        }

        public static ReviewModel ToModel(this ReviewDTO dto)
        {
            return new ReviewModel(dto.ReviewID, dto.BookID, dto.Text, dto.Rating, dto.Date);
        }

        public static UserModel ToModel(this UserDTO dto)
        {
            return new UserModel(dto.UserID, dto.Name, dto.Email);
        }
    }
}
