using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using Booksy.ViewModels;
using Booksy.Extensions;

namespace Booksy.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _svc;

        public UserController(UserService svc)
        {
            _svc = svc;
        }

        public IActionResult Details(int id)
        {
            var u = _svc.GetUserById(id);
            if (u == null) return RedirectToAction("Index", "Book");
            return View(u.ToViewModel());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserViewModel vm)
        {
            _svc.AddUser(vm.Name ?? "", vm.Email ?? "");
            return RedirectToAction("Index", "Book");
        }
    }
}