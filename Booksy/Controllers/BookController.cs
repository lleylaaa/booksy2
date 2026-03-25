using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using ServiceLibrary.Models;
using Booksy.ViewModels;

namespace Booksy.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _svc;

        public BookController(IBookService svc)
        {
            _svc = svc;
        }

        public IActionResult Index()
        {
            var books = _svc.GetAllBooks()
                .Select(b => new BookViewModel
                {
                    BookID = b.BookID,
                    Name = b.Name,
                    Author = b.Author,
                    Genre = b.Genre,
                    Rating = b.Rating
                }).ToList();
            return View(books);
        }

        public IActionResult Details(int id)
        {
            var b = _svc.GetBookById(id);
            if (b == null) return RedirectToAction("Index");

            return View(new BookViewModel
            {
                BookID = b.BookID,
                Name = b.Name,
                Author = b.Author,
                Genre = b.Genre,
                Rating = b.Rating
            });
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

            _svc.AddBook(new BookModel
            {
                Name = vm.Name,
                Author = vm.Author,
                Genre = vm.Genre,
                Rating = vm.Rating
            });
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var b = _svc.GetBookById(id);
            if (b == null) return RedirectToAction("Index");

            // FR-04/B-04-02: Alleen de eigenaar mag het bewerken.
            // Placeholder: Check logic hier als er een login systeem is.

            return View(new BookViewModel
            {
                BookID = b.BookID,
                Name = b.Name,
                Author = b.Author,
                Genre = b.Genre,
                Rating = b.Rating
            });
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            _svc.UpdateBook(new BookModel
            {
                BookID = vm.BookID,
                Name = vm.Name,
                Author = vm.Author,
                Genre = vm.Genre,
                Rating = vm.Rating
            });
            // K-04-01: Na het opslaan wordt de gebruiker teruggestuurd naar de detailpagina.
            return RedirectToAction("Details", new { id = vm.BookID });
        }

        public IActionResult Delete(int id)
        {
            var b = _svc.GetBookById(id);
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
            _svc.DeleteBook(BookID);
            // K-05-01: Na het verwijderen wordt de gebruiker teruggestuurd naar het boekenoverzicht.
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SetRating(int bookID, int rating)
        {
            _svc.SetRating(bookID, rating);
            return RedirectToAction("Details", new { id = bookID });
        }
    }
}