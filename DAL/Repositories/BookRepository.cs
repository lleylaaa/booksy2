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
                var cmd = new SqlCommand(
                    @"SELECT b.BoekID, b.Naam, b.AuteurID, a.Naam AS AuteurNaam
                      FROM Book b
                      JOIN Author a ON a.AuteurID = b.AuteurID", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new BookDTO(
                        (int)reader["BoekID"],
                        reader["Naam"].ToString() ?? "",
                        (int)reader["AuteurID"],
                        reader["AuteurNaam"].ToString() ?? ""
                    ));
                }
                reader.Close();
                foreach (var book in list)
                    book.Genres.AddRange(GetGenresForBook(conn, book.BookID));
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
                var cmd = new SqlCommand(
                    @"SELECT b.BoekID, b.Naam, b.AuteurID, a.Naam AS AuteurNaam
                      FROM Book b
                      JOIN Author a ON a.AuteurID = b.AuteurID
                      WHERE b.BoekID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                BookDTO? book = null;
                if (reader.Read())
                {
                    book = new BookDTO(
                        (int)reader["BoekID"],
                        reader["Naam"].ToString() ?? "",
                        (int)reader["AuteurID"],
                        reader["AuteurNaam"].ToString() ?? ""
                    );
                }
                reader.Close();
                if (book != null)
                    book.Genres.AddRange(GetGenresForBook(conn, book.BookID));
                return book;
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het ophalen van het boek.", ex);
            }
        }

        public void AddBook(string name, int authorId, List<int> genreIds)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Book (Naam, AuteurID) OUTPUT INSERTED.BoekID VALUES (@Naam, @AuteurID)", conn);
                cmd.Parameters.AddWithValue("@Naam", name);
                cmd.Parameters.AddWithValue("@AuteurID", authorId);
                var bookId = (int)cmd.ExecuteScalar();
                LinkGenres(conn, bookId, genreIds);
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het toevoegen van het boek.", ex);
            }
        }

        public void UpdateBook(int id, string name, int authorId, List<int> genreIds)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE Book SET Naam=@Naam, AuteurID=@AuteurID WHERE BoekID=@id", conn);
                cmd.Parameters.AddWithValue("@Naam", name);
                cmd.Parameters.AddWithValue("@AuteurID", authorId);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                var clear = new SqlCommand("DELETE FROM BookGenre WHERE BoekID=@id", conn);
                clear.Parameters.AddWithValue("@id", id);
                clear.ExecuteNonQuery();
                LinkGenres(conn, id, genreIds);
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
                var clear = new SqlCommand("DELETE FROM BookGenre WHERE BoekID = @id", conn);
                clear.Parameters.AddWithValue("@id", id);
                clear.ExecuteNonQuery();
                var cmd = new SqlCommand("DELETE FROM Book WHERE BoekID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Fout bij het verwijderen van het boek.", ex);
            }
        }

        private static List<GenreDTO> GetGenresForBook(SqlConnection conn, int bookId)
        {
            var genres = new List<GenreDTO>();
            var cmd = new SqlCommand(
                @"SELECT g.GenreID, g.Naam
                  FROM Genre g
                  JOIN BookGenre bg ON bg.GenreID = g.GenreID
                  WHERE bg.BoekID = @bookId", conn);
            cmd.Parameters.AddWithValue("@bookId", bookId);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                genres.Add(new GenreDTO(
                    (int)reader["GenreID"],
                    reader["Naam"].ToString() ?? ""
                ));
            }
            reader.Close();
            return genres;
        }

        private static void LinkGenres(SqlConnection conn, int bookId, List<int> genreIds)
        {
            foreach (var genreId in genreIds)
            {
                var cmd = new SqlCommand(
                    "INSERT INTO BookGenre (BoekID, GenreID) VALUES (@BoekID, @GenreID)", conn);
                cmd.Parameters.AddWithValue("@BoekID", bookId);
                cmd.Parameters.AddWithValue("@GenreID", genreId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
