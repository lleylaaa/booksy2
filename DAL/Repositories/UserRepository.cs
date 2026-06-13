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
            var cmd = new SqlCommand(
                "SELECT GebruikerID, Naam, Email, WachtwoordHash FROM [User] WHERE GebruikerID = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            return ReadOne(cmd);
        }

        public UserDTO? GetUserByEmail(string email)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand(
                "SELECT GebruikerID, Naam, Email, WachtwoordHash FROM [User] WHERE Email = @email", conn);
            cmd.Parameters.AddWithValue("@email", email);
            return ReadOne(cmd);
        }

        public int AddUser(string name, string email, string passwordHash)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand(
                "INSERT INTO [User] (Naam, Email, WachtwoordHash) OUTPUT INSERTED.GebruikerID VALUES (@Naam, @Email, @Hash)", conn);
            cmd.Parameters.AddWithValue("@Naam", name);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Hash", passwordHash);
            return (int)cmd.ExecuteScalar();
        }

        private static UserDTO? ReadOne(SqlCommand cmd)
        {
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new UserDTO(
                    (int)reader["GebruikerID"],
                    reader["Naam"].ToString() ?? "",
                    reader["Email"].ToString() ?? "",
                    reader["WachtwoordHash"].ToString() ?? ""
                );
            }
            return null;
        }
    }
}
