using Interface;
using ServiceLibrary.Models;


namespace ServiceLibrary.Extensions
{
    public static class DtoExtensions
    {
        public static BookModel ToModel(this BookDTO dto)
        {
            return new BookModel(dto.BookID, dto.Name, dto.Author, dto.Genre);
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
