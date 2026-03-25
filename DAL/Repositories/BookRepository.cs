using Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;

        public BookRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<(int BookID, string Name, string Author, string Genre, int? Rating)> GetAllBooks()
        {
            try
            {
                var list = new List<(int BookID, string Name, string Author, string Genre, int? Rating)>();
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand("SELECT BoekID, Naam, Auteur, Genre, Rating FROM Book", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add((
                        (int)reader["BoekID"],
                        reader["Naam"].ToString() ?? "",
                        reader["Auteur"].ToString() ?? "",
                        reader["Genre"].ToString() ?? "",
                        reader["Rating"] == DBNull.Value ? null : (int?)reader["Rating"]
                    ));
                }
                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het ophalen van boeken.", ex);
            }
        }

        public (int BookID, string Name, string Author, string Genre, int? Rating)? GetBookById(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand("SELECT BoekID, Naam, Auteur, Genre, Rating FROM Book WHERE BoekID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return (
                        (int)reader["BoekID"],
                        reader["Naam"].ToString() ?? "",
                        reader["Auteur"].ToString() ?? "",
                        reader["Genre"].ToString() ?? "",
                        reader["Rating"] == DBNull.Value ? null : (int?)reader["Rating"]
                    );
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het ophalen van het boek.", ex);
            }
        }

        public void AddBook(string name, string author, string genre, int? rating)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Book (Naam, Auteur, Genre, Rating) VALUES (@Naam, @Auteur, @Genre, @Rating)", conn);
                cmd.Parameters.AddWithValue("@Naam", name);
                cmd.Parameters.AddWithValue("@Auteur", author);
                cmd.Parameters.AddWithValue("@Genre", (object?)genre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Rating", (object?)rating ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het toevoegen van het boek.", ex);
            }
        }

        public void UpdateBook(int id, string name, string author, string genre, int? rating)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE Book SET Naam=@Naam, Auteur=@Auteur, Genre=@Genre, Rating=@Rating WHERE BoekID=@id", conn);
                cmd.Parameters.AddWithValue("@Naam", name);
                cmd.Parameters.AddWithValue("@Auteur", author);
                cmd.Parameters.AddWithValue("@Genre", (object?)genre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Rating", (object?)rating ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het updaten van het boek.", ex);
            }
        }

        public void DeleteBook(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Book WHERE BoekID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het verwijderen van het boek.", ex);
            }
        }
    }
}