using CybersecurityAwarenessBot.CybersecurityAwarenessBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set console title
            Console.Title = "Cybersecurity Awareness Bot";

            AsciiArt art = new AsciiArt();
            art.DisplayLogo();

            // Create and run the chatbot
            ChatBot chatbot = new ChatBot();  
            chatbot.Start();
        }
    }
}
