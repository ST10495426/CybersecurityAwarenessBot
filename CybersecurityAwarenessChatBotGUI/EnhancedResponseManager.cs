using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessChatBotGUI
{
     public class EnhancedResponseManager
    {

        // Automatic bot information properties for configuration
        public string BotName { get; set; } = "CyberBot";
        public string BotVersion { get; set; } = "1.0";

        // Dictionary for keyword responses with MULTIPLE answers.
        private readonly Dictionary<string, List<string>> keywordResponses;

        // Random for selecting random responses 
        private Random random = new Random();

        public EnhancedResponseManager()
        {
            // KEYWORD RESPONSES WITH RANDOM VARIATIONS 
            keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            { "password", new List<string> {
                "🔐 Use strong passwords with at least 12 characters, including numbers, symbols, and both cases.",
                "🔐 Never reuse passwords across different websites. Each account needs its own unique password.",
                "🔐 Consider using a password manager to generate and store complex passwords securely.",
                "🔐 Enable Two-Factor Authentication (2FA) whenever possible for an extra layer of security."
            }},
            { "scam", new List<string> {
                "⚠️ Never share personal information with unsolicited callers or emails.",
                "⚠️ Scammers often create urgency - 'Act now or your account will be closed!' Always verify independently.",
                "⚠️ If something sounds too good to be true, it probably is. Trust your instincts.",
                "⚠️ Report scams to the relevant authorities like your bank or the FTC."
            }},
            { "privacy", new List<string> {
                "🛡️ Review your social media privacy settings regularly.",
                "🛡️ Be careful what personal information you share online - once it's out, it's hard to remove.",
                "🛡️ Use a VPN on public Wi-Fi to protect your browsing privacy.",
                "🛡️ Regularly clear your browser cookies and cache."
            }},
            { "phishing", new List<string> {
                "🎣 Check the sender's email address carefully - scammers use addresses that look similar to real ones.",
                "🎣 Never click links in suspicious emails. Hover over them first to see the actual URL.",
                "🎣 Look for spelling and grammar errors - these are common in phishing attempts.",
                "🎣 When in doubt, go directly to the official website instead of using email links."
            }},
            { "browsing", new List<string> {
                "🌐 Always look for 'https://' and the padlock icon in the address bar.",
                "🌐 Keep your browser and extensions updated to the latest versions.",
                "🌐 Use ad-blockers and privacy extensions for safer browsing.",
                "🌐 Avoid saving passwords in your browser - use a dedicated password manager instead."
            }},
            { "safe browsing", new List<string> {
                "🌐 Always look for 'https://' and the padlock icon in the address bar.",
                "🌐 Keep your browser and extensions updated to the latest versions.",
                "🌐 Clear your cookies and cache regularly.",
                "🌐 Use a VPN on public Wi-Fi networks."
            }}
        };
        }

        // Detect which keyword is in the user's message
        public string DetectKeyword(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return null;

            string lowerInput = userInput.ToLower();

            foreach (string keyword in keywordResponses.Keys)
            {
                if (lowerInput.Contains(keyword))
                {
                    return keyword;
                }
            }
            return null;
        }

        // Get a RANDOM response for a keyword 
        public string GetRandomResponse(string keyword)
        {
            if (keywordResponses.ContainsKey(keyword))
            {
                List<string> responses = keywordResponses[keyword];
                int randomIndex = random.Next(responses.Count);
                return responses[randomIndex];
            }
            return null;
        }

        // Get all available keywords for help display
        public List<string> GetAvailableKeywords()
        {
            return new List<string>(keywordResponses.Keys);
        }
        
      
      }
}

