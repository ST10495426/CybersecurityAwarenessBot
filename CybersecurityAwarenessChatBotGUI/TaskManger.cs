using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CybersecurityAwarenessChatBotGUI
{
    public class TaskManager
    {
        public class TaskItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime? ReminderDate { get; set; }
            public bool IsCompleted { get; set; }
            public DateTime CreatedAt { get; set; }

            public string Status => IsCompleted ? "✅ Completed" : "⏳ Pending";
        }

        public string AddTask(string title, string description = "", DateTime? reminder = null)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO tasks (title, description, reminder_date) VALUES (@title, @desc, @reminder)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@desc", description ?? "");
                    cmd.Parameters.AddWithValue("@reminder", (object)reminder ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            return $"✅ Task added: '{title}'";
        }

        public List<TaskItem> GetAllTasks()
        {
            var tasks = new List<TaskItem>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM tasks ORDER BY is_completed ASC, created_at DESC";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new TaskItem
                        {
                            Id = reader.GetInt32("id"),
                            Title = reader.GetString("title"),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description"),
                            ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ? (DateTime?)null : reader.GetDateTime("reminder_date"),
                            IsCompleted = reader.GetBoolean("is_completed"),
                            CreatedAt = reader.GetDateTime("created_at")
                        });
                    }
                }
            }
            return tasks;
        }

        public string MarkComplete(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE tasks SET is_completed = TRUE WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0 ? "✅ Task marked as completed!" : "❌ Task not found.";
                }
            }
        }

        public string DeleteTask(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM tasks WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0 ? "🗑️ Task deleted." : "❌ Task not found.";
                }
            }
        }

        public string GetTasksSummary()
        {
            var tasks = GetAllTasks();
            if (tasks.Count == 0) return "📋 You have no tasks.";

            string result = "📋 Your tasks:\n";
            foreach (var t in tasks)
            {
                result += $"{t.Id}. {t.Title} - {t.Status}";
                if (t.ReminderDate.HasValue)
                    result += $" ⏰ Reminder: {t.ReminderDate.Value:yyyy-MM-dd HH:mm}";
                result += "\n";
            }
            return result.TrimEnd('\n');
        }
    }
}
