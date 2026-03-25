using Booksy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

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

        public IActionResult BoekToevoegen()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BoekToevoegen(Boek boek)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Boek (GebruikerID, Naam, Auteur, Genre) VALUES (1, @Naam, @Auteur, @Genre)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Naam", boek.Naam);
                cmd.Parameters.AddWithValue("@Auteur", boek.Auteur);
                cmd.Parameters.AddWithValue("@Genre", boek.Genre);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Boekenoverzicht()
        {
            string connectionString = _configuration.GetConnectionString("MSSQL connection string");
            List<Boek> boeken = new List<Boek>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT BoekID, Naam, Auteur, Genre FROM Boek";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    boeken.Add(new Boek
                    {
                        BoekID = (int)reader["BoekID"],
                        Naam = reader["Naam"].ToString(),
                        Auteur = reader["Auteur"].ToString(),
                        Genre = reader["Genre"].ToString()
                    });
                }
            }

            return View(boeken);
        }
        public IActionResult BoekDetails(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
            Boek boek = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT BoekID, Naam, Auteur, Genre FROM Boek WHERE BoekID = @BoekID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BoekID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    boek = new Boek
                    {
                        BoekID = (int)reader["BoekID"],
                        Naam = reader["Naam"].ToString(),
                        Auteur = reader["Auteur"].ToString(),
                        Genre = reader["Genre"].ToString()
                    };
                }
            }

            if (boek == null)
            {
                return RedirectToAction("Boekenoverzicht");
            }

            return View(boek);
        }
    }
}