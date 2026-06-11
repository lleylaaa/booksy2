using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using Booksy.ViewModels;

namespace Booksy.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // FR-07: Review schrijven bij een boek
        [HttpPost]
        public IActionResult Create(ReviewViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Book", new { id = vm.BookID });
            }
            _reviewService.AddReview(vm.BookID, vm.Text ?? "", vm.Rating);
            return RedirectToAction("Details", "Book", new { id = vm.BookID });
        }

        // FR-09: Review verwijderen - bevestigingspagina
        public IActionResult Delete(int id, int bookId)
        {
            var vm = new ReviewViewModel { ReviewID = id, BookID = bookId };
            return View(vm);
        }

        // FR-09: Review verwijderen - verwijder bevestigd
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id, int bookId)
        {
            _reviewService.DeleteReview(id);
            return RedirectToAction("Details", "Book", new { id = bookId });
        }
    }
}
