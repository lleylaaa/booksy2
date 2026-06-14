using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using Booksy.ViewModels;
using Booksy.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Booksy.Controllers
{
    // K-12-01: de boekpagina's zijn beveiligd. Niet ingelogd? Dan stuurt de
    // cookie-authenticatie door naar de loginpagina.
    [Authorize]
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
            _bookService.AddBook(vm.Name ?? "", vm.Author ?? "", SplitGenres(vm.Genres));
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var b = _bookService.GetBookById(id);
            if (b == null) return RedirectToAction("Index");

            return View(b.ToViewModel());
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            _bookService.UpdateBook(vm.BookID, vm.Name ?? "", vm.Author ?? "", SplitGenres(vm.Genres));
            // K-04-01: Na het opslaan wordt de gebruiker teruggestuurd naar de detailpagina.
            return RedirectToAction("Details", new { id = vm.BookID });
        }

        public IActionResult Delete(int id)
        {
            var b = _bookService.GetBookById(id);
            if (b == null) return RedirectToAction("Index");

            return View(b.ToViewModel());
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int BookID)
        {
            _bookService.DeleteBook(BookID);
            // K-05-01: Na het verwijderen wordt de gebruiker teruggestuurd naar het boekenoverzicht.
            return RedirectToAction("Index");
        }

        // FR-15: genres komen als komma-gescheiden tekst binnen uit het formulier.
        private static List<string> SplitGenres(string? genres)
        {
            if (string.IsNullOrWhiteSpace(genres))
                return new List<string>();
            return genres
                .Split(',')
                .Select(g => g.Trim())
                .Where(g => g.Length > 0)
                .ToList();
        }
    }
}
