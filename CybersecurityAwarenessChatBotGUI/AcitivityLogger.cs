using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CybersecurityAwarenessChatBotGUI
{
    public class ActivityLogger
    {
        private static List<string> logEntries = new List<string>();
        private static int maxEntries = 20;

        public static void Log(string actionType, string description)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string entry = $"[{timestamp}] {actionType}: {description}";
            logEntries.Add(entry);

            // Keep only last maxEntries
            if (logEntries.Count > maxEntries)
                logEntries.RemoveAt(0);

            // Also save to database
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO activity_log (action_type, description) VALUES (@type, @desc)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@type", actionType);
                        cmd.Parameters.AddWithValue("@desc", description);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // Silently fail - log still works in memory
            }
        }

        public static string GetLog()
        {
            if (logEntries.Count == 0)
                return "📋 No activities logged yet.";

            string result = "📋 **Recent Activity Log:**\n\n";
            for (int i = logEntries.Count - 1; i >= 0; i--)
            {
                result += logEntries[i] + "\n";
            }
            return result;
        }

        public static void ClearLog()
        {
            logEntries.Clear();
            Log("System", "Activity log cleared");
        }
    }
}
