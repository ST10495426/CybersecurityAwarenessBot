using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessChatBotGUI
{
    public class ConversationMemory
    {
        // Dictionary to store user information
        private Dictionary<string, string> userMemory = new Dictionary<string, string>();

        // Track the last topic discussed
        public string LastTopic { get; set; }

        // Track conversation history
        private List<string> conversationHistory = new List<string>();

        // Store user information
        public void Remember(string key, string value)
        {
            if (userMemory.ContainsKey(key))
            {
                userMemory[key] = value;
            }
            else
            {
                userMemory.Add(key, value);
            }
        }

        // Recall stored information
        public string Recall(string key)
        {
            if (userMemory.ContainsKey(key))
            {
                return userMemory[key];
            }
            return null;
        }

        // Check if user has a stored interest
        public string GetUserInterest()
        {
            return Recall("interest");
        }

        // Add to conversation history
        public void AddToHistory(string message)
        {
            conversationHistory.Add(message);
            // Keep only last 10 messages from conversation
            if (conversationHistory.Count > 10)
                conversationHistory.RemoveAt(0);
        }

        // Get the last bot response for the use of the "tell me more" feature
        public string GetLastBotResponse()
        {
            for (int i = conversationHistory.Count - 1; i >= 0; i--)
            {
                if (conversationHistory[i].StartsWith("Bot:"))
                {
                    return conversationHistory[i].Replace("Bot:", "").Trim();
                }
            }
            return null;
        }

        // Check if user asked for more information
        public bool WantsMoreInfo(string userInput)
        {
            string lowerInput = userInput.ToLower();
            return lowerInput.Contains("tell me more") ||
                    lowerInput.Contains("another tip") ||
                    lowerInput.Contains("more") ||
                    lowerInput.Contains("elaborate") ||
                    lowerInput.Contains("explain more") ||
                    lowerInput.Contains ( "yes" )||
                    lowerInput.Contains( "yeah") ||
                    lowerInput.Contains ("sure") ||
                    lowerInput. Contains ( "ok");
        }

        // Check if user changed topic
        public bool ChangedTopic(string userInput, string currentTopic)
        {
            if (string.IsNullOrEmpty(currentTopic)) return false;

            string lowerInput = userInput.ToLower();
            string[] topics = { "password", "scam", "phishing", "privacy", "browsing" };

            foreach (string topic in topics)
            {
                if (lowerInput.Contains(topic) && !currentTopic.ToLower().Contains(topic))
                {
                    return true;
                }
            }
            return false;
        }
        private string lastTopic = null;

        public void SetLastTopic(string topic)
        {
            lastTopic = topic;
        }

        public string GetLastTopic()
        {
            return lastTopic;
        }

        // Task History array

        private List<string> taskHistory = new List<string>();

        public void AddTaskToHistory(string task)
        {
            taskHistory.Add(task);
            if (taskHistory.Count > 10)
                taskHistory.RemoveAt(0);
        }

        public string GetLastTask()
        {
            if (taskHistory.Count == 0)
                return null;
            return taskHistory[taskHistory.Count - 1];
        }

        public int GetTaskCount()
        {
            return taskHistory.Count;
        }
    }
}
