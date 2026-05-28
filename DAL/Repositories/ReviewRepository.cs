using Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly string _connectionString;

        public ReviewRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ReviewDTO> GetReviewsByBookId(int bookId)
        {
            try
            {
                var list = new List<ReviewDTO>();
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand(
                    "SELECT ReviewID, BoekID, Tekst, Rating, Datum FROM Review WHERE BoekID = @bookId", conn);
                cmd.Parameters.AddWithValue("@bookId", bookId);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new ReviewDTO(
                        (int)reader["ReviewID"],
                        (int)reader["BoekID"],
                        reader["Tekst"].ToString() ?? "",
                        (int)reader["Rating"],
                        (DateTime)reader["Datum"]
                    ));
                }
                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het ophalen van reviews.", ex);
            }
        }

        public void AddReview(int bookId, string tekst, int rating)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Review (BoekID, Tekst, Rating, Datum) VALUES (@BoekID, @Tekst, @Rating, @Datum)", conn);
                cmd.Parameters.AddWithValue("@BoekID", bookId);
                cmd.Parameters.AddWithValue("@Tekst", tekst);
                cmd.Parameters.AddWithValue("@Rating", rating);
                cmd.Parameters.AddWithValue("@Datum", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het toevoegen van de review.", ex);
            }
        }

        public void DeleteReview(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Review WHERE ReviewID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het verwijderen van de review.", ex);
            }
        }
    }
}
