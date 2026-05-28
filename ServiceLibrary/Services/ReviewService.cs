using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using ServiceLibrary.Models;
using ServiceLibrary.Extensions;

namespace ServiceLibrary.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _repo;

        public ReviewService(IReviewRepository repo)
        {
            _repo = repo;
        }

        public List<ReviewModel> GetReviewsByBookId(int bookId)
        {
            return _repo.GetReviewsByBookId(bookId)
                .Select(dto => dto.ToModel())
                .ToList();
        }

        public void AddReview(int bookId, string tekst, int rating)
        {
            if (string.IsNullOrWhiteSpace(tekst))
                throw new ArgumentException("Tekst is verplicht.");
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating moet tussen 1 en 5 zijn.");
            _repo.AddReview(bookId, tekst, rating);
        }

        public void DeleteReview(int id)
        {
            _repo.DeleteReview(id);
        }
    }
}
