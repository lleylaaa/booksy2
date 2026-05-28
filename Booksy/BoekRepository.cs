using Booksy .Models;
using Microsoft.Data.SqlClient;
namespace Booksy.Data
{
    public class BoekRepository
    {
        private readonly string _connectionString;

        public BoekRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 🔥 Boek toevoegen
        public void VoegBoekToe(Boek boek)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = "INSERT INTO Boek (GebruikerID, Naam, Auteur, Genre) VALUES (1, @Naam, @Auteur, @Genre)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Naam", boek.Naam);
                cmd.Parameters.AddWithValue("@Auteur", boek.Auteur);
                cmd.Parameters.AddWithValue("@Genre", boek.Genre);

                cmd.ExecuteNonQuery();
            }
        }

        // 📚 Alle boeken ophalen
        public List<Boek> GetBoeken()
        {
            List<Boek> boeken = new List<Boek>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
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

            return boeken;
        }

        // 🔍 1 boek ophalen
        public Boek? GetBoekById(int id)
        {
            Boek? boek = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
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

            return boek;
        }
    }
}