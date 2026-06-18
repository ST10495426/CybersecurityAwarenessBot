using System;
using MySql.Data.MySqlClient;

namespace CybersecurityAwarenessChatBotGUI
{
    public static class DatabaseHelper
    {
        
        private static string connectionString = "Server=localhost;Port=3306;Database=cybersecurity_bot;Uid=root;Pwd=My049g4amm8ng@;";

        public static MySqlConnection GetConnection()
        {
            try
            {
                return new MySqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception("Database connection failed: " + ex.Message);
            }
        }

        public static void TestConnection()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                Console.WriteLine("Database connection successful!");
            }
        }
    }
}
