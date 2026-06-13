using Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly string _connectionString;

        public GenreRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<GenreDTO> GetAllGenres()
        {
            try
            {
                var list = new List<GenreDTO>();
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand("SELECT GenreID, Naam FROM Genre", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new GenreDTO(
                        (int)reader["GenreID"],
                        reader["Naam"].ToString() ?? ""
                    ));
                }
                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het ophalen van genres.", ex);
            }
        }

        public GenreDTO? GetGenreById(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand("SELECT GenreID, Naam FROM Genre WHERE GenreID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new GenreDTO(
                        (int)reader["GenreID"],
                        reader["Naam"].ToString() ?? ""
                    );
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het ophalen van het genre.", ex);
            }
        }

        public int AddGenre(string name)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Genre (Naam) OUTPUT INSERTED.GenreID VALUES (@Naam)", conn);
                cmd.Parameters.AddWithValue("@Naam", name);
                return (int)cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het toevoegen van het genre.", ex);
            }
        }
    }
}
