using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessBot
{
    public class ResponseManager
    {
        // Automatic bot information properties for configuration
        public string BotName { get; set; } = "CyberBot";
        public string BotVersion { get; set; } = "1.0";

        // Dictionary to store questions and their corresponding answers
        private readonly Dictionary<string, string> responses;

        public ResponseManager()
        {
            responses = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase)
        {
            { "how are you", "I'm functioning securely, thank you for asking! How can I help you stay safe online?" },
            { "what is your purpose", $"My purpose is to educate and assist you with cybersecurity best practices. I'm {BotName } version {BotVersion}, your personal security assistant!" },
            { "what can i ask you about", "You can ask me about:\n- Password safety\n- Phishing attacks\n- Social engineering\n- Safe browsing habits\n- General cybersecurity tips" },
            {"password defination","This is a string of characters that must be keyed to gain access to a computer, network, or service or to a phone or similar device" },
            { "password safety", " Password Safety Tips:\n- Use long, complex passwords (12+ characters)\n- Never reuse passwords across sites\n- Enable Two-Factor Authentication (2FA) whenever possible\n- Consider using a password manager" },
            {"phishing defination","\n Phishing is a type of cyberattack that involves fraudulent communication, typically through emails, text messages, or websites, designed to trick individuals into revealing sensitive information such as usernames, passwords, and financial data." },
            { "phishing", " Phishing Awareness:\n- Never click suspicious links in emails or texts\n- Check sender email addresses carefully\n- Look for spelling errors and urgent language\n- When in doubt, go directly to the official website" },
            {"social engineering","Social engineering is a manipulation technique that exploits human psychology to gain access to confidential information, systems, or valuables, often leading to security breaches.An example of this techniques would be PHISHING." },
            {"cyberattack","A cyberattack occurs when an individual or organization targets computer systems, networks, or digital devices without authorization, aiming to compromise the confidentiality, integrity, or availability of information" },
            { "safe browsing", " Safe Browsing Habits:\n- Ensure websites use HTTPS (look for the padlock icon)\n- Avoid using public Wi-Fi for sensitive transactions\n- Keep your browser and extensions updated\n- Clear cookies and cache regularly" },
            { "help", "I can answer questions about:\n- Password safety\n- Phishing\n- Cyberattack\n- Safe browsing\nJust type your question or topic!" }
        };
        }

        public string GetResponse(string userInput, string userName)
        {
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return null;
            }

            // Special case for "how are you" to include user's name
            if (userInput.IndexOf("how are you", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return $"I'm functioning securely, thank you for asking! How can I help you stay safe online,?"+ userName;
            }

            // Check if the input matches any key in the dictionary
            foreach (var key in responses.Keys)
            {
                if (userInput.IndexOf(key, System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return responses[key];
                }
            }

            return null;
        }
    }
}