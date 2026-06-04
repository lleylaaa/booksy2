using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Booksy.Models;

namespace Booksy.Controllers
{
    // Vangt onverwachte fouten op. Program.cs stuurt fouten naar /Home/Error
    // wanneer de app niet in Development draait.
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View("Error", new ErrorViewModel { RequestId = requestId });
        }
    }
}
