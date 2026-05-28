using Interface;
using Microsoft.Data.SqlClient;
using System;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public UserDTO? GetUserById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand("SELECT GebruikerID, Naam, Email FROM [User] WHERE GebruikerID = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new UserDTO(
                    (int)reader["GebruikerID"],
                    reader["Naam"].ToString() ?? "",
                    reader["Email"].ToString() ?? ""
                );
            }
            return null;
        }

        public void AddUser(string name, string email)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand(
                "INSERT INTO [User] (Naam, Email) VALUES (@Naam, @Email)", conn);
            cmd.Parameters.AddWithValue("@Naam", name);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.ExecuteNonQuery();
        }
    }
}