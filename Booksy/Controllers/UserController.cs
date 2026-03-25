using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using ServiceLibrary.Models;
using Booksy.ViewModels;

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
            return View(new UserViewModel
            {
                UserID = u.UserID,
                Name = u.Name,
                Email = u.Email
            });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserViewModel vm)
        {
            _svc.AddUser(new UserModel
            {
                Name = vm.Name,
                Email = vm.Email
            });
            return RedirectToAction("Index", "Book");
        }
    }
}