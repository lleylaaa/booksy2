using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using Booksy.ViewModels;

namespace Booksy.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        // FR-11: registreren
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var user = _userService.Register(vm.Name ?? "", vm.Email ?? "", vm.Password ?? "");
                await SignInAsync(user.UserID, user.Name, user.Email);
                // K-11-01: na een succesvolle registratie/login naar de persoonlijke pagina.
                return RedirectToAction("Index", "Book");
            }
            catch (System.ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // FR-11: inloggen
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = _userService.Login(vm.Email ?? "", vm.Password ?? "");
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "E-mailadres of wachtwoord is onjuist.");
                return View(vm);
            }

            await SignInAsync(user.UserID, user.Name, user.Email);
            // K-11-01: na een succesvolle login naar de persoonlijke pagina.
            return RedirectToAction("Index", "Book");
        }

        // FR-12: uitloggen
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // K-12-01: na het uitloggen terug naar de startpagina.
            return RedirectToAction("Login");
        }

        private async Task SignInAsync(int userId, string name, string email)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, name),
                new(ClaimTypes.Email, email)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }
    }
}
