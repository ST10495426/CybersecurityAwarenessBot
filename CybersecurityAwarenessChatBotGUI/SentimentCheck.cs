using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessChatBotGUI
{
    public class SentimentCheck
    {
        // Keywords for different sentiments
        private readonly List<string> worriedKeywords = new List<string>
    {
        "worried", "scared", "nervous", "anxious", "afraid", "concerned", "unsafe"
    };

        private readonly List<string> curiousKeywords = new List<string>
    {
        "curious", "interested", "want to learn", "tell me", "explain", "how does"
    };

        private readonly List<string> frustratedKeywords = new List<string>
    {
        "frustrated", "annoyed", "confused", "difficult", "hard", "don't understand"
    };

        private readonly List<string> happyKeywords = new List<string>
    {
        "happy", "great", "good", "thanks", "thank you", "awesome", "helpful"
    };

        // Method to detect sentiment from user input
        public string DetectSentiment(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "neutral";

            string lowerInput = userInput.ToLower();

            foreach (string keyword in worriedKeywords)
            {
                if (lowerInput.Contains(keyword))
                    return "worried";
            }

            foreach (string keyword in frustratedKeywords)
            {
                if (lowerInput.Contains(keyword))
                    return "frustrated";
            }

            foreach (string keyword in curiousKeywords)
            {
                if (lowerInput.Contains(keyword))
                    return "curious";
            }

            foreach (string keyword in happyKeywords)
            {
                if (lowerInput.Contains(keyword))
                    return "happy";
            }

            return "neutral";
        }

        // Get an empathetic response based on sentiment
        public string GetEmpatheticResponse(string sentiment, string topic)
        {
            switch (sentiment)
            {
                case "worried":
                    return "It's completely understandable to feel worried about " + topic + ". Let me share some tips to help you feel more secure.\n\n";
                case "frustrated":
                    return "I understand this can be frustrating. Cybersecurity can feel overwhelming. Let me break it down simply for you.\n\n";
                case "curious":
                    return "That's great that you're curious about " + topic + "! Being interested in security is the first step to staying safe online.\n\n";
                case "happy":
                    return "I'm glad you're feeling positive about cybersecurity! Let me share some useful information with you.\n\n";
                default:
                    return "";
            }
        }
    }
}

