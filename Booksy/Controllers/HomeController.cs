using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Booksy.Models;
using Microsoft.Data.SqlClient;

namespace Booksy.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
} 