using Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly string _connectionString;

        public AuthorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<AuthorDTO> GetAllAuthors()
        {
            try
            {
                var list = new List<AuthorDTO>();
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand("SELECT AuteurID, Naam FROM Author", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new AuthorDTO(
                        (int)reader["AuteurID"],
                        reader["Naam"].ToString() ?? ""
                    ));
                }
                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het ophalen van auteurs.", ex);
            }
        }

        public AuthorDTO? GetAuthorById(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand("SELECT AuteurID, Naam FROM Author WHERE AuteurID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new AuthorDTO(
                        (int)reader["AuteurID"],
                        reader["Naam"].ToString() ?? ""
                    );
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het ophalen van de auteur.", ex);
            }
        }

        public int AddAuthor(string name)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Author (Naam) OUTPUT INSERTED.AuteurID VALUES (@Naam)", conn);
                cmd.Parameters.AddWithValue("@Naam", name);
                return (int)cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het toevoegen van de auteur.", ex);
            }
        }
    }
}
