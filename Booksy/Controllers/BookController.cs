using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using Booksy.ViewModels;
using Booksy.Extensions;
using System.Linq;

namespace Booksy.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _bookService;
        private readonly ReviewService _reviewService;

        public BookController(BookService bookService, ReviewService reviewService)
        {
            _bookService = bookService;
            _reviewService = reviewService;
        }

        public IActionResult Index()
        {
            var books = _bookService.GetAllBooks()
                .Select(b => b.ToViewModel())
                .ToList();
            return View(books);
        }

        public IActionResult Details(int id)
        {
            var b = _bookService.GetBookById(id);
            if (b == null) return RedirectToAction("Index");

            var reviews = _reviewService.GetReviewsByBookId(id)
                .Select(r => r.ToViewModel())
                .ToList();

            var vm = new BookDetailsViewModel
            {
                Book = b.ToViewModel(),
                Reviews = reviews,
                NewReview = new ReviewViewModel { BookID = id }
            };
            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            _bookService.AddBook(vm.Name ?? "", vm.Author ?? "", vm.Genre ?? "");
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var b = _bookService.GetBookById(id);
            if (b == null) return RedirectToAction("Index");

            return View(new BookViewModel
            {
                BookID = b.BookID,
                Name = b.Name,
                Author = b.Author,
                Genre = b.Genre
            });
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            _bookService.UpdateBook(vm.BookID, vm.Name ?? "", vm.Author ?? "", vm.Genre ?? "");
            // K-04-01: Na het opslaan wordt de gebruiker teruggestuurd naar de detailpagina.
            return RedirectToAction("Details", new { id = vm.BookID });
        }

        public IActionResult Delete(int id)
        {
            var b = _bookService.GetBookById(id);
            if (b == null) return RedirectToAction("Index");

            return View(new BookViewModel
            {
                BookID = b.BookID,
                Name = b.Name,
                Author = b.Author,
                Genre = b.Genre
            });
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int BookID)
        {
            _bookService.DeleteBook(BookID);
            // K-05-01: Na het verwijderen wordt de gebruiker teruggestuurd naar het boekenoverzicht.
            return RedirectToAction("Index");
        }
    }
}