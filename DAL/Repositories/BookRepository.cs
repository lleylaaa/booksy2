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

        public List<BookDTO> GetAllBooks()
        {
            try
            {
                var list = new List<BookDTO>();
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand("SELECT BoekID, Naam, Auteur, Genre FROM Book", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new BookDTO(
                        (int)reader["BoekID"],
                        reader["Naam"].ToString() ?? "",
                        reader["Auteur"].ToString() ?? "",
                        reader["Genre"].ToString() ?? ""
                    ));
                }
                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het ophalen van boeken.", ex);
            }
        }

        public BookDTO? GetBookById(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand("SELECT BoekID, Naam, Auteur, Genre FROM Book WHERE BoekID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new BookDTO(
                        (int)reader["BoekID"],
                        reader["Naam"].ToString() ?? "",
                        reader["Auteur"].ToString() ?? "",
                        reader["Genre"].ToString() ?? ""
                    );
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het ophalen van het boek.", ex);
            }
        }

        public void AddBook(string name, string author, string genre)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Book (Naam, Auteur, Genre) VALUES (@Naam, @Auteur, @Genre)", conn);
                cmd.Parameters.AddWithValue("@Naam", name);
                cmd.Parameters.AddWithValue("@Auteur", author);
                cmd.Parameters.AddWithValue("@Genre", (object?)genre ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het toevoegen van het boek.", ex);
            }
        }

        public void UpdateBook(int id, string name, string author, string genre)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE Book SET Naam=@Naam, Auteur=@Auteur, Genre=@Genre WHERE BoekID=@id", conn);
                cmd.Parameters.AddWithValue("@Naam", name);
                cmd.Parameters.AddWithValue("@Auteur", author);
                cmd.Parameters.AddWithValue("@Genre", (object?)genre ?? DBNull.Value);
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